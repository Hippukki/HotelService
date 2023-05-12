using HotelService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelService.DataContext.Repositories
{
    public class HotelRepository
    {
        private readonly HotelContext _context;

        public HotelRepository(HotelContext context)
        {
            _context = context;
        }

        public async Task<List<Hotel>> GetListHotelsAsync()
        {
            return await _context.Hotels.Where(h => !h.IsDeleted).ToListAsync();
        }

        public async Task<Hotel> GetHotelByIdAsync(int Id)
        {
            return await _context.Hotels.FirstOrDefaultAsync(h => h.Id == Id);
        }

        public async Task CreateHotelAsync(Hotel hotel)
        {
            await _context.Hotels.AddAsync(hotel);
            _context.SaveChanges();
        }

        public async Task UpdateHotelAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteHotelAsync(int Id)
        {
            var hotel = await GetHotelByIdAsync(Id);
            if(hotel == null)
            {
                return false;
            }

            hotel.IsDeleted = true;
            hotel.UpdateDateTime = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
