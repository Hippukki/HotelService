﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelService.Models
{
    public class Service : BaseModel
    {
        public string? ServiceName { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public List<Hotel>? Hotels { get; set; }
        public List<ResidenceOrder>? ResidenceOrders { get; set; }
    }
}
