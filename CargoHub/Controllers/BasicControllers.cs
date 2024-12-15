using CargoHub.Models;
using CargoHub.Services;
using CargoHub.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace CargoHub.Controllers
{
    [Route("api/v2/[Controller]")]
    public class ItemTypesController(BaseStorageService storage, ErrorHandler error) : BaseController<ItemType>(storage, error) {}

    [Route("api/v2/[Controller]")]
    public class ItemLinesController(BaseStorageService storage, ErrorHandler error) : BaseController<ItemLine>(storage, error) {}

    [Route("api/v2/[Controller]")]
    public class ItemGroupsController(BaseStorageService storage, ErrorHandler error) : BaseController<ItemGroup>(storage, error) {}

    [Route("api/v2/[Controller]")]
    public class ClientsController(BaseStorageService storage, ErrorHandler error) : BaseController<Client>(storage, error) {}

    [Route("api/v2/[Controller]")]
    public class InventoriesController(BaseStorageService storage, ErrorHandler error) : BaseController<Inventory>(storage, error) {}

    [Route("api/v2/[Controller]")]
    public class SuppliersController(BaseStorageService storage, ErrorHandler error) : BaseController<Supplier>(storage, error) {}

    [Route("api/v2/[Controller]")]
    public class TransfersController(BaseStorageService storage, ErrorHandler error) : BaseController<Transfer>(storage, error) {}

    [Route("api/v2/[Controller]")]
    public class WarehousesController(BaseStorageService storage, ErrorHandler error) : BaseController<Warehouse>(storage, error) {}

}

