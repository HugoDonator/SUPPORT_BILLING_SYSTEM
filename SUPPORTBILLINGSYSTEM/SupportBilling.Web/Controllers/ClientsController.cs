using Microsoft.AspNetCore.Mvc;
using SupportBilling.Web.Services;
using SupportBilling.Web.Models;

namespace SupportBilling.Web.Controllers
{
    public class ClientsController : Controller
    {
        private readonly ApiService _apiService;

        public ClientsController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var clients = await _apiService.GetAsync<List<ClientDto>>("Clients");
            return View(clients);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ClientDto clientDto)
        {
            await _apiService.PostAsync("Clients", clientDto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var client = await _apiService.GetAsync<ClientDto>($"Clients/{id}");
            return View(client);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ClientDto clientDto)
        {
            await _apiService.PutAsync($"Clients/{clientDto.Id}", clientDto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var client = await _apiService.GetAsync<ClientDto>($"Clients/{id}");
            return View(client);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _apiService.DeleteAsync($"Clients/{id}");
            return RedirectToAction("Index");
        }
    }
}
