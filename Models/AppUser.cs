using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLandmarks.Api.Models
{
    public class AppUser : IdentityUser
    {
        public string ApiKey { get; set; }
    }
}
