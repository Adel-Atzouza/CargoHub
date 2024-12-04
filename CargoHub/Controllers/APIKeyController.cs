using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CargoHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace CargoHub.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class APIKeyController : ControllerBase
    {
        private readonly ApiKeyGeneratorService _apiKeyGenerator;

        public APIKeyController(ApiKeyGeneratorService apiKeyGenerator)
        {
            _apiKeyGenerator = apiKeyGenerator;
        }

        [HttpPost("generate-key")]
        public IActionResult GenerateApiKey()
        {
            var apiKey = _apiKeyGenerator.GenerateApiKey();
            return Ok(new { ApiKey = apiKey });
        }
    }
}