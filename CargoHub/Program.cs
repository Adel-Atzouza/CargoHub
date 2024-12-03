using CargoHub;
using CargoHub.Services;
using Microsoft.EntityFrameworkCore;

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
builder.Services.AddTransient<OrderService>();
builder.Services.AddTransient<LocationService>();
builder.Services.AddTransient<ShipmentService>();

// Register your custom ItemsService
builder.Services.AddScoped<ItemsService>();  // This line is necessary
builder.Services.AddScoped<ItemTypesService>();
// Add controllers to the services
builder.Services.AddControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// app.Use(async (context, next) =>
// {
//     if (!context.Request.Headers.ContainsKey("ApiKey"))
//     {
//         context.Response.StatusCode = 401;
//         return;
//     }
//     await next.Invoke();
// });

app.MapControllers();
app.Urls.Add("http://localhost:3000");

// Run the app
app.Run();


