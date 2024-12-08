using Microsoft.EntityFrameworkCore;
using SupportBilling.APPLICATION.Contract;
using SupportBilling.APPLICATION.Service;
using SupportBilling.INFRASTRUCTURE.Context;
using SupportBilling.INFRASTRUCTURE.Repositories;
using System.Text.Json.Serialization;

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

// Configurar controladores y habilitar manejo de referencias circulares
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; // Maneja ciclos sin metadatos
        options.JsonSerializerOptions.WriteIndented = true; // JSON formateado
    });

// Configuraci�n de Swagger para la documentaci�n de API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registrar los servicios de aplicaci�n
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();


// Registrar el repositorio gen�rico
builder.Services.AddScoped(typeof(BaseRepository<>));

var app = builder.Build();

// Configuraci�n del pipeline de la aplicaci�n
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Habilitar CORS
app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

// Map Controllers (sin usar UseEndpoints)
app.MapControllers();

app.Run();
