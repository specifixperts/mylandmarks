using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLandmarks.Api.ViewModels.Location
{
    public class IndexViewModel : BaseIndex
    {
        public string Name { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Type { get; set; }
    }

    public struct LatLong
    {
        public string Latitude;
        public string Longitude;
    }
}
