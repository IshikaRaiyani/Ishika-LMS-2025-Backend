using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.DTOs.AdminDTOs
{
    public class AddAdminDTO
    {
        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(100, ErrorMessage = "Full Name cannot exceed 100 characters")]
        public string FullName { get; set; }

        //public string RoleName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(5, ErrorMessage = "Password must be at least 5 characters.")]
        [MaxLength(256, ErrorMessage = "Password cannot exceed 256 characters.")]
        public string Password { get; set; }
    }
}
