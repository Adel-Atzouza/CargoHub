using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CargoHub.Services
{
    public class ApiKeyGeneratorService
    {
        public string GenerateApiKey()
        {
            //Generates a unique API key.
            return Guid.NewGuid().ToString();
        }
    }
}