using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Tujyane.Services
{
    public class AuthService
    {
        private readonly IJSRuntime _jsRuntime;

        // Constructor injection
        public AuthService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task Login(string email, string password)
        {
            // Call the JavaScript loginUser function
            await _jsRuntime.InvokeVoidAsync("loginUser", email, password);
        }
        public async Task Register(string email, string password, string name)
        {
            // Call the JavaScript registerUser function
            await _jsRuntime.InvokeVoidAsync("registerUser", email, password, name);
        }
    }
}
