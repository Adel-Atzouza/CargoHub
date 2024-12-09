using System;
using System.Security.Cryptography;
using System.Text;
using CargoHub.Enums;
using CargoHub.Models;

namespace CargoHub.Services
{
    public class APIKeyService
    {
        private readonly AppDbContext _context;

        public APIKeyService(AppDbContext context)
        {
            _context = context;
        }

        // Method to generate the API key
        public string GenerateApiKey()
        {
            // Generate a new GUID as the API key
            Guid apiKey = Guid.NewGuid();
            return apiKey.ToString(); // Return it as a string
        }

        // Method to hash the API key using SHA-256
        public string HashApiKey(string apiKey)
        {
            using SHA256 sha256 = SHA256.Create();
            // Convert the key to bytes and hash it
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(apiKey));
            return Convert.ToBase64String(hashBytes); // Return base64 encoded hash
        }

        // Method to create and save the API key
        public APIKey CreateApiKey(RolesEnum.Roles role, DateTime expiryDate)
        {
            string rawApiKey = GenerateApiKey();
            string hashedApiKey = HashApiKey(rawApiKey);

            // Create the APIKey model
            var apiKey = new APIKey
            {
                //ApiKey = rawApiKey, // You can store this as a raw key in the response if needed, but never in DB
                HashedApiKey = hashedApiKey,
                UserRole = role,
                ExpiryDate = expiryDate,
                LastUsed = DateTime.UtcNow,
                IsActive = true
            };

            // Save to the database
            _context.APIKeys.Add(apiKey);
            _context.SaveChanges();

            return apiKey;
        }

        // Method to verify if the provided API key matches
        public bool VerifyApiKey(string rawApiKey)
        {
            // Hash the provided API key and compare it with the stored hashed key
            string hashedApiKey = HashApiKey(rawApiKey);
            var apiKey = _context.APIKeys
                                 .FirstOrDefault(x => x.HashedApiKey == hashedApiKey && x.IsActive);

            return apiKey != null; // Return true if a match is found
        }
    }
}
