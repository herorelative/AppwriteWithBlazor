using AppwriteWithBlazor.Helpers;
using System.Text.Json;

namespace AppwriteWithBlazor.Providers
{
    public interface IAppwriteCollectionRepository<T>
    {
        Task<List<T>> GetAll();
        Task<T> GetById(string documentId);
        Task<string> Create(T entity);
        Task Update(string documentId, T entity);
        Task Delete(string documentId);
    }

    public class AppwriteCollectionRepository<T> : IAppwriteCollectionRepository<T>
    {
        private readonly HttpClient _client;
        private readonly IAppStates _states;
        private readonly string _databaseId;
        private readonly string _collectionId;
        private readonly string _endpoint;

        public AppwriteCollectionRepository(IHttpClientFactory clientFactory, IAppStates states, string databaseId, string collectionId)
        {
            _client = clientFactory.CreateClient("APPWRITE");
            _states = states;
            _databaseId = databaseId;
            _collectionId = collectionId;
            _endpoint = "/v1/databases/{databaseId}/collections/{collectionId}/documents";
        }

        public AppwriteCollectionRepository(HttpClient client, IAppStates states, string databaseId, string collectionId)
        {
            _client = client;
            _states = states;
            _databaseId = databaseId;
            _collectionId = collectionId;
            _endpoint = "/v1/databases/{databaseId}/collections/{collectionId}/documents";
        }

        public async Task<List<T>> GetAll()
        {
            var url = _endpoint
                .Replace("{databaseId}", _databaseId)
                .Replace("{collectionId}", _collectionId);
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            request.Headers.TryAddWithoutValidation("Cookie", await _states.GetToken());
            request.Headers.TryAddWithoutValidation("X-Fallback-Cookies", await _states.GetToken());

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            // var document = JsonSerializer.Deserialize<Document<List<T>>>(json, ExtensionMethods.DeserializerSettings);

            return new List<T>();
        }

        public Task<T> GetById(string documentId)
        {
            throw new NotImplementedException();
        }

        public async Task<string> Create(T entity)
        {
            var url = _endpoint
                .Replace("{databaseId}", _databaseId)
                .Replace("{collectionId}", _collectionId);
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            request.Headers.TryAddWithoutValidation("Cookie", await _states.GetToken());
            request.Headers.TryAddWithoutValidation("X-Fallback-Cookies", await _states.GetToken());

            var json = JsonSerializer.Serialize(entity, ExtensionMethods.SerializerSettings);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            request.Content = content;

            HttpResponseMessage response = await _client.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var locationHeader = response.Headers.Location.ToString();
            var documentId = locationHeader.Substring(locationHeader.LastIndexOf('/') + 1);

            return documentId;
        }

        public Task Update(string documentId, T entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(string documentId)
        {
            throw new NotImplementedException();
        }
    }
}
