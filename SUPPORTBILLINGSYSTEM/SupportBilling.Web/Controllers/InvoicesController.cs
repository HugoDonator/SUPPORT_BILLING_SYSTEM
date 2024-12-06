using Microsoft.AspNetCore.Mvc;
using SupportBilling.APPLICATION.Dtos;
using SupportBilling.Web.Models;
using SupportBilling.Web.Services;
using ClientDto = SupportBilling.Web.Models.ClientDto;
using CreateInvoiceDto = SupportBilling.Web.Models.CreateInvoiceDto;
using InvoiceDto = SupportBilling.Web.Models.InvoiceDto;
using PaymentDto = SupportBilling.Web.Models.PaymentDto;
using ServiceDto = SupportBilling.Web.Models.ServiceDto;

namespace SupportBilling.Web.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly ApiService _apiService;

        public InvoicesController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // Lista de Facturas
        public async Task<IActionResult> Index()
        {
            var invoices = await _apiService.GetAsync<List<InvoiceDto>>("Invoices");
            return View(invoices);
        }

        // Detalles de una Factura
        public async Task<IActionResult> Details(int id)
        {
            var invoice = await _apiService.GetAsync<InvoiceDto>($"Invoices/{id}");
            if (invoice == null)
            {
                return NotFound();
            }
            return View(invoice);
        }

        // GET: Crear Factura
        public async Task<IActionResult> Create()
        {
            try
            {
                var clients = await _apiService.GetAsync<List<ClientDto>>("Clients");
                var services = await _apiService.GetAsync<List<ServiceDto>>("Services");

                if (clients == null || clients.Count == 0)
                {
                    ModelState.AddModelError("", "No hay clientes disponibles. Registre clientes antes de crear una factura.");
                }

                if (services == null || services.Count == 0)
                {
                    ModelState.AddModelError("", "No hay servicios disponibles. Registre servicios antes de crear una factura.");
                }

                var model = new CreateInvoiceViewModel
                {
                    Clients = clients,
                    Services = services,
                    SelectedServices = new List<CreateInvoiceDetailDto>() // Inicializa la lista vacía
                };

                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al cargar los datos. " + ex.Message);
                return RedirectToAction("Index");
            }
        }

        // POST: Crear Factura
        [HttpPost]
        public async Task<IActionResult> Create(CreateInvoiceViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Si el modelo no es válido, recargar datos y retornar vista con errores
                    var clients = await _apiService.GetAsync<List<ClientDto>>("Clients");
                    var services = await _apiService.GetAsync<List<ServiceDto>>("Services");
                    model.Clients = clients;
                    model.Services = services;
                    return View(model);
                }

                // Cálculos necesarios para la factura
                decimal subtotal = 0;
                foreach (var service in model.SelectedServices)
                {
                    var selectedService = model.Services.FirstOrDefault(s => s.Id == service.ServiceId);
                    if (selectedService != null)
                    {
                        subtotal += selectedService.Price * service.Quantity;
                    }
                }

                decimal taxRate = 0.18m; // Tasa fija de impuestos del 18%
                decimal tax = subtotal * taxRate;
                decimal total = subtotal + tax;

                // Mapeo de los detalles de la factura seleccionados
                var invoiceDetails = model.SelectedServices.Select(s => new CreateInvoiceDetailDto
                {
                    ServiceId = s.ServiceId,
                    Quantity = s.Quantity
                }).ToList();

                // Construcción de la factura completa para enviar a la API
                var invoice = new CreateInvoiceDto
                {
                    ClientId = model.SelectedClientId,
                    Details = invoiceDetails,
                    Subtotal = subtotal,
                    Tax = tax,
                    Total = total
                };

                // Llamada a la API para guardar la factura
                await _apiService.PostAsync("Invoices", invoice);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al crear la factura. " + ex.Message);
                return View(model);
            }
        }

        // GET: Registrar un Pago
        public IActionResult Payments(int id)
        {
            var payment = new PaymentDto { InvoiceId = id };
            return View(payment);
        }

        // POST: Registrar un Pago
        [HttpPost]
        public async Task<IActionResult> Payments(PaymentDto paymentDto)
        {
            try
            {
                await _apiService.PostAsync("Payments", paymentDto);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al registrar el pago. " + ex.Message);
                return View(paymentDto);
            }
        }
    }
}
