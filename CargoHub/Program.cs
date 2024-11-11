using CargoHub;
using CargoHub.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Register your DbContext with the SQLite connection string
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Datasource=CargoHub.db"));

// Register your custom ItemsService
builder.Services.AddScoped<ItemsService>();  // This line is necessary
builder.Services.AddScoped<ItemTypesService>();
// Add controllers to the services
builder.Services.AddControllers();

var app = builder.Build();

// Enable routing for controllers
app.MapControllers();

// Run the app
app.Run();

