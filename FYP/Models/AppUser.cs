using System;
using System.Collections.Generic;

namespace FYP.Models
{
    public partial class AppUser
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public byte[] Password { get; set; }
        public string Role { get; set; }
        public string Class { get; set; }
        public string Email { get; set; }
    }
}
