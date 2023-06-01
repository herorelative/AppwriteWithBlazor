using AppwriteWithBlazor.Helpers;
using AppwriteWithBlazor.Models;
using System.Net.Http.Json;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace AppwriteWithBlazor.Providers
{
    public interface ITodoProvider
    {
        Task<Document<List<Todo>>> List(string projectId);
        Task<bool> Create(TodoCreateModel model);
        Task<bool> Update(string id, TodoCreateModel model);
        Task<bool> Delete(string id);
    }

    public class TodoProvider : ITodoProvider
    {
        private readonly HttpClient _client;
        private readonly IAppStates _states;
        private readonly string _databaseId;
        private readonly string _collectionId;
        private readonly SimpleAuthStateProvider _authorizationService;

        public TodoProvider(IHttpClientFactory clientFactory, IAppStates states, string databaseId, string collectionId, SimpleAuthStateProvider authorizationService)
        {
            _client = clientFactory.CreateClient("APPWRITE");
            _states = states;
            _databaseId = databaseId;
            _collectionId = collectionId;
            _authorizationService = authorizationService;
        }

        #region list
        public async Task<Document<List<Todo>>> List(string projectId)
        {
            HttpRequestMessage request;
            if(projectId == null)
            {
                request = new(HttpMethod.Get, $"/v1/databases/{_databaseId}/collections/{_collectionId}/documents");
            }
            else
            {
                request = new(HttpMethod.Get, $"/v1/databases/{_databaseId}/collections/{_collectionId}/documents?queries[]=equal(\"projectId\",[{projectId}])");
            }

            request.Headers.TryAddWithoutValidation("Cookie", await _states.GetToken());
            request.Headers.TryAddWithoutValidation("X-Fallback-Cookies", await _states.GetToken());

            HttpResponseMessage response = await _client.SendAsync(request);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<Document<List<Todo>>>() ?? new Document<List<Todo>>();
        }
        #endregion

        #region create
        public async Task<bool> Create(TodoCreateModel model)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, $"/v1/databases/{_databaseId}/collections/{_collectionId}/documents");

                request.Headers.TryAddWithoutValidation("Cookie", await _states.GetToken());
                request.Headers.TryAddWithoutValidation("X-Fallback-Cookies", await _states.GetToken());

                var user = await _authorizationService.GetCurrentUser();

                DocumentCreate<TodoCreateModel> document = new()
                {
                    DocumentId = Guid.NewGuid().ToString(),
                    Data = model,
                };

                //only record owner can update and delete
                document.Permissions.Add($"read(\"user:{user.UserId}\")");
                document.Permissions.Add($"update(\"user:{user.UserId}\")");
                document.Permissions.Add($"delete(\"user:{user.UserId}\")");

                var modelItemJson = new StringContent(
                    JsonSerializer.Serialize(document, ExtensionMethods.SerializerSettings),
                    encoding: System.Text.Encoding.UTF8,
                    Application.Json
                );

                request.Content = modelItemJson;

                HttpResponseMessage response = await _client.SendAsync(request);

                response.EnsureSuccessStatusCode();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        #endregion

        #region update
        public async Task<bool> Update(string id, TodoCreateModel model)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Patch, $"/v1/databases/{_databaseId}/collections/{_collectionId}/documents/{id}");

                request.Headers.TryAddWithoutValidation("Cookie", await _states.GetToken());
                request.Headers.TryAddWithoutValidation("X-Fallback-Cookies", await _states.GetToken());

                var user = await _authorizationService.GetCurrentUser();

                DocumentCreate<TodoCreateModel> document = new()
                {
                    DocumentId = id,
                    Data = model
                };

                //only record owner can update and delete
                document.Permissions.Add($"read(\"user:{user.UserId}\")");
                document.Permissions.Add($"update(\"user:{user.UserId}\")");
                document.Permissions.Add($"delete(\"user:{user.UserId}\")");

                var modelItemJson = new StringContent(
                    JsonSerializer.Serialize(document, ExtensionMethods.SerializerSettings),
                    encoding: System.Text.Encoding.UTF8,
                    Application.Json
                );

                request.Content = modelItemJson;

                HttpResponseMessage response = await _client.SendAsync(request);

                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        #endregion

        #region delete
        public async Task<bool> Delete(string id)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Delete, $"/v1/databases/{_databaseId}/collections/{_collectionId}/documents/{id}");

                request.Headers.TryAddWithoutValidation("Cookie", await _states.GetToken());
                request.Headers.TryAddWithoutValidation("X-Fallback-Cookies", await _states.GetToken());

                HttpResponseMessage response = await _client.SendAsync(request);

                response.EnsureSuccessStatusCode();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        #endregion
    }
}
