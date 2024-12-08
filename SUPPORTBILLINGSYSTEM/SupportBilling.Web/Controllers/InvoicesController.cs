using Microsoft.AspNetCore.Mvc;
using SupportBilling.Web.Models;
using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Rendering;
using SupportBilling.DOMAIN.Entities;

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
                        Status = i.Status?.Name ?? "Unknown"
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
            Console.WriteLine("Método Create POST ha sido llamado.");

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState)
                {
                    foreach (var subError in error.Value.Errors)
                    {
                        Console.WriteLine($"Error en {error.Key}: {subError.ErrorMessage}");
                    }
                }
                ModelState.AddModelError(string.Empty, "Hay errores en los datos proporcionados.");
                await LoadClientsAndServicesAsync();
                await LoadStatusesAsync();
                return View(invoice);
            }

            try
            {
                var jsonInvoice = JsonSerializer.Serialize(invoice);
                Console.WriteLine("Payload enviado al servidor: " + jsonInvoice);

                var content = new StringContent(jsonInvoice, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("Invoices", content);

                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Respuesta del servidor: " + responseBody);

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

            await LoadClientsAndServicesAsync();
            await LoadStatusesAsync();
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
