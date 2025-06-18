using LibraryManagementSystem.DTOs.StudentDTOs;
using LibraryManagementSystem.DTOs.UserDTOs;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using LibraryManagementSystem.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using LibraryManagementSystem.DTOs.BookDTOs;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Http.HttpResults;


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

        [HttpPost("RegistrationStudent")]
        public async Task<IActionResult> RegistrationStudentAsync([FromBody] RegisterStudentDTO UserAdminDto)
        {
            try
            {
                var response = await _userServices.RegistrationStudentAsync(UserAdminDto);
                if (response == null)
                {
                    return BadRequest(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("BorrowBookRequest")]
        public async Task<IActionResult> BorrowBookRequestAsync(BorrowBookRequestDTO borrowBookRequestdto)
        {
            try
            {
                var response = await _userServices.BorrowBookRequestAsync(borrowBookRequestdto);
                if (response == null)
                {
                    return BadRequest("Something went wrong!");
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet("GetBookingHistory")]
        public async Task<IActionResult> GetBookingHistoryAsync(int Userid)
        {
            try {
                var bookingHistory = await _userServices.GetBookingHistoryAsync(Userid);
                if (bookingHistory == null)
                {
                    return NotFound("No History Found!");
                }
                return Ok(bookingHistory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetPendingRequest")]
        public async Task<IActionResult> GetPendingRequestAsync(int Userid)
        {
            try
            {

                var pendingrequest = await _userServices.GetPendingRequestAsync(Userid);
                if (pendingrequest == null)
                {
                    return NotFound("No History Found!");
                }
                return Ok(pendingrequest);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("CurrentBooking")]
        public async Task<IActionResult> CurrentBookingAsync(int Userid)
        {
            try
            {
                var currentbooking = await _userServices.CurrentBookingAsync(Userid);
                if (currentbooking == null)
                {
                    return NotFound("No Current Booking Found!");
                }
                return Ok(currentbooking);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPut("ReturnBookRequest")]
        public async Task<IActionResult> ReturnBookRequestAsync([FromBody] int TransactionId)
        {
            try
            {
                var response = await _userServices.ReturnBookRequestAsync(TransactionId);
                if (response == null) return NotFound("Book not found.");
                return Ok("Return Request Submitted Successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPost("AddtoWishlist")]
        public async Task<IActionResult> AddtoWishlistAsync(int userId, int bookId)
        {
            try
            {
                var response = await _userServices.AddtoWishlistAsync(userId, bookId);
                if (response.Contains("already", StringComparison.OrdinalIgnoreCase))
                {
                    return Conflict(response);
                }
                if (response == null)
                {
                    return BadRequest("Book Not Added to your wishlist");
                }
                return Ok("Book successfully added to your wishlist..");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet("GetUserWishlists")]
        public async Task<IActionResult> GetUserWishlistsAsync([FromQuery] int userId)
        {
            try
            {

                var response = await _userServices.GetUserWishlistsAsync(userId);
                if (response == null)
                {
                    return BadRequest("Your wishlist is empty!");
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpDelete("RemoveFromWishlist")]
        public async Task<IActionResult> RemoveFromWishlistAsync([FromQuery] int wishlistId)
        {
            try
            {
                var result = await _userServices.RemoveFromWishlistAsync(wishlistId);
                if (!result)
                {
                    return NotFound("Wishlist item not found");
                }
                return Ok("Removed from wishlist");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet("GetBestSellingBooks")]
        public async Task<ActionResult<IEnumerable<GetAllBooksDTO>>> GetBestSellingBooksAsync()
        {
            try
            {
                var allBooks = await _userServices.GetBestSellingBooksAsync();

                if (allBooks == null)
                {
                    return BadRequest("No best selling books found!");
                }
                return Ok(allBooks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet("GetNewArrivals")]
        public async Task<ActionResult<IEnumerable<GetAllBooksDTO>>> GetNewArrivalsAsync()
        {
            try
            {
                var allBooks = await _userServices.GetNewArrivalsAsync();

                if (allBooks == null)
                {
                    return BadRequest("No new arrivals of books found!");
                }
                return Ok(allBooks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet("BookRecommendations")]

        public async Task<List<Book>> BookRecommendationsAsync(int userid)
        {
                var recommendation = await _userServices.BookRecommendationsAsync(userid);

                return recommendation;
            
        }

    }

}

