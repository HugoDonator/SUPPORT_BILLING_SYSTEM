using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SupportBilling.Web.Models;
using System.Text;
using System.Text.Json;

namespace SupportBilling.Web.Controllers
{
    public class ClientsController : Controller
    {
        private readonly HttpClient _httpClient;

        // Inyección del HttpClient a través de DI
        public ClientsController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(configuration["ApiSettings:BaseUrl"]);
        }

        // Listar todos los clientes
        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetAsync("Clients");

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var clients = JsonSerializer.Deserialize<List<ClientViewModel>>(data, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return View(clients);
                }
                else
                {
                    // Manejo de error cuando la API no responde correctamente
                    ModelState.AddModelError(string.Empty, $"Error: {response.ReasonPhrase}");
                    return View(new List<ClientViewModel>());
                }
            }
            catch (Exception ex)
            {
                // Captura de errores inesperados
                ModelState.AddModelError(string.Empty, $"An unexpected error occurred: {ex.Message}");
                return View(new List<ClientViewModel>());
            }
        }

        // Vista para crear un cliente
        public IActionResult Create()
        {
            return View();
        }

        // Acción para crear un cliente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClientViewModel client)
        {
            // Verificar si el modelo es válido
            if (!ModelState.IsValid)
            {
                return View(client);
            }

            try
            {
                // Serializar el objeto ClientViewModel a JSON
                var jsonClient = JsonSerializer.Serialize(client);
                var content = new StringContent(jsonClient, Encoding.UTF8, "application/json");

                // Enviar solicitud POST a la API
                var response = await _httpClient.PostAsync("Clients", content);

                // Verificar la respuesta de la API
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index)); // Redirige a la lista de clientes
                }

                // Si hay un error, agregar un mensaje a ModelState
                ModelState.AddModelError(string.Empty, $"Error: {response.ReasonPhrase}");
            }
            catch (Exception ex)
            {
                // Manejar errores inesperados
                ModelState.AddModelError(string.Empty, $"An unexpected error occurred: {ex.Message}");
            }

            return View(client);
        }

        // Acción para editar un cliente
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Clients/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var client = JsonSerializer.Deserialize<ClientViewModel>(data, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return View(client);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, $"Error: {response.ReasonPhrase}");
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An unexpected error occurred: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        // Acción para actualizar un cliente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ClientViewModel client)
        {
            if (!ModelState.IsValid)
            {
                return View(client);
            }

            try
            {
                var jsonClient = JsonSerializer.Serialize(client);
                var content = new StringContent(jsonClient, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"Clients/{client.Id}", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, $"Error: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An unexpected error occurred: {ex.Message}");
            }

            return View(client);
        }

        // Acción para eliminar un cliente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"Clients/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, $"Error: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An unexpected error occurred: {ex.Message}");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
