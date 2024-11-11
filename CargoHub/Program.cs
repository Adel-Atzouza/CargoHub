using CargoHub;
using Microsoft.EntityFrameworkCore;
using CargoHub.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Datasource=CargoHub.db"));
builder.Services.AddScoped<ClientService>();
builder.Services.AddScoped<InventoryService>();

builder.Services.AddTransient<StorageService>();

var app = builder.Build();
app.MapControllers();

app.Run();
