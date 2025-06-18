using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Services.Interfaces
{
    public interface INotificationService
    {
        Task<List<StudentNotification>> GetUnreadNotificationsAsync(int Userid);

        Task NotificationMarkAsReadAsync(int NotificationId);

        Task NotifyWishlistUsersIfBookBecomesAvailable(int bookId);



    }
}
