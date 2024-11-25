using CargoHub;
using CargoHub.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Register your DbContext with the SQLite connection string
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Datasource=CargoHub.db"));
builder.Services.AddScoped<ItemGroupsService>();
builder.Services.AddScoped<ItemLinesService>();


builder.Services.AddTransient<WarehouseService>();
builder.Services.AddTransient<TransferService>();
builder.Services.AddTransient<SupplierService>();

// Register your custom ItemsService
builder.Services.AddScoped<ItemsService>();  // This line is necessary
builder.Services.AddScoped<ItemTypesService>();
// Add controllers to the services
builder.Services.AddControllers();

var app = builder.Build();

// Enable routing for controllers
app.MapControllers();
app.Urls.Add("http://localhost:3000");

// Run the app
app.Run();

