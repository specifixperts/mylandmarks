using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLandmarks.Api.ViewModels.Photo
{
    public class PhotoLocationViewModel
    {
        public Guid LocationId { get; set; }
        public Guid PhotoId { get; set; }
        public string Source { get; set; }
        public string Url { get; set; }
        public string LocationName { get; set; }
    }
}
