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
            try
            {
                CurrentUser = await _jsRuntime.InvokeAsync<AppwriteUser>("getCurrentUser");
            }
            catch
            {
                CurrentUser = null; // means no session yet
            }
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
            try
            {
                // First, try to login
                await _jsRuntime.InvokeVoidAsync("loginUser", email, password);

                // Then fetch the user
                CurrentUser = await _jsRuntime.InvokeAsync<AppwriteUser>("getCurrentUser");
                NotifyStateChanged();
            }
            catch (JSException ex)
            {
                Console.WriteLine($"Login failed: {ex.Message}");
                throw; // rethrow so UI can show error
            }
        }


        public async Task Logout()
        {
            await _jsRuntime.InvokeVoidAsync("logoutUser");
            CurrentUser = null;
            NotifyStateChanged();
        }
    }
}
