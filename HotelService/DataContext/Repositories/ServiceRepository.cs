using HotelService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelService.DataContext.Repositories
{
    public class ServiceRepository
    {
        private readonly HotelContext _context;

        public ServiceRepository(HotelContext context)
        {
            _context = context;
        }

        public async Task CreateServiceAsync(Service service)
        {
            await _context.Services.AddAsync(service);
            _context.SaveChanges();
        }

        public async Task<List<Service>> GetServisesAsync()
        {
            return await _context.Services.Where(s => !s.IsDeleted).ToListAsync();
        }

        public async Task<Service> GetServiceByIdAsync(int Id)
        {
            return await _context.Services.FirstOrDefaultAsync(s => s.Id == Id);
        }

        public async Task<bool> DeleteServiceAsync(int Id)
        {
            var service = await GetServiceByIdAsync(Id);
            if(service == null)
            {
                return false;
            }

            service.IsDeleted = true;
            service.UpdateDateTime = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
