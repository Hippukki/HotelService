using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelService.Models
{
    public class Review : BaseModel
    {
        public string? VisitorFullName { get; set; }
        public string? HotelName { get; set; }
        public string? ReviewBody { get; set; }
        public Hotel? Hotel { get; set; }

    }
}
