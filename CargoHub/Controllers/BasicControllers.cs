using CargoHub.Models;
using CargoHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace CargoHub.Controllers
{
    [Route("api/v2/[Controller]")]
    public class ItemTypesController(BaseStorageService storage) : BaseController<ItemType>(storage) {}

    [Route("api/v2/[Controller]")]
    public class ItemLinesController(BaseStorageService storage) : BaseController<ItemLine>(storage) {}

    [Route("api/v2/[Controller]")]
    public class ItemGroupsController(BaseStorageService storage) : BaseController<ItemGroup>(storage) {}

    [Route("api/v2/[Controller]")]
    public class ClientsController(BaseStorageService storage) : BaseController<Client>(storage) {}

    [Route("api/v2/[Controller]")]
    public class InventoriesController(BaseStorageService storage) : BaseController<Inventory>(storage) {}

    [Route("api/v2/[Controller]")]
    public class SuppliersController(BaseStorageService storage) : BaseController<Supplier>(storage) {}

    [Route("api/v2/[Controller]")]
    public class TransfersController(BaseStorageService storage) : BaseController<Transfer>(storage) {}

    [Route("api/v2/[Controller]")]
    public class WarehousesController(BaseStorageService storage) : BaseController<Warehouse>(storage) {}

}

