using HotelService.DataContext.Repositories;
using HotelService.Models;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HotelService.ViewModels.ReviewVM
{
    public class CreateReviewVM
    {
        private readonly ReviewRepository _reviewRepository;
        private Hotel _selectedHotel;

        public string? VisitorFullName { get; set; }
        public string? ReviewBody { get; set; }

        public CreateReviewVM(ReviewRepository reviewRepository, Hotel selectedHotel)
        {
            _reviewRepository = reviewRepository;
            _selectedHotel = selectedHotel;
        }

        public ICommand Save => new DelegateCommand<object>(CreateReviewAsync);

        private async void CreateReviewAsync(object obj)
        {
            if (VisitorFullName == null || ReviewBody == null)
            {
                MessageBox.Show("Ошибка! Все поля должны быть заполнены.");
                return;
            }

            try
            {
                var review = new Review
                {
                    VisitorFullName = VisitorFullName,
                    ReviewBody = ReviewBody,
                    HotelName = _selectedHotel.Name,
                    Hotel = _selectedHotel,
                    CreateDateTime = DateTime.Now
                };

                await _reviewRepository.CreateReviewAsync(review);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка во время сохранения отзыва: {ex.Message}");
            }
        }
    }
}
