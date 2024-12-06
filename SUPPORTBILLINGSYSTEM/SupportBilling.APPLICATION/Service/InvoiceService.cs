﻿using Fluent.Infrastructure.FluentModel;
using SupportBilling.APPLICATION.Contract;
using SupportBilling.APPLICATION.Dtos;
using SupportBilling.DOMAIN.Entities;
using SupportBilling.INFRASTRUCTURE.Interfaces;

namespace SupportBilling.APPLICATION.Service
{
    public class InvoiceService : IInvoiceService
    {
        private readonly ApplicationDbContext _dbContext;

        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IPaymentRepository _paymentRepository;
        public InvoiceService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public InvoiceService(
            IInvoiceRepository invoiceRepository,
            IClientRepository clientRepository,
            IServiceRepository serviceRepository,
            IPaymentRepository paymentRepository)
        {
            _invoiceRepository = invoiceRepository;
            _clientRepository = clientRepository;
            _serviceRepository = serviceRepository;
            _paymentRepository = paymentRepository;
        }

        // Crear una nueva factura
        public async Task CreateInvoiceAsync(CreateInvoiceDto invoiceDto)
        {
            // Validar que el cliente existe
            var client = await _clientRepository.GetByIdAsync(invoiceDto.ClientId);
            if (client == null)
                throw new Exception("Cliente no encontrado");

            // Calcular el total
            decimal totalAmount = 0;
            var details = new List<InvoiceDetail>();
            foreach (var detail in invoiceDto.Details)
            {
                var service = await _serviceRepository.GetByIdAsync(detail.ServiceId);
                if (service == null)
                    throw new Exception($"Servicio con ID {detail.ServiceId} no encontrado");

                var lineTotal = service.Price * detail.Quantity;
                totalAmount += lineTotal;

                details.Add(new InvoiceDetail
                {
                    ServiceId = detail.ServiceId,
                    Quantity = detail.Quantity,
                    UnitPrice = service.Price // Usamos "UnitPrice" en lugar de "Price"
                });
            }

            // Calcular impuestos
            decimal taxRate = 0.18m; // Tasa fija del 18%
            decimal totalWithTax = totalAmount + (totalAmount * taxRate);

            // Crear la factura
            var invoice = new Invoice
            {
                ClientId = invoiceDto.ClientId,
                InvoiceDate = DateTime.Now,
                TotalAmount = totalWithTax,
                Status = "Pendiente" // Estado inicial de la factura
            };

            await _invoiceRepository.AddAsync(invoice);

            // Agregar detalles a la factura
            foreach (var detail in details)
            {
                detail.InvoiceId = invoice.Id;
                await _invoiceRepository.AddDetailAsync(detail);
            }
        }

        // Registrar un pago para una factura
        public async Task RegisterPaymentAsync(CreatePaymentDto paymentDto)
        {
            // Validar que la factura existe
            var invoice = await _invoiceRepository.GetByIdAsync(paymentDto.InvoiceId);
            if (invoice == null)
                throw new Exception("Factura no encontrada");

            // Registrar el pago
            var payment = new Payment
            {
                InvoiceId = paymentDto.InvoiceId,
                AmountPaid = paymentDto.AmountPaid,
                PaymentDate = DateTime.Now
            };
            await _paymentRepository.AddAsync(payment);

            // Calcular el total pagado y actualizar el estado de la factura
            var totalPaid = await _paymentRepository.GetTotalPaidByInvoiceIdAsync(paymentDto.InvoiceId);
            if (totalPaid >= invoice.TotalAmount)
                invoice.Status = "Pagada";
            else
                invoice.Status = "Pendiente";

            await _invoiceRepository.UpdateAsync(invoice);
        }

        // Obtener todas las facturas
        public async Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync()
        {
            var invoices = await _invoiceRepository.GetAllAsync();
            return invoices.Select(i => new InvoiceDto
            {
                Id = i.Id,
                ClientId = i.ClientId,
                ClientName = i.Client?.Name,
                InvoiceDate = i.InvoiceDate,
                TotalAmount = i.TotalAmount,
                Status = i.Status // Incluye el estado
            });
        }

        // Obtener una factura por ID
        public async Task<InvoiceDto> GetInvoiceByIdAsync(int id)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(id);
            if (invoice == null) return null;

            return new InvoiceDto
            {
                Id = invoice.Id,
                ClientId = invoice.ClientId,
                ClientName = invoice.Client?.Name,
                InvoiceDate = invoice.InvoiceDate,
                TotalAmount = invoice.TotalAmount,
                Status = invoice.Status,
                Details = invoice.Details?.Select(d => new InvoiceDetailDto
                {
                    Id = d.Id,
                    ServiceName = d.Service?.Name,
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice,
                }).ToList()
            };
        }

        public async Task UpdateInvoiceAsync(InvoiceDto invoiceDto)
        {
            // Busca la factura en la base de datos
            var invoice = await _dbContext.Invoices.FindAsync(invoiceDto.Id);

            if (invoice == null)
            {
                throw new Exception($"Factura con ID {invoiceDto.Id} no encontrada.");
            }

            // Actualiza los campos necesarios
            invoice.Status = invoiceDto.Status;
            invoice.Total = invoiceDto.Total;

            // Guarda los cambios
            await _dbContext.SaveChangesAsync();
        }

    }
}
