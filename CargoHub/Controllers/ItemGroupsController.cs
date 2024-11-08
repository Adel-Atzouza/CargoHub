using Microsoft.AspNetCore.Mvc;
namespace CargoHub
{
    public class ItemGroupsController : Controller
    {
        ItemGroupsService service;
        public ItemGroupsController(ItemGroupsService service)
        {
            service = service;
        }
    }
}
