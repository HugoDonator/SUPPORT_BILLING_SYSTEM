using System.Net.Http;
using System.Text.Json;

namespace SupportBilling.Web.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        // Constructor que recibe HttpClient e IConfiguration
        public ApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiUrl = configuration["ApiUrl"]; // Asigna el valor desde appsettings.json
        }

        // Método para realizar solicitudes GET
        public async Task<T> GetAsync<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}{endpoint}");
            response.EnsureSuccessStatusCode(); // Lanza una excepción si la respuesta no es 2xx
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        // Método para realizar solicitudes POST
        public async Task PostAsync<T>(string endpoint, T data)
        {
            var content = new StringContent(JsonSerializer.Serialize(data), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_apiUrl}{endpoint}", content);
            response.EnsureSuccessStatusCode(); // Verifica que la respuesta sea exitosa
        }

        // Método para realizar solicitudes PUT
        public async Task PutAsync<T>(string endpoint, T data)
        {
            try
            {
                var content = new StringContent(JsonSerializer.Serialize(data), System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{_apiUrl}{endpoint}", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error al realizar PUT en {endpoint}. Código de estado: {response.StatusCode}. Respuesta: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error de conexión al endpoint {endpoint}: {ex.Message}");
            }
        }

        // Método para realizar solicitudes DELETE
        public async Task DeleteAsync(string endpoint)
        {
            var response = await _httpClient.DeleteAsync($"{_apiUrl}{endpoint}");
            response.EnsureSuccessStatusCode();
        }
    }
}
