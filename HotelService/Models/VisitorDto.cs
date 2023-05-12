using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelService.Models
{
    public class VisitorDto : BaseModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PatromicName { get; set; }
        public string? PhoneNumber { get; set; }
        public Passport? PassData { get; set; }
        public DateOnly BirthDate { get; set; }
    }
}
