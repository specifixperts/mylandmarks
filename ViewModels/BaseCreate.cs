using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLandmarks.Api.ViewModels
{
    public abstract class BaseCreate
    {
        [JsonIgnore]
        public Guid Id { get; set; } = Guid.NewGuid();
        [JsonIgnore]
        public DateTime DateCreated { get; set; } = DateTime.Now;
        [JsonIgnore]
        public bool Deleted { get; set; } = false;
    }
}
