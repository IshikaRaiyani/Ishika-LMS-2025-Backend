using LibraryManagementSystem.Repositories.Interfaces;
using LibraryManagementSystem.Services.Interfaces;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IEmailService _emailService;

        public NotificationService(INotificationRepository notificationRepository, IAdminRepository adminRepository, IStudentRepository studentRepository, IEmailService emailService)
        {
            _notificationRepository = notificationRepository;
            _adminRepository = adminRepository;
            _studentRepository = studentRepository;
            _emailService = emailService;


        }


        public async Task<List<StudentNotification>> GetUnreadNotificationsAsync(int Userid)
        {
            var notifications = await _notificationRepository.GetUnreadNotificationsAsync(Userid);

            return notifications.Select(n => new StudentNotification
            {
                UserId = n.UserId,
                NotificationId = n.NotificationId,
                Message = n.Message,
                CreatedAt = n.CreatedAt
            }).ToList();
        }

        public async Task NotificationMarkAsReadAsync(int NotificationId)
        {
            await _notificationRepository.NotificationMarkAsReadAsync(NotificationId);


        }

        public async Task NotifyWishlistUsersIfBookBecomesAvailable(int bookId)
        {
            var book = await _adminRepository.GetBookByIDAsync(bookId);
            if (book == null || book.AvailableCopies <= 0)
                return;

            var wishlists = await _studentRepository.FindWishlistAsync(bookId);

            foreach (var wishlist in wishlists)
            {
                var notification = new StudentNotification
                {
                    UserId = wishlist.UserId,
                    Message = $"📚 Good news! The book '{book.Title}' is now available.You can now borrow it from the library",
                    CreatedAt = DateTime.Now,
                    IsRead = false

                };

                await _notificationRepository.AddNotificationAsync(notification);

                await _emailService.SendEmailAsync(
             wishlist.User.Email,
             "Wishlist Book is Available..",
              $"Your wishlisted book '{book.Title}' is now available.Now you can place the borrow request for that book");
            }

           

        }
    }
}
