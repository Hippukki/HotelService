using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelService.Models
{
    public class ApartmentDto : BaseModel
    {
        public int Number { get; set; }
        public int Stage { get; set; }
        public string? Category { get; set; }
        public string? Level { get; set; }
        public string? ShortDescription { get; set; }
        public int Price { get; set; }
        public Hotel? Hotel { get; set; }
    }
}
