using LibraryManagementSystem.Services.Implementations;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [ApiController]
    [Route("NotificationController")]
    [Authorize(Roles = "Student")]
    public class NotificationController : ControllerBase 
    {
        private readonly INotificationService _notifcationServices;

        public NotificationController(INotificationService notifcationServices)
        {
            _notifcationServices = notifcationServices;
        }

        [HttpGet("GetUnreadNotifications")]
        public async Task<IActionResult> GetUnreadNotificationsAsync(int Userid)
        {
            try
            {
                var response = await _notifcationServices.GetUnreadNotificationsAsync(Userid);

                if (response == null)
                {
                    return NotFound("No unread notifications found."); 
                }
                return Ok(response); 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "An error occurred while retrieving notifications."); 
            }
        }

        [HttpPut("NotificationMarkAsRead")]
        public async Task<IActionResult> NotificationMarkAsReadAsync(int NotificationId)
        {
            try
            {
                await _notifcationServices.NotificationMarkAsReadAsync(NotificationId);
                return Ok("Notification marked as read");

            }
            catch(Exception ex)
            {
                return NotFound("Something went wrong!");
               
            }
        }


    }
}
