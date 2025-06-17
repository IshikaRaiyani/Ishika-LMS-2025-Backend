using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.Models
{
    public class StudentNotification
    {
        [Key]
        public int NotificationId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public User User { get; set; }

        public string Message { get; set; }

        public DateOnly CreatedAt { get; set; }

        public bool IsRead { get; set; }

          
            


    }
}
