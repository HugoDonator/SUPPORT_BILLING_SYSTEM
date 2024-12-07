// File: SupportBilling.API/Program.cs
using Microsoft.EntityFrameworkCore;
using SupportBilling.APPLICATION.Contract;
using SupportBilling.APPLICATION.Service;
using SupportBilling.INFRASTRUCTURE.Context;
using SupportBilling.INFRASTRUCTURE.Repositories;
using SupportBilling.DOMAIN.Entities;

var builder = WebApplication.CreateBuilder(args);

// Habilitar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configuraci�n de la cadena de conexi�n
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BillingDbContext>(options =>
    options.UseSqlServer(connectionString));

// Registrar los servicios de aplicaci�n (IClientService, IServiceService, IInvoiceService)
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddHttpClient();
// Registrar los repositorios gen�ricos
builder.Services.AddScoped(typeof(BaseRepository<>));

// Configurar controladores (eliminar llamada duplicada)
builder.Services.AddControllers();

// Configuraci�n de Swagger para la documentaci�n de API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuraci�n del pipeline de la aplicaci�n
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Habilitar CORS antes de los dem�s middleware
app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
