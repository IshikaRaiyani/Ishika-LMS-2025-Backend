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
    
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminServices;

        public AdminController(IAdminService adminServices)
        {
            _adminServices = adminServices;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddAdmin")]
        public async Task<IActionResult> AddAdminAsync([FromBody] AddAdminDTO addAdminDto)
        {
            try
            {
                var response = await _adminServices.AddAdminAsync(addAdminDto);
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

        [Authorize(Roles = "Admin")]
        [HttpPost("AddLibrarian")]
        public async Task<IActionResult> AddLibrarianAsync([FromBody] AddLibrarianDTO addLibrarianDto)
        {
            try
            {
                var response = await _adminServices.AddLibrarianAsync(addLibrarianDto);
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

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteLibrarian/{userId}")]
        public async Task<IActionResult> DeleteLibrarianAsync(int userId)
        {
            try
            {
                var response = await _adminServices.DeleteLibrarianAsync(userId);
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

        [Authorize(Roles = "Admin")]
        [HttpPatch("BlockStudent")]
        public async Task<IActionResult> BlockStudentAsync([FromBody] UpdateStudentStatusDTO updateStudentStatusDto)
        {
            try
            {
                var response = await _adminServices.BlockStudentAsync(updateStudentStatusDto);

                if (response != "Student Blocked Successfully!")
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

        [Authorize(Roles = "Admin")]
        [HttpPatch("UnblockStudent")]
        public async Task<IActionResult> UnblockStudentAsync([FromBody] UpdateStudentStatusDTO updateStudentStatusDto)
        {
            try
            {
                var response = await _adminServices.UnblockStudentAsync(updateStudentStatusDto);

                if (response != "Student Unblocked Successfully!")
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

        [Authorize(Roles = "Admin")]
        [HttpGet("GetActiveUsers")]
        public async Task<IActionResult> GetActiveUsersAsync()
        {
            try
            {
                var activeUsers = await _adminServices.GetActiveUsersAsync();

                if (activeUsers == null)
                {
                    return BadRequest("No Users Found!");
                }
                return Ok(activeUsers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [Authorize(Roles = "Admin")]

        [HttpGet("GetBlockedUsers")]
        public async Task<IActionResult> GetBlockedUsers()
        {
            try
            {
                var blockedUsers = await _adminServices.GetBlockedUsersAsync();

                if (blockedUsers == null)
                {
                    return BadRequest("No Users Found!");
                }
                return Ok(blockedUsers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }


        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetUserCount")]
        public async Task<IActionResult> GetUserCount()
        {
            try
            {
                var userCount = await _adminServices.GetUserCountAsync();

                if (userCount == null)
                {
                    return BadRequest("No Users Found!");
                }
                return Ok(userCount);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }



        [Authorize(Roles = "Admin, Librarian")]
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            try
            {
                var allUsers = await _adminServices.GetAllUsersAsync();

                if (allUsers == null)
                {
                    return BadRequest("No Users Found!");
                }
                return Ok(allUsers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }


        }

        [Authorize(Roles = "Admin, Librarian")]
       
        [HttpPost("AddBook")]
        public async Task<IActionResult> AddBookAsync([FromBody] Book book)
        {
            try
            {
                var response = await _adminServices.AddBookAsync(book);
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

        [Authorize(Roles = "Admin, Librarian")]

        [HttpGet("GetTotalBookCount")]
        public async Task<IActionResult> GetTotalBookCount()
        {
            try
            {
                var bookCount = await _adminServices.GetTotalBookCountAsync();

                if (bookCount == null)
                {
                    return BadRequest("No Books Found!");
                }
                return Ok(bookCount);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }



        }

        [Authorize(Roles = "Admin, Librarian")]
        [HttpGet("GetTotalAvailableBooks")]
        public async Task<IActionResult> GetTotalAvailableBooksAsync()
        {
            try
            {
                var avaiable_books_count = await _adminServices.GetTotalAvailableBooksAsync();

                if (avaiable_books_count == null)
                {
                    return BadRequest("No Books Found!");
                }
                return Ok(avaiable_books_count);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }


        }

        [Authorize(Roles ="Admin,Student,Librarian")]
        [HttpGet("GetAllBooks")]

        public async Task<ActionResult<IEnumerable<GetAllBooksDTO>>> GetAllBooksAsync()
        {
            try
            {
                var allBooks = await _adminServices.GetAllBooksAsync();

                if (allBooks == null)
                {
                    return BadRequest("No Books Found!");
                }
                return Ok(allBooks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }


        }

        [Authorize(Roles = "Admin, Librarain, Student")]

        [HttpGet("GetBookByid")]
        public async Task<ActionResult<GetBookByIdDTO>> GetBookById(int Bookid)
        {
            try
            {
                var getbook = await _adminServices.GetBookByIdAsync(Bookid);

                if (getbook == null)
                {
                    throw new Exception("Book Id not found!");
                }

                return Ok(getbook);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }


        }

        [Authorize(Roles = "Admin")]

        [HttpDelete("DeleteBooks/{bookId}")]
        public async Task<IActionResult> DeleteBookAsync(int bookId)
        {
            try
            {
                var response = await _adminServices.DeleteBookAsync(bookId);
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

        [Authorize(Roles = "Admin, Librarian")]
        
        [HttpPut("UpdateBookById")]
        public async Task<IActionResult> UpdateBook([FromBody] UpdateBookDTO dto)
        {
            try
            {
                var updated = await _adminServices.UpdateBookAsync(dto);
                if (!updated) return NotFound("Book not found.");

                return Ok("Book updated successfully.");
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }



        }

        [Authorize(Roles = "Admin")]

        [HttpGet("Dashboardmetrics")]
        public async Task<ActionResult<DashboardDTO>> GetDashboardMetricsAsync()
        {
            try
            {
                var metrics = await _adminServices.GetDashboardMetricsAsync();
                return Ok(metrics);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);

            }

        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllTransactions")]

        public async Task<IActionResult> GetAllTransactionsAsync()
        {
            try
            {
                var transactions = await _adminServices.GetAllTransactionsAsync();
                if (transactions == null || !transactions.Any())
                {
                    return NotFound("No transactions found.");
                }

                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);

            }

        }



    }

}

