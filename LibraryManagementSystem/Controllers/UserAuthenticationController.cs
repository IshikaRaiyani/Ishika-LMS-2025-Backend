using LibraryManagementSystem.DTOs.ResetPasswordDTOs;
using LibraryManagementSystem.DTOs.UserAuthenticationDTOs;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [ApiController]
    [Route("UserAuthenticationController")]
    [Authorize(Roles = "Admin, Librarian, Student")]
    public class UserAuthenticationController : ControllerBase
    {
        private readonly IUserAuthenticationService _userAuthenticationService;

        public UserAuthenticationController(IUserAuthenticationService userAuthenticationService)
        {
            _userAuthenticationService = userAuthenticationService;
        }

        //[Authorize(Roles = "Admin, Librarian, Student")]
        [AllowAnonymous]
        [HttpPost("LogInUser")]
        public async Task<IActionResult> LogInUserAsync([FromBody] UserLoginDTO userDto)

        {

            var response = await _userAuthenticationService.LogInUserAsync(userDto);

            if (response == null)
            {
                return BadRequest("Error in creating Token/ Invalid Email Address / Invalid Password");
            }

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("RequestResetPassword")]
        public async Task<IActionResult> RequestResetPassword([FromBody] ResetRequestPasswordDTO request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Email))
            {
                return BadRequest("Invalid email address.");
            }

            string result = await _userAuthenticationService.ResetPasswordRequestAsync(request.Email);

            if (result.StartsWith("An error"))
            {
                return BadRequest(new { error = result });
            }

         
            return Ok(new { resetToken = result });
        }


        [AllowAnonymous]
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDto)
        {
            if (resetPasswordDto == null)
            {
                return BadRequest("Invalid request payload.");
            }

            string result = await _userAuthenticationService.ResetPasswordAsync(resetPasswordDto);

            if (result == "PASSWORD RESET SUCCESSFULLY")
            {
                return Ok(new { message = result });
            }
            else
            {
                return BadRequest(new { error = result });
            }
        }
    }
}
