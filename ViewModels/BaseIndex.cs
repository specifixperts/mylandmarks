using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLandmarks.Api.ViewModels
{
    public class BaseIndex
    {
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateLastEdited { get; set; }
        public bool Deleted { get; set; }
    }
}
