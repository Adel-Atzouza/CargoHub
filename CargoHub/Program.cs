using CargoHub;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Datasource=CargoHub.db"));
builder.Services.AddScoped<ItemGroupsService>();


var app = builder.Build();
app.MapControllers();

app.Run();
