using MyLandmarks.Api.ViewModels.Photo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLandmarks.Api.ViewModels.Flickr
{
    public class IndexViewModel : ApiResponse
    {
        public string Id { get; set; }
        public string Secret { get; set; }
        public string Server { get; set; }
    }
}
