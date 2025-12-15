using Microsoft.EntityFrameworkCore;
using TransactionService.Application.Commands;
using TransactionService.Application.Handlers;
using TransactionService.Domain.Entities;
using TransactionService.Domain.Ports;
using TransactionService.Infrastructure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Connection strings
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                     ?? "Host=localhost;Port=5432;Database=pruebanetby;Username=postgres;Password=root";

// URL del ProductService
var productServiceUrl = builder.Configuration.GetValue<string>("ProductService:BaseUrl")
                       ?? "https://localhost:57873";

builder.Services.AddDbContext<TransactionDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<ITransactionRepository, EfTransactionRepository>();

builder.Services.AddHttpClient<IProductStockPort, ProductStockHttpClient>(client =>
{
    client.BaseAddress = new Uri(productServiceUrl);
});

builder.Services.AddScoped<RegisterTransactionHandler>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Transaction Service API",
        Version = "v1",
        Description = "Microservicio de Gestión de Transacciones de inventario."
    });
});

var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();
app.MapControllers();
app.Run();
