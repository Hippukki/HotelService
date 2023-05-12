using HotelService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelService.DataContext.Repositories
{
    public class ReviewRepository
    {
        private readonly HotelContext _context;

        public ReviewRepository(HotelContext context)
        {
            _context = context;
        }

        public async Task<List<Review>> GetListReviewsAsync()
        {
            return await _context.Reviews.Where(r => !r.IsDeleted).ToListAsync();
        }

        public async Task<Review> GetReviewByIdAsync(int Id)
        {
            return await _context.Reviews.FirstOrDefaultAsync(r => r.Id == Id);
        }

        public async Task<List<Review>> GetReviewsByHotelIdAsync(int hotelId)
        {
            return await _context.Reviews.Include(r => r.Hotel).Where(r => r.Hotel.Id == hotelId && !r.IsDeleted).ToListAsync();
        }

        public async Task CreateReviewAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
            _context.SaveChanges();
        }

        public async Task UpdateReviewAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteReviewAsync(int Id)
        {
            var review = await GetReviewByIdAsync(Id);
            if (review == null)
            {
                return false;
            }

            review.IsDeleted = true;
            review.UpdateDateTime = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
