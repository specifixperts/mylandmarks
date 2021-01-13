using System;
using System.Collections.Generic;

namespace MyLandmarks.Api.Models
{
    public class Photo : Entity
    {
        public string Source { get; set; }
        public string Url { get; set; }
        public DateTime DateTaken { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public string SourceId { get; set; }

        public virtual IEnumerable<PhotoLocation> Locations { get; set; }
    }

    public class PhotoLocation : Entity
    {
        public Guid LocationId { get; set; }
        public Guid PhotoId { get; set; }

        public virtual Photo Photo { get; set; }
        public virtual Location Location { get; set; }
    }
}