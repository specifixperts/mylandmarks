using System.Collections.Generic;

namespace MyLandmarks.Api.Models
{
    public class Location : Entity
    {
        public string Name { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Type { get; set; }

        public virtual IEnumerable<PhotoLocation> Photos { get; set; }
    }
}