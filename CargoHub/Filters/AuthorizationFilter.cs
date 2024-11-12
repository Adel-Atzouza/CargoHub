using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CargoHub
{
    public class AuthorizationFilter : Attribute, IAsyncActionFilter
    {
        private readonly List<string> _roles = ["Admin", "FM", "WHM", "SP"];
        public async Task OnActionExecutionAsync(ActionExecutingContext _context, ActionExecutionDelegate next)
        {
            var context = _context.HttpContext;
            string? ApiKey = context.Request.Headers["ApiKey"].FirstOrDefault();
            string Url = context.Request.Path.ToString();
            if (ApiKey == null)
            {
                context.Response.StatusCode = 401;
                return;
            }
            var parts = ApiKey.Split("-");
            //First check if the api_key contains one of the roles
            if (!_roles.Contains(parts[0]))
            {
                context.Response.StatusCode = 401;
                return;
            }
            // Check if the api key is valid
            string role = parts[0];
            if (role == "FM")
            {
                // has access to orders en shipments
                if (Url.Contains("orders") || Url.Contains("shipments"))
                {
                    // check the code of the warehouse and give access only to that warehouse: sprint 2
                    // string WareHouseCode = parts[1];
                    await next();
                }
                context.Response.StatusCode = 401;
                return;

            }
            if (role == "WHM")
            {
                // has access to Items in the warehouses that he's managing.
                if (Url.Contains("Items") || Url.Contains("Warehouses") ||
                    Url.Contains("ItemGroups") || Url.Contains("ItemLines")||
                    Url.Contains("ItemTypes"))
                {
                    // check the codes of the warehouses and give access only to those warehouses: sprint 2
                    // string[] WareHouseCodes = parts[1].Split(",");
                    await next();
                }
                context.Response.StatusCode = 401;

                return;

            }
            if (role == "SP")
            {
                if (Url.Contains("Orders"))
                {
                    await next();
                }
                context.Response.StatusCode = 401;
                return;
            }
            await next();
        }
    }
}