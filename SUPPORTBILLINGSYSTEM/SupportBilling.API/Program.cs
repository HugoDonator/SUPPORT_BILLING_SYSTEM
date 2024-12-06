using Microsoft.EntityFrameworkCore;
using SupportBilling.APPLICATION.Contract;
using SupportBilling.APPLICATION.Services;
using SupportBilling.DOMAIN.Core;
using SupportBilling.INFRASTRUCTURE.Context;
using SupportBilling.INFRASTRUCTURE.Repositories;


var builder = WebApplication.CreateBuilder(args);

// Configuración de la cadena de conexión y DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar servicios de la capa APPLICATION
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IServiceService, ServiceService>();

// Registrar repositorios de la capa INFRASTRUCTURE
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();

// Configurar controladores y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurar middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
