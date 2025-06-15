using LibraryManagementSystem.Data;
using LibraryManagementSystem.DTOs.StudentDTOs;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace LibraryManagementSystem.Repositories.Implementations
{
    public class StudentRepository : IStudentRepository
    {
        private readonly DataContext _context;

        public StudentRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task AddStudentAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetUserBooks(int UserId)
        {
            var users = await _context.Users.Where(u => u.UserId == UserId).Select(u => u.NoofBooks).FirstOrDefaultAsync();
            return users;
        }

        public async Task BorrowBookRequestAsync(BookManagement bookManagement)
        {
            await _context.BookManagement.AddAsync(bookManagement);
            await _context.SaveChangesAsync();
        }   

        public async Task<User> GetUserByIDAsync(int userid)
        {
            //return await _context.Users.FindAsync(userid);
            return await _context.Users.FirstOrDefaultAsync(u => u.UserId == userid);


        }

        public async Task<bool> isBookExistingAsync(int userid, int bookid, string borrowStatus, string borrowStatus1, string returnStatus, string returnStatus1)
        {
            return await _context.BookManagement.AnyAsync(b => b.UserId == userid && b.BookId == bookid && (b.BorrowStatus == borrowStatus || b.BorrowStatus == borrowStatus1) && (b.ReturnStatus == returnStatus || b.ReturnStatus == returnStatus1));



        }

        public async Task<List<BookingHistoryDTO>> GetBookingHistory(int UserId)
        {
            return await _context.BookManagement.Where(t => t.UserId == UserId && t.BorrowStatus=="Approved" && t.ReturnStatus=="Approved").Select(t => new BookingHistoryDTO
            {
                Title = t.Book.Title,
                Author = t.Book.Author,
                Genre = t.Book.Genre,
                BorrowDate = t.BorrowDate.Value,
                ReturnDate = t.ReturnDate.Value,
                Fine = t.Fine,
            }).ToListAsync();

            
        }

        public async Task<List<PendingRequestDTO>> GetPendingRequest(int UserId)
        {
            return await _context.BookManagement.Where(t => t.UserId == UserId && (t.BorrowStatus == "Pending" || t.BorrowStatus == "Approved") && t.ReturnStatus == "Pending").Select(t => new PendingRequestDTO
            {
                Title = t.Book.Title,
                Author = t.Book.Author,
                BorrowRequestDate = t.BorrowRequestDate,
                BorrowDate = t.BorrowDate ?? DateOnly.MinValue,

                DueDate = t.DueDate ?? DateOnly.MinValue,
                BorrowStatus = t.BorrowStatus,
                ReturnStatus= t.ReturnStatus
            }).ToListAsync();


        }

        public async Task<List<CurrentBookingDTO>> CurrentBookingAsync(int UserId)
        {
            return await _context.BookManagement.Where(t => t.UserId == UserId && t.BorrowStatus == "Approved" && t.ReturnStatus == "None").Select(t => new CurrentBookingDTO
            {
                TransactionId = t.TransactionId,
                Title = t.Book.Title,
                Author = t.Book.Author,
                Genre = t.Book.Genre,
                BorrowDate = t.BorrowDate.Value,
                DueDate = t.DueDate.Value,
               
            }).ToListAsync();


        }

        public async Task<bool> ReturnBookRequestAsync(BookManagement bookManagement)
        {
            _context.BookManagement.Update(bookManagement);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }


    }


}

