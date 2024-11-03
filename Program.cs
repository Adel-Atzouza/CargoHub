using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options =>options.UseSqlite("Data Source=MidTerm.db"));

//Add Code below related to Q1 if any
builder.Services.AddControllers();

//Add Code below related to Q2 if any
builder.Services.AddTransient<IOrderService, OrderService>();

var app = builder.Build();

//Add Code below related to Q1 if any
app.MapControllers();

app.Urls.Add("http://localhost:5001");

app.MapGet("/", () => "Welcome to the Midterm exam, Wish you best of luck");

//Q4 a: Add code related to Middleware here  
app.Use(async (context, next) =>
{
    await next.Invoke();

    if (context.Request.Path.StartsWithSegments("/api/orders"))
    {
        string requestPath = $"request path: {context.Request.Path}";
        string responseStatusCode = $"response status code: {context.Response.StatusCode}\n";
        await File.AppendAllTextAsync("log.txt", $"{requestPath}, {responseStatusCode}");
    }

    if (context.Response.StatusCode == 401)
    {
        string requestPath = $"request path: {context.Request.Path}";
        await File.AppendAllTextAsync("important.txt", $"{context.Request.Method} {requestPath}\n");
    }
});

app.Run();

//Given Domain Class
public class Order {
    public Guid Id { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime OrderDate { get; set; }
    public override string ToString()=>
    $"Order [Id:{Id}, TotalAmount:{TotalAmount}, OrderDate:{OrderDate}]";
}

//Context Class
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed some initial data
        modelBuilder.Entity<Order>().HasData(
            new Order { Id = Guid.Parse("95F802C1-ED8B-49DC-BBAD-0319D33586E1"), TotalAmount = 10.45m, OrderDate = DateTime.Now },
            new Order { Id = Guid.Parse("CB44AE42-F5A3-4DBF-A954-9444E8C2AF6A"), TotalAmount = 100m, OrderDate = DateTime.Now },
            new Order { Id = Guid.Parse("DA0F9031-EDDE-45A2-A7C6-742DA19DB0D5"), TotalAmount = 67.89m, OrderDate = DateTime.Now }
        );
    }
}


//Q1: Mock Controller Here
[Route("api/mock")]
public class MockController : Controller
{
    [HttpGet("{id}")]
    public IActionResult GetMock(Guid id)
    {
        if (id == Guid.Empty) return BadRequest();
        
        return Ok(id);
    }

    [HttpPut()]
    public IActionResult PutMock([FromQuery] Guid? id, [FromBody] Order order)
    {
        if (order is null) return BadRequest();
        if (id is null) return Ok("Insert Request");
        return Ok("Update Request");
    }

    [HttpDelete()]
    public IActionResult DeleteMock([FromQuery] Guid[] Ids)
    {
        var ids = Ids.Where(_=>_!=Guid.Empty).ToList();
        if (ids.Count == 0) return BadRequest();
        return Ok(ids);
    }
}

//Q2 Complete the OrderService

public interface IOrderService
{
    Order? Get(Guid id); // Returns Order object if a given order is found against id, otherwise returns null;
    bool Insert(Order order);// Makes sure to avoid duplicate insertion, returns true if Insert operation was successful otherwise returns false.
    bool Update(Order order, Guid id); // Updates Order date and amount for given id passed as parameter separately. 
    int Delete(IEnumerable<Guid> ids ); // Return the number of records deleted against the given list of Ids
}

public class OrderService(AppDbContext appDbContext) : IOrderService
{
    AppDbContext appDbContext = appDbContext;
    public int Delete(IEnumerable<Guid> ids)
    {
        foreach (Guid id in ids)
        {
            Order? x = appDbContext.Orders.FirstOrDefault(_=>_.Id == id);
            if (x is not null) appDbContext.Orders.Remove(x);
        }
        return appDbContext.SaveChanges();
    }

    public Order? Get(Guid id)
    {
        return appDbContext.Orders.FirstOrDefault(_=>_.Id == id);
    }

    public bool Insert(Order order)
    {
        Order? x = appDbContext.Orders.FirstOrDefault(_=>_.Id == order.Id);
        if (x is not null) return false;
        appDbContext.Orders.Add(order);
        int n = appDbContext.SaveChanges();
        return n > 0;
    }

    public bool Update(Order order, Guid id)
    {
        Order? x = appDbContext.Orders.FirstOrDefault(_=>_.Id == id);
        if (x is null) return false;
        x.OrderDate = order.OrderDate;
        x.TotalAmount = order.TotalAmount;
        int n = appDbContext.SaveChanges();
        return n > 0;
    }
}

//Q3: OrdersController with dependency injection
[Route("api/orders")]
public class OrdersController(IOrderService orderService) : Controller
{
    IOrderService orderService = orderService;

    [HttpGet("{id}")]
    public IActionResult GetOrder(Guid id)
    {
        if (id == Guid.Empty) return BadRequest();
        Order? order = orderService.Get(id);
        return order is not null ? Ok(order) : NoContent();
    }

    [HttpPut()]
    [WritePermission()]
    public IActionResult PutOrder([FromQuery] Guid? id, [FromBody] Order order)
    {
        if (order is null) return BadRequest();
        if (id is null)
        {
            bool x = orderService.Insert(order);
            return x ? Ok("Inserted") : NoContent() ;
        }
        bool y = orderService.Update(order, (Guid)id);
        return y ? Ok("Updated") : NoContent();
    }

    [HttpDelete()]
    [WritePermission()]
    public IActionResult DeleteOrder([FromQuery] Guid[] Ids)
    {
        var ids = Ids.Where(_=>_!=Guid.Empty).ToList();
        if (ids.Count == 0) return BadRequest();
        int x = orderService.Delete(ids);
        return Ok($"{x} records deleted");
    }
}


//Q4 b: Filter here
public class WritePermission : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext actionContext, ActionExecutionDelegate next)
    {
        var context = actionContext.HttpContext;
        
        if (!context.Request.Headers.ContainsKey("Permission"))
        {
            context.Response.StatusCode = 401;
            return;
        }
        else if (context.Request.Headers["Permission"] != "True")
        {
            context.Response.StatusCode = 401;
            return;
        }

        await next.Invoke();
    }
} 