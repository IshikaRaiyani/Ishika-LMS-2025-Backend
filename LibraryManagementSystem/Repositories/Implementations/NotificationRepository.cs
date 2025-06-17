using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Repositories.Implementations
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly DataContext _context;

        public NotificationRepository(DataContext context)
        {
            _context = context;
        }

        public async Task AddNotificationAsync(StudentNotification notification)
        {

            await _context.studentNotifications.AddAsync(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<List<StudentNotification>> GetUnreadNotificationsAsync(int Userid)
        {
            return await _context.studentNotifications
           .Where(n => n.UserId == Userid && !n.IsRead)
           .OrderByDescending(n => n.CreatedAt)
           .ToListAsync();
        }

        public async Task NotificationMarkAsReadAsync(int NotificationId)
        {
            var notification = await _context.studentNotifications.FindAsync(NotificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}
