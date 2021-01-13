using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace MyLandmarks.Api.ViewModels.Location
{
    public class CountryViewModel : BaseCreate
    {
        
        public string Code { get; set; }
        public string Name { get; set; }
        public List<string> Latlng { get; set; }
        [JsonIgnore]
        public string Type { get; set; } = "Country";
    }
}
