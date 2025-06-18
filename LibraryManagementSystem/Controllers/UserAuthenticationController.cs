using LibraryManagementSystem.DTOs.ResetPasswordDTOs;
using LibraryManagementSystem.DTOs.UserAuthenticationDTOs;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [ApiController]
    [Route("UserAuthenticationController")]
    
    public class UserAuthenticationController : ControllerBase
    {
        private readonly IUserAuthenticationService _userAuthenticationService;

        public UserAuthenticationController(IUserAuthenticationService userAuthenticationService)
        {
            _userAuthenticationService = userAuthenticationService;
        }

        
       
        [HttpPost("LogInUser")]
        public async Task<IActionResult> LogInUserAsync([FromBody] UserLoginDTO userDto)

        {
            try
            {
                var response = await _userAuthenticationService.LogInUserAsync(userDto);

                if (response == null)
                {
                    return BadRequest("Error in creating Token/ Invalid Email Address / Invalid Password/Student in Block Mode");
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPost("RequestResetPassword")]
        public async Task<IActionResult> RequestResetPassword([FromBody] ResetRequestPasswordDTO request)
        {
            try
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

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }


      
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDto)
        {
            try
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
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
