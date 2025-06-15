using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace LibraryManagementSystem.DTOs.AccessTokenDTOs
{
    public class AccessTokenResponseDTO
    {
        [Required(ErrorMessage = "Access Token is required.")]
        public string AccessToken { get; set; }

        public string Message { get; set; }

        public string FullName { get; set; }  

        public string Email { get; set; }

        public string RoleName { get; set; }

        public int UserId { get; set; }


    }
}
