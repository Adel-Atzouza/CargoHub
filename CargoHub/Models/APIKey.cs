using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CargoHub.Enums;
using Microsoft.VisualBasic;

namespace CargoHub.Models
{
    public class APIKey : BaseModel
    {
        public int Id { get; set; }
        public string ApiKey { get; set; }
        public UserRole Role { get; set; }
        public DateTime ExpiracyDate { get; set; }
        public DateTime KeyLastUsed { get; set; }
        public bool IsKeyDisabled { get; set; }
    }
}