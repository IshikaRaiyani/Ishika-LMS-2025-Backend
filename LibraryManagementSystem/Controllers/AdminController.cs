using LibraryManagementSystem.DTOs.AdminDTOs;
using LibraryManagementSystem.DTOs.LibrarianDTOs;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [ApiController]
    [Route("AddAdminController")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminServices;

        public AdminController(IAdminService adminServices)
        {
            _adminServices = adminServices;
        }

        [HttpPost("AddAdmin")]
        public async Task<IActionResult> AddAdminAsync([FromBody] AddAdminDTO addAdminDto)
        {
            var response = await _adminServices.AddAdminAsync(addAdminDto);
            if (response == null)
            {

                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("AddLibrarian")]
        public async Task<IActionResult> AddLibrarianAsync([FromBody] AddLibrarianDTO addLibrarianDto)
        {
            var response = await _adminServices.AddLibrarianAsync(addLibrarianDto);
            if (response == null)
            {

                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("DeleteLibrarian")]
        public async Task<IActionResult> DeleteLibrarianAsync([FromBody] int userId)
        {
            var response = await _adminServices.DeleteLibrarianAsync(userId);
            if (response == null)
            {

                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("BlockStudent")]
        public async Task<IActionResult> BlockStudentAsync(UpdateStudentStatusDTO updateStudentStatusDto)
        {
            var response = await _adminServices.BlockStudentAsync(updateStudentStatusDto);

            var BlockStatus = "Student Blocked Successfully";

            if (response != "BlockStatus")
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("UnblockStudent")]
        public async Task<IActionResult> UnblockStudentAsync(UpdateStudentStatusDTO updateStudentStatusDto)
        {
            var response = await _adminServices.UnblockStudentAsync(updateStudentStatusDto);

            var BlockStatus = "Student Unblocked Successfully";

            if (response != "BlockStatus")
            {
                return BadRequest(response);
            }
            return Ok(response);
        }



    }
}
