using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelService.Models
{
    public class Hotel : BaseModel
    {
        public string? Name { get; set; }
        public string? FullAddress { get; set; }
        public int? StageCount { get; set; }
        public List<Service>? Services { get; set; }
        public List<Review>? Reviews { get; set; }
    }
}
