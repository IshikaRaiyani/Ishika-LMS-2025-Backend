using LibraryManagementSystem.DTOs.StudentDTOs;
using LibraryManagementSystem.DTOs.UserDTOs;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using LibraryManagementSystem.Services.Implementations;
using Microsoft.AspNetCore.Authorization;


namespace LibraryManagementSystem.Controllers
{
    [ApiController]
    [Route("StudentController")]
    [Authorize(Roles = "Student")]

    public class StudentController : ControllerBase
    {
        private readonly IStudentService _userServices;


        public StudentController(IStudentService userServices)
        {
            _userServices = userServices;
        }

        //[AllowAnonymous]
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

      
        
        [HttpPost("BorrowBookRequest")]

        public async Task<IActionResult> BorrowBookRequestAsync(BorrowBookRequestDTO borrowBookRequestdto)
        {
            var response = await _userServices.BorrowBookRequestAsync(borrowBookRequestdto);
            if (response == null)
            {
                return BadRequest("Something went wrong!");
            }
            return Ok(response);

        }


        [HttpGet("GetBookingHistory")]

        public async Task<IActionResult> GetBookingHistory(int Userid)
        {
            var bookingHistory = await _userServices.GetBookingHistory(Userid);

            if (bookingHistory == null)
            {
                return NotFound("No History Found!");
            }

            return Ok(bookingHistory);
        }

        [HttpGet("GetPendingRequest")]
        public async Task<IActionResult> GetPendingRequest(int Userid)
        {
            var pendingrequest = await _userServices.GetPendingRequest(Userid);

            if (pendingrequest == null)
            {
                return NotFound("No History Found!");
            }

            return Ok(pendingrequest);
        }

        [HttpGet("CurrentBooking")]
        public async Task<IActionResult> CurrentBookingAsync(int Userid)
        {
            var currentbooking = await _userServices.CurrentBookingAsync(Userid);

            if (currentbooking == null)
            {
                return NotFound("No Current Booking Found!");
            }

            return Ok(currentbooking);
        }

        [HttpPut("ReturnBookRequest")]

        public async Task<IActionResult> ReturnBookRequestAsync([FromBody] int TransactionId)
        {
            var response = await _userServices.ReturnBookRequestAsync(TransactionId);
            if (response == null) return NotFound("Book not found.");

            return Ok("Return Request Submitted Successfully.");
        }










    }
}
