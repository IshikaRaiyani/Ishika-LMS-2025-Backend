using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Repositories.Interfaces
{
    public interface INotificationRepository
    {
        Task AddNotificationAsync(StudentNotification notification);

        Task<List<StudentNotification>> GetUnreadNotificationsAsync(int Userid);

        Task NotificationMarkAsReadAsync(int NotificationId);
    }
}
