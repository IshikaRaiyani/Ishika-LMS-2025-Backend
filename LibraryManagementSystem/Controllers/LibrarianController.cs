using Azure;
using LibraryManagementSystem.DTOs.BookDTOs;
using LibraryManagementSystem.DTOs.StudentDTOs;
using LibraryManagementSystem.Services.Implementations;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{

    [ApiController]
    [Route("LibrarianController")]
    [Authorize(Roles = "Librarian")]
    public class LibrarianController : ControllerBase
    {
        private readonly ILibrarianService _userServices;

        public LibrarianController(ILibrarianService userServices)
        {
            _userServices = userServices;
        }

        [HttpPut("ApproveBorrowRequest")]
        public async Task<IActionResult> ApproveBorrowRequestAsync([FromBody] int TransactionId)
        {
            var response = await _userServices.ApproveBorrowRequestAsync(TransactionId);
            if (response == null || response == "Book not found." || response == "User not found." || response == "Transaction not found")
            {
                return NotFound(response);
            }

            if (response.Contains("cannot be approved"))
            {
                return Conflict(response); 
            }

            return Ok("Borrow Request Approved Successfully");
           
        }

        [HttpPut("RejectBorrowRequest")]
        public async Task<IActionResult> RejectBorrowRequestAsync([FromBody] int TransactionId)
        {
            var response = await _userServices.RejectBorrowRequestAsync(TransactionId);
            if (response == null) return NotFound("Book not found.");

            return Ok("Borrow Request Rejected Successfully.");
        }

        [HttpGet("GetPendingBorrowRequests")]
        public async Task<IActionResult> GetPendingBorrowRequestsAsync()
        {
            var response = await _userServices.GetPendingBorrowRequestsAsync();

            if (response == null)
            {
                return BadRequest("No pending borrow request found!");
            }
            return Ok(response);
        }

        [HttpPut("ApproveReturnRequest")]
        public async Task<IActionResult> ApproveReturnRequestAsync([FromBody] int TransactionId)
        {
            var response = await _userServices.ApproveReturnRequestAsync(TransactionId);
            if (response == null) return NotFound("Book not found.");

            return Ok("Return Request Approved Successfully.");
        }

        [HttpGet("GetPendingReturnRequests")]
        public async Task<IActionResult> GetPendingReturnRequestsAsync()
        {
            var response = await _userServices.GetPendingReturnRequestsAsync();

            if (response == null)
            {
                return BadRequest("No pending return request found!");
            }
            return Ok(response);
        }


    }  
   
}

     
    
    

