using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelService.Models
{
    public class Apartment : BaseModel
    {
        public int Number { get; set; }
        public int Stage { get; set; }
        public ApartmentCategory Category { get; set; }
        public ApartmentLevel Level { get; set; }
        public string? ShortDescription { get; set; }
        public decimal Price { get; set; }
        public Hotel? Hotel { get; set; }
        public int? HotelId { get; set; }
    }
}
