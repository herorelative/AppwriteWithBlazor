using AppwriteWithBlazor.Helpers;
using AppwriteWithBlazor.Models;
using System.Net.Http.Json;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace AppwriteWithBlazor.Providers
{
    public interface IProjectProvider
    {
        Task<Document<List<Project>>> List();
        Task<bool> Create(ProjectCreateModel model);
        Task<bool> Update(string id, ProjectCreateModel model);
        Task<bool> Delete(string id);
    }

    public class ProjectProvider : IProjectProvider
    {
        private readonly HttpClient _client;
        private readonly IAppStates _states;
        private readonly string _databaseId;
        private readonly string _collectionId;
        private readonly SimpleAuthStateProvider _authorizationService;

        public ProjectProvider(IHttpClientFactory clientFactory, IAppStates states, string databaseId, string collectionId, SimpleAuthStateProvider authorizationService)
        {
            _client = clientFactory.CreateClient("APPWRITE");
            _states = states;
            _databaseId = databaseId;
            _collectionId = collectionId;
            _authorizationService = authorizationService;
        }

        #region list
        public async Task<Document<List<Project>>> List()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/v1/databases/{_databaseId}/collections/{_collectionId}/documents");

            request.Headers.TryAddWithoutValidation("Cookie", await _states.GetToken());
            request.Headers.TryAddWithoutValidation("X-Fallback-Cookies", await _states.GetToken());

            HttpResponseMessage response = await _client.SendAsync(request);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<Document<List<Project>>>() ?? new Document<List<Project>>();
        }
        #endregion

        #region create
        public async Task<bool> Create(ProjectCreateModel model)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, $"/v1/databases/{_databaseId}/collections/{_collectionId}/documents");

                request.Headers.TryAddWithoutValidation("Cookie", await _states.GetToken());
                request.Headers.TryAddWithoutValidation("X-Fallback-Cookies", await _states.GetToken());

                var user = await _authorizationService.GetCurrentUser();

                DocumentCreate<ProjectCreateModel> document = new()
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
        public async Task<bool> Update(string id, ProjectCreateModel model)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Patch, $"/v1/databases/{_databaseId}/collections/{_collectionId}/documents/{id}");

                request.Headers.TryAddWithoutValidation("Cookie", await _states.GetToken());
                request.Headers.TryAddWithoutValidation("X-Fallback-Cookies", await _states.GetToken());

                var user = await _authorizationService.GetCurrentUser();

                DocumentCreate<ProjectCreateModel> document = new()
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
