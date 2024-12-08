using Microsoft.AspNetCore.Mvc;
using SupportBilling.Web.Models;
using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Rendering;
using SupportBilling.DOMAIN.Entities;
using SupportBilling.Web.DTOs;

namespace SupportBilling.Web.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly HttpClient _httpClient;

        public InvoicesController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(configuration["ApiSettings:BaseUrl"]);
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetAsync("Invoices");

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var invoices = JsonSerializer.Deserialize<List<Invoice>>(data, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    var viewModel = invoices.Select(i => new InvoiceViewModel
                    {
                        Id = i.Id,
                        ClientName = i.Client?.Name ?? "Unknown",
                        InvoiceDate = i.InvoiceDate,
                        TotalAmount = i.TotalAmount,
                        Status = i.Status?.Name ?? "Pending"
                    }).ToList();

                    return View(viewModel);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, $"Error: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Unexpected error: {ex.Message}");
            }

            return View(new List<InvoiceViewModel>());
        }

        public async Task<IActionResult> Create()
        {
            await LoadClientsAndServicesAsync();
            await LoadStatusesAsync();
            return View(new InvoiceViewModel { InvoiceDate = DateTime.Now, Status = "Pending" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InvoiceViewModel invoice)
        {
            if (!ModelState.IsValid)
            {
                await LoadClientsAndServicesAsync();
                return View(invoice);
            }

            if (string.IsNullOrEmpty(invoice.Status))
            {
                invoice.Status = "Pending"; // Asignar valor predeterminado
            }
            try
            {
                // Serializa el modelo para enviarlo a la API
                var jsonInvoice = JsonSerializer.Serialize(invoice);

                // Agrega el Console.WriteLine aquí
                Console.WriteLine("Datos enviados al servidor: " + jsonInvoice);

                var content = new StringContent(jsonInvoice, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("Invoices", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, "Error: Could not create invoice.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
            }

            await LoadClientsAndServicesAsync();
            return View(invoice);
        }

        private async Task LoadClientsAndServicesAsync()
        {
            var responseClients = await _httpClient.GetAsync("Clients");
            ViewBag.Clients = responseClients.IsSuccessStatusCode
                ? new SelectList(JsonSerializer.Deserialize<List<ClientViewModel>>(await responseClients.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }), "Id", "Name")
                : new SelectList(new List<ClientViewModel>(), "Id", "Name");

            var responseServices = await _httpClient.GetAsync("Services");
            ViewBag.Services = responseServices.IsSuccessStatusCode
                ? JsonSerializer.Deserialize<List<ServiceViewModel>>(await responseServices.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                : new List<ServiceViewModel>();
        }

        private Task LoadStatusesAsync()
        {
            ViewBag.Statuses = new SelectList(new List<string> { "Pending", "Paid" });
            return Task.CompletedTask;
        }
    }
}
