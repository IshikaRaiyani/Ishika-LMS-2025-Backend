using System.Security.Claims;
using LibraryManagementSystem.DTOs.AdminDTOs;
using LibraryManagementSystem.DTOs.BookDTOs;
using LibraryManagementSystem.DTOs.LibrarianDTOs;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [ApiController]
    [Route("AddAdminController")]
    //[Authorize(Roles = "Admin")]
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

        [HttpDelete("DeleteLibrarian/{userId}")]
        public async Task<IActionResult> DeleteLibrarianAsync(int userId)
        {
            var response = await _adminServices.DeleteLibrarianAsync(userId);
            if (response == null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPatch("BlockStudent")]
        public async Task<IActionResult> BlockStudentAsync([FromBody] UpdateStudentStatusDTO updateStudentStatusDto)
        {
            var response = await _adminServices.BlockStudentAsync(updateStudentStatusDto);

            if (response != "Student Blocked Successfully!")
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPatch("UnblockStudent")]
        public async Task<IActionResult> UnblockStudentAsync([FromBody] UpdateStudentStatusDTO updateStudentStatusDto)
        {
            var response = await _adminServices.UnblockStudentAsync(updateStudentStatusDto);

            if (response != "Student Unblocked Successfully!")
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("GetActiveUsers")]
        public async Task<IActionResult> GetActiveUsersAsync()
        {
            var activeUsers = await _adminServices.GetActiveUsersAsync();

            if (activeUsers == null)
            {
                return BadRequest("No Users Found!");
            }
            return Ok(activeUsers);
        }

        [HttpGet("GetBlockedUsers")]
        public async Task<IActionResult> GetBlockedUsers()
        {
            var blockedUsers = await _adminServices.GetBlockedUsersAsync();

            if (blockedUsers == null)
            {
                return BadRequest("No Users Found!");
            }
            return Ok(blockedUsers);
        }

        [HttpGet("GetUserCount")]
        public async Task<IActionResult> GetUserCount()
        {
            var userCount = await _adminServices.GetUserCountAsync();

            if (userCount == null)
            {
                return BadRequest("No Users Found!");
            }
            return Ok(userCount);
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var allUsers = await _adminServices.GetAllUsersAsync();

            if (allUsers == null)
            {
                return BadRequest("No Users Found!");
            }
            return Ok(allUsers);
        }

        [Authorize(Roles = "Admin, Librarian")]
        [AllowAnonymous]
        [HttpPost("AddBook")]
        public async Task<IActionResult> AddBookAsync([FromBody] Book book)
        {
            var response = await _adminServices.AddBookAsync(book);
            if (response == null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("GetTotalBookCount")]
        public async Task<IActionResult> GetTotalBookCount()
        {
            var bookCount = await _adminServices.GetTotalBookCountAsync();

            if (bookCount == null)
            {
                return BadRequest("No Books Found!");
            }
            return Ok(bookCount);
        }

        [HttpGet("GetTotalAvailableBooks")]
        public async Task<IActionResult> GetTotalAvailableBooksAsync()
        {
            var avaiable_books_count = await _adminServices.GetTotalAvailableBooksAsync();

            if (avaiable_books_count == null)
            {
                return BadRequest("No Books Found!");
            }
            return Ok(avaiable_books_count);
        }

        [Authorize(Roles ="Admin,Student,Librarian")]
        [HttpGet("GetAllBooks")]

        public async Task<ActionResult<IEnumerable<GetAllBooksDTO>>> GetAllBooksAsync()
        {
            var allBooks = await _adminServices.GetAllBooksAsync();

            if (allBooks == null)
            {
                return BadRequest("No Books Found!");
            }
            return Ok(allBooks);
        }

        [HttpGet("GetBookByid")]
        public async Task<ActionResult<GetBookByIdDTO>> GetBookById(int Bookid)
        {
            var getbook = await _adminServices.GetBookByIdAsync(Bookid);

            if (getbook == null)
            {
                throw new Exception("Book Id not found!");
            }

            return Ok(getbook);
        }

        [HttpDelete("DeleteBooks/{bookId}")]
        public async Task<IActionResult> DeleteBookAsync(int bookId)
        {
            var response = await _adminServices.DeleteBookAsync(bookId);
            if (response == null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin, Librarian")]
        [AllowAnonymous]
        [HttpPut("UpdateBookById")]
            public async Task<IActionResult> UpdateBook([FromBody] UpdateBookDTO dto)
            {
           
                var updated = await _adminServices.UpdateBookAsync(dto);
                if (!updated) return NotFound("Book not found.");

                return Ok("Book updated successfully.");
            }



    }

}

