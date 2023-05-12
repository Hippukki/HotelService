using HotelService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelService.DataContext.Repositories
{
    public class ResidenceOrderRepository
    {
        private readonly HotelContext _context;

        public ResidenceOrderRepository(HotelContext context)
        {
            _context = context;
        }

        public async Task<List<ResidenceOrder>> GetOrdersByHotelIdAsync(int Id)
        {
            return await _context.ResidenceOrders
                .Include(o => o.Visitor)
                .Include(o => o.Hotel)
                .Include(o => o.Apartments)
                .Include(o => o.Services)
                .Where(o => o.Hotel.Id == Id && !o.IsDeleted)
                .ToListAsync();
        }

        public async Task CreateOrderAsync(ResidenceOrder order)
        {
            await _context.ResidenceOrders.AddAsync(order);
            _context.SaveChanges();
        }

        public async Task UpdateOrderAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<ResidenceOrder> GetOrderByIdAsync(Guid Id)
        {
            return await _context.ResidenceOrders.FirstOrDefaultAsync(o => o.GuidId == Id);
        }

        public async Task<bool> DeleteOrderAsync(Guid Id)
        {
            var order = await GetOrderByIdAsync(Id);
            if(order == null)
            {
                return false;
            }

            order.UpdateDateTime = DateTime.Now;
            order.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
