using System;
using CargoHub.Enums;

namespace CargoHub.Models
{
    public class APIKey : BaseModel
    {
        //Identifier, can also be a GUID
        public int Id { get; set; }

        //Hashed API key, we don't want the real key unprotected in the database, due to potential hacker attacks
        public string HashedApiKey { get; set; }

        //We don't want typos, we want specific roles, therefor we use an enum.
        public RolesEnum.Roles UserRole { get; set; }

        //An api key should only be used for a certain amount of time before it needs to be regenerated. This is to unactivate unused old API keys.
        public DateTime ExpiryDate { get; set; }

        //This value will be checked to see if an api key is still being used. If not, we can set it inactive.
        public DateTime LastUsed { get; set; }

        //When an api key is inactive, it should not work. It should be treated like a non-existing key.

        public bool IsActive { get; set; }
    }

    public class APIKeyDTO : BaseModel
    {
        public UserRole Role { get; set; }
    }

    public class APIKeyDTO : BaseModel
    {
        public UserRole Role { get; set; }
    }
}