using LibraryManagementSystem.DTOs.UserDTOs;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [ApiController]
    [Route("StudentController")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _userServices;

        public StudentController(IStudentService userServices)
        {
            _userServices = userServices;
        }

        [HttpPost("RegistrationStudent")]
        public async Task<IActionResult> RegistrationStudentAsync([FromBody] RegisterStudentDTO UserAdminDto)
        {
            var response = await _userServices.RegistrationStudentAsync(UserAdminDto);
            if (response == null)
            {

                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
