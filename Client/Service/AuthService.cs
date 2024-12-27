
using Client.Model;
using System.Net.Http.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Client.Service
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<AuthResponse> LoginAsync(LoginModel loginModel)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/Auth/Login", loginModel);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<AuthResponse>();
                }
                else
                {
                   
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Đăng nhập thất bại: {errorMessage}");
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Lối server: {ex.Message}");
            }

        }
    }
}
