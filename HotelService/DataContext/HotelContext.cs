using HotelService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelService.DataContext
{
    public class HotelContext : DbContext
    {
        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Passport> Passports { get; set; }
        public DbSet<ResidenceOrder> ResidenceOrders { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Visitor> Visitors { get; set; }

        public HotelContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
