﻿using System.Net;
using System.Text.RegularExpressions;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.DTOs.AdminDTOs;
using LibraryManagementSystem.DTOs.BookDTOs;
using LibraryManagementSystem.DTOs.LibrarianDTOs;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Repositories.Implementations
{
    public class AdminRepository : IAdminRepository
    {
        private readonly DataContext _context;

        public AdminRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> IsEmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<User> GetUserByIDAsync(int userid)
        {
            
            return await _context.Users.FirstOrDefaultAsync(u => u.UserId == userid);


        }


        public async Task<string> GetUserRoleAsync(int userid)
        {
            return await _context.Users.Where(u => u.UserId == userid).Select(u => u.RoleName).FirstOrDefaultAsync();




        }

        public async Task<Book> GetBookByIDAsync(int bookId)
        {
            return await _context.Books.FirstOrDefaultAsync(b => b.BookId == bookId);
        }

        public async Task AddAdminAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task AddLibrarianAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<string> DeleteLibrarianAsync(User user)
        {
            try
            {


                var users = _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return "Librarian Deleted Successfully";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return "Error in Deleting Librarian";
            }
        }

        public async Task StudentStatusUpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<string> GetActiveUsersAsync()
        {
            var count = await _context.Users.CountAsync(u => u.Status == "Active");

            return count.ToString();

        }

        public async Task<string> GetBlockedUsersAsync()
        {
            var count = await _context.Users.CountAsync(u => u.Status == "Blocked");

            return count.ToString();

        }

        public async Task<string> GetUserCountAsync()
        {
            var count = await _context.Users.CountAsync();

            return count.ToString();

        }

        public async Task<List<AllUsersDTO>> GetAllUsersAsync()
        {
            try
            {
                var users = await _context.Users
              .Select(u => new AllUsersDTO
              {
                  UserId = u.UserId,
                  RoleName = u.RoleName,
                  FullName = u.FullName,
                  Email = u.Email,
                  Status = u.Status,
                  NoofBooks = u.NoofBooks
              })

              .AsNoTracking()
               .ToListAsync();

                return users;
            }
            catch (Exception ex)
            {

                throw new Exception("User Details Not Found!!");
            }




        }

        public async Task AddBookAsync(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        public async Task<string> GetTotalBookCountAsync()
        {


            var count = await _context.Books.Select(o => o.TotalCopies).SumAsync();

            return count.ToString();

        }

        public async Task<string> GetTotalAvailableBooksAsync()
        {


            var count = await _context.Books.Select(o => o.AvailableCopies).SumAsync();

            return count.ToString();

        }

        public async Task<List<GetAllBooksDTO>> GetAllBooksAsync()
        {
            try
            {
                var allBooks = await _context.Books.Select(book => new GetAllBooksDTO
                {
                    BookId = book.BookId,
                    Title = book.Title,
                    Author = book.Author,
                    Genre = book.Genre,
                    TotalCopies = book.TotalCopies,
                    AvailableCopies = book.AvailableCopies,
                    Description = book.Description,
                    IsBestSelling = book.IsBestSelling,
                    AddedOn = DateOnly.FromDateTime(DateTime.Today),
                    ImageUrl = book.ImageUrl
                }).AsNoTracking().ToListAsync();

                return allBooks;
            }
            catch (Exception ex)
            {

                throw new Exception("Books Details Not Found!!");
            }


        }

        public async Task<GetBookByIdDTO> GetBookByIdAsync(int Bookid)
        {
            var book = await _context.Books.FindAsync(Bookid);

            if (book == null) return null;
            return new GetBookByIdDTO
            {

                BookId = book.BookId,
                Title = book.Title,
                Author = book.Author,
                Genre = book.Genre,
                TotalCopies = book.TotalCopies,
                AvailableCopies = book.AvailableCopies,
                Description = book.Description,
                IsBestSelling = book.IsBestSelling,
                ImageUrl = book.ImageUrl
            };
        }


        public async Task<string> DeleteBookAsync(Book book)
        {
            try
            {
                var users = _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                return "Book Deleted Successfully";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return "Error in Deleting Book";
            }
        }

        public async Task<Book?> GetBookbyIdAsync(int bookId)
        {
            return await _context.Books.FindAsync(bookId);
        }

        public async Task<bool> UpdateBookAsync(Book book)
        {
            _context.Books.Update(book);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<DashboardDTO> GetDashboardMetricsAsync()
        {
            var now = DateTime.UtcNow;
            var today = DateOnly.FromDateTime(now);
            var startOfWeek = DateOnly.FromDateTime(now.AddDays(-(int)now.DayOfWeek));
            var startOfMonth = new DateOnly(now.Year, now.Month, 1);

            var totalTransactions = await _context.BookManagement.CountAsync();

            var booksIssuedThisWeek = await _context.BookManagement
               .Where(b => (b.RequestType == "Borrowed" || b.RequestType == "ReturnRequested" || b.RequestType == "Returned") && b.BorrowDate >= startOfWeek)
               .CountAsync();
            

            var booksIssuedThisMonth = await _context.BookManagement
            .Where(b => (b.RequestType == "Borrowed" || b.RequestType == "ReturnRequested" || b.RequestType == "Returned") && b.BorrowDate >= startOfMonth)
            .CountAsync();

            var mostIssuedBookId = await _context.BookManagement
            .Where(b => (b.RequestType == "Borrowed" || b.RequestType == "ReturnRequested" || b.RequestType == "Returned"))
            .GroupBy(b => b.BookId)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key)
            .FirstOrDefaultAsync();

            var mostIssuedBookTitle = await _context.Books
                .Where(b => b.BookId == mostIssuedBookId)
                .Select(b => b.Title)
                .FirstOrDefaultAsync();

            var totalOverdue = await _context.BookManagement
            .Where(b => (b.ReturnDate == null && b.DueDate < today) ||
                        (b.ReturnStatus == "Approved" && b.ReturnDate > b.DueDate))
            .CountAsync();


            var totalFine = await _context.BookManagement.SumAsync(b => b.Fine);

            var activeUsersThisWeek = await _context.BookManagement
                .Where(b => (b.RequestType == "Borrowed" || b.RequestType == "ReturnRequested" || b.RequestType == "Returned") && (b.BorrowDate >= startOfWeek))
                .Select(b => b.UserId)
                .Distinct()
                .CountAsync();

            var activeUsersThisMonth = await _context.BookManagement
            .Where(b => (b.RequestType == "Borrowed" || b.RequestType == "ReturnRequested" || b.RequestType == "Returned") && (b.BorrowDate >= startOfMonth))
            .Select(b => b.UserId)
            .Distinct()
            .CountAsync();


            return new DashboardDTO
            {
                TotalTransactions = totalTransactions,
                BooksIssuedThisWeek = booksIssuedThisWeek,
                BooksIssuedThisMonth = booksIssuedThisMonth,
                MostIssuedBookTitle = mostIssuedBookTitle,
                TotalOverdueTransactions = totalOverdue,
                TotalFineAmount = totalFine,
                ActiveUsersThisWeek = activeUsersThisWeek,
                ActiveUsersThisMonth = activeUsersThisMonth
            };














        }

        public async Task<List<TransactionDetailDTO>> GetAllTransactionsAsync()
        {
            return await _context.BookManagement
                .Include(bm => bm.User)
                .Include(bm => bm.Book)
                .Select(bm => new TransactionDetailDTO
                {
                    TransactionId = bm.TransactionId,
                    UserName = bm.User.FullName,
                    BookTitle = bm.Book.Title,
                    RequestType = bm.RequestType,
                    BorrowDate = bm.BorrowDate,
                    ReturnDate = bm.ReturnDate,
                    Fine = bm.Fine,
                    BorrowStatus = bm.BorrowStatus,
                    ReturnStatus = bm.ReturnStatus
                })
                .ToListAsync();
        }
    }
}
