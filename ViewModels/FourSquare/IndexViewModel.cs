using MyLandmarks.Api.ViewModels.Photo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLandmarks.Api.ViewModels.FourSquare
{
    public class IndexViewModel : ApiResponse
    {
        public string Id { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int CreatedAt { get; set; }

    }
}
