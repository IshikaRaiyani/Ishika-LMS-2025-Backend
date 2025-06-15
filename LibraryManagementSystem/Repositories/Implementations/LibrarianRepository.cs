using LibraryManagementSystem.Data;
using LibraryManagementSystem.DTOs.LibrarianDTOs;
using LibraryManagementSystem.DTOs.StudentDTOs;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Repositories.Implementations
{
    public class LibrarianRepository : ILibrarianRepository
    {
        private readonly DataContext _context;

        public LibrarianRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<BookManagement> GetTransactionById(int TransactionId)
        {
            return await _context.BookManagement.FindAsync(TransactionId);
        }

        public async Task<int> GetUserFineAsync(int UserId)
        {
            return await _context.BookManagement.Where(t => t.UserId == UserId).SumAsync(t => t.Fine);
        }

        public async Task<bool> ApproveBorrowRequestAsync(BookManagement bookManagement)
        {
             _context.BookManagement.Update(bookManagement);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> RejectBorrowRequestAsync(BookManagement bookManagement)
        {
            _context.BookManagement.Update(bookManagement);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        
        public async Task<List<GetPendingBorrowRequestsDTO>> GetPendingBorrowRequestsAsync()
        {
            try
            {
                var pendingRequests = await _context.BookManagement
                    .Where(br => br.BorrowStatus == "Pending")
                    .Include(br => br.User)
                    .Include(br => br.Book)
                    .Select(br => new GetPendingBorrowRequestsDTO
                    {
                        UserId = br.UserId,
                        FullName = br.User.FullName,
                        NoofBooks = br.User.NoofBooks,
                        BookId = br.BookId,
                        Title = br.Book.Title,
                        AvailableCopies = br.Book.AvailableCopies,
                        BorrowRequestDate = br.BorrowRequestDate,
                        BorrowStatus = br.BorrowStatus
                    })
                    .ToListAsync();

                return pendingRequests;
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching pending borrow requests", ex);
            }
        }

        public async Task<bool> ApproveReturnRequestAsync(BookManagement bookManagement)
        {
            _context.BookManagement.Update(bookManagement);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }





    }

}

