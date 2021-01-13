using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLandmarks.Api.ViewModels.FourSquare
{
    public class VenueViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        [JsonProperty("location.lat")]
        public string Latitude { get; set; }
        [JsonProperty("location.lng")]
        public string Longitude { get; set; }
    }
}
