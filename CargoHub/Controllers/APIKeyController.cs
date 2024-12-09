using System;
using System.Threading.Tasks;
using CargoHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using CargoHub.Services;
using CargoHub.Enums;

namespace CargoHub.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class APIKeyController : ControllerBase
    {
        private readonly ApiKeyGeneratorService _apiKeyGenerator;
        private readonly AppDbContext _dbContext;

        public APIKeyController(ApiKeyGeneratorService apiKeyGenerator, AppDbContext dbContext)
        {
            _apiKeyGenerator = apiKeyGenerator;
            _dbContext = dbContext;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetApiKey()
        {
            IQueryable<APIKey> query = _dbContext.APIKeys;


            var apiKeys = await query
                .Where(k => !k.IsKeyDisabled)
                .Take(100)
                .ToListAsync();

            return Ok(apiKeys);

        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetApiKey(int id)
        {
            IQueryable<APIKey> query = _dbContext.APIKeys;

            var apiKeyRecord = await query
                .Where(k => k.Id == id && !k.IsKeyDisabled)
                .FirstOrDefaultAsync();

            if (apiKeyRecord == null)
            {
                return NotFound($"API key with ID {id} not found or has been disabled.");
            }

            return Ok(apiKeyRecord);
        }



        /// <summary>
        /// Creates an API key and puts it in the database.
        /// The unhashed key is returned for testing (remove this is the program ever goes live!)
        /// </summary>
        [HttpPost("generate-key")]
        public async Task<IActionResult> GenerateApiKey([FromBody] APIKeyDTO request)
        {
            if (request == null || !Enum.IsDefined(typeof(UserRole), request.Role))
            {
                return BadRequest("Invalid or missing role specified.");
            }

            var rawApiKey = _apiKeyGenerator.GenerateApiKey();

            var apiKeyModel = new APIKey
            {
                ApiKey = HashKey(rawApiKey),
                Role = request.Role,
                CreationDate = DateTime.UtcNow,
                ExpiracyDate = DateTime.UtcNow.AddYears(1),
                KeyLastUsed = DateTime.MinValue,
                IsKeyDisabled = false
            };

            _dbContext.APIKeys.Add(apiKeyModel);
            await _dbContext.SaveChangesAsync();

            return Ok(new { ApiKey = rawApiKey });
        }

        /// <summary>
        /// Method used to has the API key (although it can hash anything, like a password, too!)
        /// </summary>
        private string HashKey(string key)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(key));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}
