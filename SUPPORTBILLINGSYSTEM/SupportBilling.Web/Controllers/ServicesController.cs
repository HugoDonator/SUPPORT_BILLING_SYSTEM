using Microsoft.AspNetCore.Mvc;
using SupportBilling.Web.Models;
using System.Text;
using System.Text.Json;

namespace SupportBilling.Web.Controllers
{
    public class ServicesController : Controller
    {
        private readonly HttpClient _httpClient;

        public ServicesController(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(configuration["ApiSettings:BaseUrl"]);
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("Services");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var services = JsonSerializer.Deserialize<List<ServiceViewModel>>(data, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return View(services);
            }
            return View(new List<ServiceViewModel>());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceViewModel service)
        {
            if (!ModelState.IsValid)
            {
                return View(service);
            }

            try
            {
                var jsonService = JsonSerializer.Serialize(service);
                var content = new StringContent(jsonService, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("Services", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, $"Error: {response.ReasonPhrase}");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Unexpected error: {ex.Message}");
            }

            return View(service);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"Services/{id}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var service = JsonSerializer.Deserialize<ServiceViewModel>(data, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return View(service);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ServiceViewModel service)
        {
            if (!ModelState.IsValid)
            {
                return View(service);
            }

            try
            {
                var jsonService = JsonSerializer.Serialize(service);
                var content = new StringContent(jsonService, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"Services/{service.Id}", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, $"Error: {response.ReasonPhrase}");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Unexpected error: {ex.Message}");
            }

            return View(service);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"Services/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }
    }
}
