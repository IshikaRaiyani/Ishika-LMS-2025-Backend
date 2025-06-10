using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string RoleName { get; set; }

        [Required(ErrorMessage="Full Name is required.")]
        [StringLength(100, ErrorMessage = "Full Name cannot exceed 100 characters")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(5, ErrorMessage = "Password must be at least 5 characters.")]
        [MaxLength(256, ErrorMessage = "Password cannot exceed 256 characters.")]
        public string Password { get; set; }

        public string Status { get; set; }   

        public int NoofBooks { get; set; }

        
        

    }
}
