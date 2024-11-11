using CargoHub;
using CargoHub.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Datasource=CargoHub.db"));

builder.Services.AddTransient<StorageService>();
builder.Services.AddTransient<OrderService>();
builder.Services.AddTransient<LocationService>();


var app = builder.Build();
app.MapControllers();

app.Run();
