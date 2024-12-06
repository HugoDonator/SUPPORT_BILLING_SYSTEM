using Microsoft.AspNetCore.Mvc;
using SupportBilling.APPLICATION.Dtos;
using SupportBilling.Web.Models;
using SupportBilling.Web.Services;
using ServiceDto = SupportBilling.Web.Models.ServiceDto;

namespace SupportBilling.Web.Controllers
{
    public class ServicesController : Controller
    {
        private readonly ApiService _apiService;

        public ServicesController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var services = await _apiService.GetAsync<List<ServiceDto>>("Services");
            return View(services);
        }

        public IActionResult Create()
        {
            return View(new ServiceDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create(ServiceDto service)
        {
            await _apiService.PostAsync("Services", service);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var service = await _apiService.GetAsync<ServiceDto>($"Services/{id}");
            return View(service);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ServiceDto service)
        {
            await _apiService.PutAsync($"Services/{service.Id}", service);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _apiService.DeleteAsync($"Services/{id}");
            return RedirectToAction("Index");
        }

        
        
    }
}
