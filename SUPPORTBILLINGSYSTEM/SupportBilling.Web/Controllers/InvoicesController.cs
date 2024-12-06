using Microsoft.AspNetCore.Mvc;
using SupportBilling.APPLICATION.Dtos;
using SupportBilling.DOMAIN.Entities;
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

            foreach(var invoice in invoices)
    {
                if (invoice.Total == 0)
                {
                    // Calcula el total si no está definido
                    invoice.Total = invoice.Subtotal + invoice.Tax;
                }
            }

            return View(invoices);
        }

        // Detalles de una Factura
        public async Task<IActionResult> Details(int id)
        {
            var invoice = await _apiService.GetAsync<InvoiceDto>($"Invoices/{id}");
            if (invoice == null)
            {
                TempData["ErrorMessage"] = "Factura no encontrada.";
                return RedirectToAction("Index");
            }
            return View(invoice);
        }

        // GET: Crear Factura
        public async Task<IActionResult> Create()
        {
            var clients = await _apiService.GetAsync<List<ClientDto>>("Clients");
            var services = await _apiService.GetAsync<List<ServiceDto>>("Services");

            if (clients == null || !clients.Any())
            {
                TempData["ErrorMessage"] = "No hay clientes disponibles. Registre clientes antes de crear una factura.";
                return RedirectToAction("Index", "Clients");
            }

            if (services == null || !services.Any())
            {
                TempData["ErrorMessage"] = "No hay servicios disponibles. Registre servicios antes de crear una factura.";
                return RedirectToAction("Index", "Services");
            }

            var model = new CreateInvoiceViewModel
            {
                Clients = clients,
                Services = services,
                SelectedServices = new List<CreateInvoiceDetailDto>()
            };

            return View(model);
        }

        // POST: Crear Factura
        [HttpPost]
        public async Task<IActionResult> Create(CreateInvoiceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var clients = await _apiService.GetAsync<List<ClientDto>>("Clients");
                var services = await _apiService.GetAsync<List<ServiceDto>>("Services");
                model.Clients = clients ?? new List<ClientDto>();
                model.Services = services ?? new List<ServiceDto>();
                return View(model);
            }

            // Calcular subtotal, impuesto y total
            decimal subtotal = model.SelectedServices
                .Where(service => model.Services.Any(s => s.Id == service.ServiceId))
                .Sum(service => model.Services.First(s => s.Id == service.ServiceId).Price * service.Quantity);

            decimal taxRate = 0.18m;
            decimal tax = subtotal * taxRate;
            decimal total = subtotal + tax;

            var invoiceDetails = model.SelectedServices.Select(s => new CreateInvoiceDetailDto
            {
                ServiceId = s.ServiceId,
                Quantity = s.Quantity
            }).ToList();

            var invoice = new CreateInvoiceDto
            {
                ClientId = model.SelectedClientId,
                Details = invoiceDetails,
                Subtotal = subtotal,
                Tax = tax,
                Total = total,
                Status = "Pendiente"
            };

            try
            {
                await _apiService.PostAsync("Invoices", invoice);
                TempData["SuccessMessage"] = "Factura creada exitosamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al guardar la factura. " + ex.Message);
                return View(model);
            }
        }

        // GET: Vista Previa de Factura
        public IActionResult Preview()
        {
            if (TempData["InvoicePreview"] is not CreateInvoiceDto invoice)
            {
                TempData["ErrorMessage"] = "La información de la factura no está disponible.";
                return RedirectToAction("Create");
            }

            return View(invoice);
        }

        // POST: Confirmar Factura
        [HttpPost]
        public async Task<IActionResult> Confirm()
        {
            if (TempData["InvoicePreview"] is not CreateInvoiceDto invoice)
            {
                TempData["ErrorMessage"] = "No se pudo recuperar la información de la factura para guardarla.";
                return RedirectToAction("Create");
            }

            try
            {
                await _apiService.PostAsync("Invoices", invoice);
                TempData["SuccessMessage"] = "Factura creada exitosamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ocurrió un error al confirmar la factura: " + ex.Message;
                return RedirectToAction("Preview");
            }
        }

        // GET: Marcar Factura como Pagada
        public async Task<IActionResult> MarkAsPaid(int id)
        {
            try
            {
                Console.WriteLine($"Intentando marcar como pagada la factura con ID: {id}");

                await _apiService.PutAsync<object>($"Invoices/{id}/MarkAsPaid", null);
                TempData["SuccessMessage"] = "Factura marcada como pagada.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Ocurrió un error al marcar la factura como pagada. {ex.Message}";
            }
            return RedirectToAction("Index");
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
                TempData["SuccessMessage"] = "Pago registrado exitosamente.";
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
