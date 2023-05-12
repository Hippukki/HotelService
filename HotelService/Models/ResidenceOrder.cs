using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelService.Models
{
    public class ResidenceOrder : BaseModel
    {
        public Guid GuidId { get; set; }
        public Visitor? Visitor { get; set; }
        public Hotel? Hotel { get; set; }
        public List<Apartment>? Apartments { get; set; }
        public List<Service>? Services { get; set; }
        public decimal TotalSumm { get; set; }
        public DateOnly? SettleDate { get; set; }
        public DateOnly? Evection { get; set; }
    }
}
