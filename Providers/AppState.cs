using AppwriteWithBlazor.Models;
using Microsoft.JSInterop;

namespace AppwriteWithBlazor.Providers
{
    public interface IAppStates
    {
        Task SetToken(string value);
        Task<string> GetToken();
        Task RemoveToken();
        Project GetCurrentProject();
        void SetCurrentProject(Project data);
    }

    public class AppState : IAppStates
    {
        private readonly IJSRuntime _jsRuntime;
        private Project _currentProject;

        public AppState(IJSRuntime jSRuntime)
        {
            _jsRuntime = jSRuntime;
        }

        public async Task SetToken(string value)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "token", value);
        }

        public async Task<string> GetToken()
        {
            return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "token");
        }

        public async Task RemoveToken()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "token");
        }

        public Project GetCurrentProject()
        {
            return _currentProject;
        }

        public void SetCurrentProject(Project data)
        {
            _currentProject = data;
        }
    }
}
