using Microsoft.JSInterop;
using System.Threading.Tasks;
using Tujyane.Models;

namespace Tujyane.Services
{
    public class AuthService
    {
        private readonly IJSRuntime _jsRuntime;

        public AppwriteUser? CurrentUser { get; private set; }

        public event Action? OnChange; // let components subscribe

        public AuthService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        private void NotifyStateChanged() => OnChange?.Invoke();

        public async Task Initialize()
        {
            CurrentUser = await GetCurrentUser();
            NotifyStateChanged();
        }

        public async Task<AppwriteUser?> GetCurrentUser()
        {
            try
            {
                var user = await _jsRuntime.InvokeAsync<AppwriteUser>("getCurrentUser");
                CurrentUser = user;
                NotifyStateChanged();
                return user;
            }
            catch
            {
                return null;
            }
        }

        public async Task Register(string email, string password, string name)
        {
            await _jsRuntime.InvokeVoidAsync("registerUser", email, password, name);
            await Initialize(); // refresh CurrentUser
        }

        public async Task Login(string email, string password)
        {
            var currentUser = await _jsRuntime.InvokeAsync<AppwriteUser>("getCurrentUser");
            if (currentUser != null)
            {
                Console.WriteLine("Already logged in, skipping new session.");
                CurrentUser = currentUser;
                await Initialize();
                return;
            }
            await _jsRuntime.InvokeVoidAsync("loginUser", email, password);
            await Initialize(); // refresh CurrentUser
        }

        public async Task Logout()
        {
            await _jsRuntime.InvokeVoidAsync("logoutUser");
            CurrentUser = null;
            NotifyStateChanged();
        }
    }
}
