using LibraryManagementSystem.DTOs.AccessTokenDTOs;
using LibraryManagementSystem.DTOs.ResetPasswordDTOs;
using LibraryManagementSystem.DTOs.UserAuthenticationDTOs;

namespace LibraryManagementSystem.Services.Interfaces
{
    public interface IUserAuthenticationService
    {
        Task<AccessTokenResponseDTO?> LogInUserAsync(UserLoginDTO userDto);

        Task<string> ResetPasswordAsync(ResetPasswordDTO resetPasswordDto);

        Task<string> ResetPasswordRequestAsync(string userEmail);
    }
}
