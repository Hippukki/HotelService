using HotelService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelService.DataContext.Repositories
{
    public class VisitorRepository
    {
        private readonly HotelContext _context;

        public VisitorRepository(HotelContext context)
        {
            _context = context;
        }

        public async Task CreateVisitorAsync(Visitor visitor)
        {
            await _context.Visitors.AddAsync(visitor);
            _context.SaveChanges();
        }

        public async Task UpdateVisitorAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Visitor> GetVisitorByIdAsync(int Id)
        {
            return await _context.Visitors.FirstOrDefaultAsync(v => v.Id == Id);
        }

        public async Task<List<Visitor>> GetVisitorsAsync()
        {
            return await _context.Visitors.Where(v => !v.IsDeleted).ToListAsync();
        }

        public async Task<bool> DeteleVisitorAsync(int Id)
        {
            var visitor = await GetVisitorByIdAsync(Id);
            if(visitor == null)
            {
                return false;
            }

            visitor.IsDeleted = true;
            visitor.UpdateDateTime = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
