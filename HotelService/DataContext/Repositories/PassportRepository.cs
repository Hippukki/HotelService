using HotelService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelService.DataContext.Repositories
{
    public class PassportRepository
    {
        private readonly HotelContext _context;

        public PassportRepository(HotelContext context)
        {
            _context = context;
        }

        public async Task CreatePassportAsync(Passport passport)
        {
            await _context.Passports.AddAsync(passport);
            _context.SaveChanges();
        }

        public async Task<Passport> GetPassportByIdAsync(int Id)
        {
            return await _context.Passports.FirstOrDefaultAsync(p => p.Id == Id);
        }

        public async Task UpdatePassportAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<Passport>> GetPassports()
        {
            return await _context.Passports.Where(p => !p.IsDeleted).ToListAsync();
        }

        public async Task<List<Passport>> GetPassports(int Id)
        {
            return await _context.Passports.Where(p => !p.IsDeleted && p.Visitor.Id == Id).ToListAsync();
        }

        public async Task<bool> DeletePassportAsync(int Id)
        {
            var passport = await GetPassportByIdAsync(Id);
            if(passport == null)
            {
                return false;
            }

            passport.IsDeleted = true;
            passport.UpdateDateTime = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
