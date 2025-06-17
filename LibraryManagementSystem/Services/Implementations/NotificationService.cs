using LibraryManagementSystem.Repositories.Interfaces;
using LibraryManagementSystem.Services.Interfaces;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
       


        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
           
        }

        //public async Task SendNotifICationAsync(int userId, string message)
        //{
        //    var notification = new StudentNotification
        //    {
        //        UserId = userId,
        //        Message = message,
        //        CreatedAt = DateTime.UtcNow,
        //    };
        //    await _notificationRepository.AddNotificationAsync(notification);
        //}

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
    }
}
