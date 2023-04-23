using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelService.Models
{
    public class Passport : BaseModel
    {
        public Visitor? Visitor { get; set; }
        public string? Data { get; set; }
    }
}
