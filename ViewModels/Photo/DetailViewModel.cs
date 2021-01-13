using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLandmarks.Api.ViewModels.Photo
{
    public class DetailViewModel : IndexViewModel
    {
        public DateTime DateTaken { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string SourceId { get; set; }
        public Location.IndexViewModel Location { get; set; }
    }
}
