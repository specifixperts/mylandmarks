using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLandmarks.Api.ViewModels.Location
{
    public class CreateViewModel : BaseCreate
    {
        public string Name { get; set; }
        public string Lattitude { get; set; }
        public string Longitude { get; set; }
        public string Type { get; set; }
    }
}
