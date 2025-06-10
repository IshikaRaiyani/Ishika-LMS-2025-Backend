using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.DTOs.ResetPasswordDTOs
{
    public class ResetPasswordDTO
    {
        [Required(ErrorMessage = "NewPassword is required.")]
        [MinLength(5, ErrorMessage = "NewPassword must be at least 6 characters.")]
        [MaxLength(20, ErrorMessage = "NewPassword cannot exceed 20 characters.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "ConfirmPassword is required.")]
        [MinLength(5, ErrorMessage = "ConfirmPassword must be at least 6 characters.")]
        [MaxLength(20, ErrorMessage = "ConfirmPassword cannot exceed 20 characters.")]
        public string ConfirmPassword { get; set; }

        public string Token { get; set; }
    }
}
