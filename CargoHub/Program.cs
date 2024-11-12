using CargoHub;
using Microsoft.EntityFrameworkCore;
using CargoHub.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Datasource=CargoHub.db"));

builder.Services.AddScoped<ClientService>();
builder.Services.AddScoped<InventoryService>();
builder.Services.AddScoped<ItemGroupsService>();
builder.Services.AddScoped<ItemLinesService>();
builder.Services.AddTransient<WarehouseService>();
builder.Services.AddTransient<TransferService>();
builder.Services.AddTransient<SupplierService>();

var app = builder.Build();

app.Use(async (context, next) =>
{
    if (!context.Request.Headers.ContainsKey("ApiKey"))
    {
        context.Response.StatusCode = 401;
        return;
    }
    await next.Invoke();
});
app.MapControllers();
app.Urls.Add("http://localhost:3000");

app.Run();
