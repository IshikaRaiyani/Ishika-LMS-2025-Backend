using LibraryManagementSystem.DTOs.StudentDTOs;
using LibraryManagementSystem.DTOs.UserDTOs;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using LibraryManagementSystem.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using LibraryManagementSystem.DTOs.BookDTOs;
using LibraryManagementSystem.Models;


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
        public async Task<IActionResult> GetBookingHistoryAsync(int Userid)
        {
            var bookingHistory = await _userServices.GetBookingHistoryAsync(Userid);
            if (bookingHistory == null)
            {
                return NotFound("No History Found!");
            }
            return Ok(bookingHistory);
        }

        [HttpGet("GetPendingRequest")]
        public async Task<IActionResult> GetPendingRequestAsync(int Userid)
        {
            var pendingrequest = await _userServices.GetPendingRequestAsync(Userid);
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

        [HttpPost("AddtoWishlist")]
        public async Task<IActionResult> AddtoWishlistAsync(int userId, int bookId)
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

        [HttpGet("GetUserWishlists")]
        public async Task<IActionResult> GetUserWishlistsAsync([FromQuery] int userId)
        {
            var response = await _userServices.GetUserWishlistsAsync(userId);
            if (response == null)
            {
                return BadRequest("Your wishlist is empty!");
            }
            return Ok(response);
        }

        [HttpDelete("RemoveFromWishlist")]
        public async Task<IActionResult> RemoveFromWishlistAsync([FromQuery] int wishlistId)
        {
            var result = await _userServices.RemoveFromWishlistAsync(wishlistId);
            if (!result)
            {
                return NotFound("Wishlist item not found");
            }
            return Ok("Removed from wishlist");
        }

        [HttpGet("GetBestSellingBooks")]
        public async Task<ActionResult<IEnumerable<GetAllBooksDTO>>> GetBestSellingBooksAsync()
        {
            var allBooks = await _userServices.GetBestSellingBooksAsync();

            if (allBooks == null)
            {
                return BadRequest("No best selling books found!");
            }
            return Ok(allBooks);
        }

        [HttpGet("GetNewArrivals")]
        public async Task<ActionResult<IEnumerable<GetAllBooksDTO>>> GetNewArrivalsAsync()
        {
            var allBooks = await _userServices.GetNewArrivalsAsync();

            if (allBooks == null)
            {
                return BadRequest("No new arrivals of books found!");
            }
            return Ok(allBooks);
        }

        [HttpGet("BookRecommendations")]

        public async Task<List<Book>> BookRecommendationsAsync(int userid)
        {
            var recommendation = await _userServices.BookRecommendationsAsync(userid);

            return recommendation;
        }


       



    }

}

