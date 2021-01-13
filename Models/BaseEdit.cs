using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLandmarks.Api.ViewModels
{
    public class BaseEdit
    {
        public Guid Id { get; set; }
        public DateTime? DateLastEdited { get; set; } = DateTime.Now;
        public bool Deleted { get; set; }
    }
}
