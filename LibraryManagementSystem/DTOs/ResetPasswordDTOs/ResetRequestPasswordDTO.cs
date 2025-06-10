using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.DTOs.ResetPasswordDTOs
{
    public class ResetRequestPasswordDTO
    {
        [Required]
        public string Email { get; set; }

    }
}
