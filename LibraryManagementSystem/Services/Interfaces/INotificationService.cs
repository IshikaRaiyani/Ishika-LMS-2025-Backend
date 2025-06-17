using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Services.Interfaces
{
    public interface INotificationService
    {
        //public Task SendNotifICationAsync(int userId, string message);

        Task<List<StudentNotification>> GetUnreadNotificationsAsync(int Userid);

        Task NotificationMarkAsReadAsync(int NotificationId);



    }
}
