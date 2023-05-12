using HotelService.DataContext.Repositories;
using HotelService.Models;
using HotelService.Views.ReviewUI;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace HotelService.ViewModels.ReviewVM
{
    public class ReviewsVM : ViewModelBase
    {
        private readonly ReviewRepository _reviewRepository;
        private ObservableCollection<Review>? _reviewList;
        private Hotel? _selectedHotel;
        private Review? _selectedReview;

        public ObservableCollection<Review>? Reviews 
        {
            get => _reviewList;
            set
            {
                _reviewList = value;
                RaiseProperty();
            }
        }
        public string? SearchData { get; set; }
        public List<string>? SearchTypes { get; set; } = new List<string>
        {
            "По посетителю",
            "По отзыву"
        };
        public string? SelectedType { get; set; }
        public Review? SelectedReview
        {
            get => _selectedReview;
            set
            {
                _selectedReview = value;
                RaiseProperty();
            }
        }
        public Hotel? SelectedHotel
        {
            get => _selectedHotel;
            set
            {
                _selectedHotel = value;
                RaiseProperty();
            }
        }

        public ReviewsVM(ReviewRepository reviewRepository, Hotel selectedHotel)
        {
            _reviewRepository = reviewRepository;
            _selectedHotel = selectedHotel;
            LoadReviewsAsync();
        }

        public ICommand Search => new DelegateCommand<object>(TrySearch);
        public ICommand Create => new DelegateCommand<object>(CreateReview);
        public ICommand Delete => new DelegateCommand<object>(DeleteReviewAsync);

        private void CreateReview(object obj)
        {
            new CreateReviewWindow(new CreateReviewVM(_reviewRepository, SelectedHotel)).Show();
            return;
        }

        private async void DeleteReviewAsync(object obj)
        {
            try
            {
                var result = await _reviewRepository.DeleteReviewAsync(SelectedReview.Id);
                if (!result)
                {
                    MessageBox.Show("Не удалось удалить отзыв, который не существует.");
                    return;
                }

                Reviews.Remove(SelectedReview);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка во время удалении отеля: {ex.Message}");
            }
        }

        private void TrySearch(object obj)
        {
            if (String.IsNullOrEmpty(SearchData))
            {
                MessageBox.Show("Ошибка! Поле поиска не заполнено.");
                return;
            }

            if (Reviews == null)
            {
                MessageBox.Show("Невозможно выполнить поиск, пока не существует ни одной записи.");
                return;
            }

            switch (SelectedType)
            {
                case "По посетителю":
                    FilterByVisitor();
                    break;

                case "По отзыву":
                    FilterByTextBody();
                    break;

                default:
                    MessageBox.Show("Ошибка! Не выбран параметр поиска.");
                    break;
            }
        }

        private void FilterByTextBody()
        {
            var reviewsList = new ObservableCollection<Review>();
            foreach (var review in Reviews)
            {
                if (review.ReviewBody.ToLower().StartsWith(SearchData.ToLower()) ||
                    review.ReviewBody.ToLower().Contains(SearchData.ToLower()))
                {
                    reviewsList.Add(review);
                }
            }
            Reviews = reviewsList;
            return;
        }

        private void FilterByVisitor()
        {
            var reviewsList = new ObservableCollection<Review>();
            foreach (var review in Reviews)
            {
                if (review.VisitorFullName.ToLower().StartsWith(SearchData.ToLower()) ||
                    review.VisitorFullName.ToLower().Contains(SearchData.ToLower()))
                {
                    reviewsList.Add(review);
                }
            }
            Reviews = reviewsList;
            return;
        }

        private async void LoadReviewsAsync()
        {
            Reviews = new(await _reviewRepository.GetReviewsByHotelIdAsync(SelectedHotel.Id));
        }
    }
}
