using AppwriteWithBlazor.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace AppwriteWithBlazor.Providers
{
    public class SimpleAuthStateProvider : AuthenticationStateProvider
    {
        private readonly IAuthProvider _auth;
        private CurrentUser _currentUser;

        public SimpleAuthStateProvider(IAuthProvider auth)
        {
            _auth = auth;
        }

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            try
            {
                var userInfo = await GetCurrentUser();
                if (userInfo != null)
                {
                    var claims = new[] {
                        new Claim(ClaimTypes.Name, _currentUser.Email),
                        new Claim(ClaimTypes.NameIdentifier, _currentUser.UserId)
                    };

                    identity = new ClaimsIdentity(claims, "Server authentication");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Request failed:" + ex.ToString());
            }

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        public async Task<CurrentUser> GetCurrentUser()
        {
            if (_currentUser != null) return _currentUser;
            _currentUser = await _auth.CurrentUserInfo();
            return _currentUser;
        }

        public async Task Logout()
        {
            await _auth.Logout();
            _currentUser = null;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task Login(UserCreateModel loginParameters)
        {
            await _auth.Login(loginParameters);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task Register(UserCreateModel registerParameters)
        {
            await _auth.Register(registerParameters);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
