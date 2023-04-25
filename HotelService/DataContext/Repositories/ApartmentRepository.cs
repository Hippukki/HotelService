using HotelService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelService.DataContext.Repositories
{
    public class ApartmentRepository
    {
        private readonly HotelContext _context;

        public ApartmentRepository(HotelContext context)
        {
            _context = context;
        }

        public async Task<List<Apartment>> GetApartmentsAsync()
        {
            return await _context.Apartments.Where(a => !a.IsDeleted).ToListAsync();
        }

        public async Task<List<Apartment>> GetApartmentsByHotelIdAsync(int hotelId)
        {
            return await _context.Apartments.Where(a => a.HotelId == hotelId && !a.IsDeleted).ToListAsync();
        }

        public async Task CreateApartmentAsync(Apartment apartment)
        {
            await _context.Apartments.AddAsync(apartment);
            _context.SaveChanges();
        }

        public async Task<Apartment> GetApartmentByIdAsync(int Id)
        {
            return await _context.Apartments.FirstOrDefaultAsync(a => a.Id == Id);
        }

        public async Task<bool> DeleteApartmentAsync(int Id)
        {
            var apartment = await GetApartmentByIdAsync(Id);
            if(apartment == null)
            {
                return false;
            }

            apartment.UpdateDateTime = DateTime.Now;
            apartment.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
