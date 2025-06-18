using System.Globalization;
using System.Linq;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.DTOs.BookDTOs;
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

        public async Task<int> GetUserBooksAsync(int UserId)
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

        public async Task<List<BookingHistoryDTO>> GetBookingHistoryAsync(int UserId)
        {
            return await _context.BookManagement.Where(t => t.UserId == UserId && (t.BorrowStatus=="Approved" || t.BorrowStatus == "Rejected") && (t.ReturnStatus=="Approved" || t.ReturnStatus == "Rejected")).Select(t => new BookingHistoryDTO
            {
                Title = t.Book.Title,
                Author = t.Book.Author,
                Genre = t.Book.Genre,
                BorrowStatus = t.BorrowStatus,
                BorrowDate = t.BorrowDate,
                ReturnStatus = t.ReturnStatus,
                ReturnDate = t.ReturnDate,

                Fine = t.Fine,
            }).ToListAsync();

            
        }

        public async Task<List<PendingRequestDTO>> GetPendingRequestAsync(int UserId)
        {
            return await _context.BookManagement.Where(t => t.UserId == UserId && (t.BorrowStatus == "Pending" || t.ReturnStatus == "Pending")).Select(t => new PendingRequestDTO
            {
                Title = t.Book.Title,
                Author = t.Book.Author,
                BorrowRequestDate = t.BorrowRequestDate,

               
                BorrowDate = t.BorrowDate,

                DueDate = t.DueDate,
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

        public async Task AddtoWishlistAsync(Studentwishlist studentwishlist)
        {
            await _context.studentwishlists.AddAsync(studentwishlist);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsBookInWishlistAsync(int userId, int bookId)
        {

            return await _context.studentwishlists.AnyAsync(w => w.UserId == userId && w.BookId == bookId);
        }

        public async Task<List<Studentwishlist>> GetUserWishlistsAsync(int userId)
        {
            return await _context.studentwishlists.Where(t => t.UserId == userId).Select(t => new Studentwishlist
            {
                WishlistId = t.WishlistId,
                UserId = t.UserId,
                BookId = t.BookId,
                AddedOn = t.AddedOn,
                Book = new Book
                {
                    Title = t.Book.Title,
                    Author = t.Book.Author,
                    Genre = t.Book.Genre,
                    Description = t.Book.Description,
                    ImageUrl = t.Book.ImageUrl,
                    
                   
                }

            }).ToListAsync();

        }

        public async Task<bool> RemoveFromWishlistAsync(int wishlistId)
        {
            var wishlistItem = await _context.studentwishlists.FirstOrDefaultAsync(w => w.WishlistId == wishlistId);


            if (wishlistItem == null)
            {
                return false; 
            }

            _context.studentwishlists.Remove(wishlistItem);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<List<GetAllBooksDTO>> GetBestSellingBooksAsync()
        {
            try
            {
                var allBooks = await _context.Books.Where(book=>book.IsBestSelling).Select(book => new GetAllBooksDTO
                {
                    BookId = book.BookId,
                    Title = book.Title,
                    Author = book.Author,
                    Genre = book.Genre,
                    TotalCopies = book.TotalCopies,
                    AvailableCopies = book.AvailableCopies,
                    Description = book.Description,
                    IsBestSelling = book.IsBestSelling,
                    AddedOn = book.AddedOn,
                    ImageUrl = book.ImageUrl
                }).AsNoTracking().ToListAsync();

                return allBooks;
            }
            catch (Exception ex)
            {

                throw new Exception("Books Details Not Found!!");
            }


        }

        public async Task<List<GetAllBooksDTO>> GetNewArrivalsAsync()
        {
            try
            {
                var today = DateOnly.FromDateTime(DateTime.Today);
                var last7days = today.AddDays(-7);
                var allBooks = await _context.Books.Where(book => book.AddedOn >= last7days && book.AddedOn <= today).Select(book => new GetAllBooksDTO
                {
                   
                    BookId = book.BookId,
                    Title = book.Title,
                    Author = book.Author,
                    Genre = book.Genre,
                    TotalCopies = book.TotalCopies,
                    AvailableCopies = book.AvailableCopies,
                    Description = book.Description,
                    IsBestSelling = book.IsBestSelling,
                    AddedOn = book.AddedOn,
                    ImageUrl = book.ImageUrl
                }).AsNoTracking().ToListAsync();

                return allBooks;
            }
            catch (Exception ex)
            {

                throw new Exception("Books Details Not Found!!");
            }


        }

        public async Task<List<Book>> GetRecommendedBooksAsync(int userid)
        {
            var history = await _context.BookManagement
                .Where(bm => bm.UserId == userid)
                .Include(bm => bm.Book)
                .Select(bm => new { bm.Book.Genre, bm.Book.Author, bm.BookId })
                .Distinct()
                .ToListAsync();

            if (history == null || !history.Any())
            {
                var topIssuedBookIds = await _context.BookManagement
                    .GroupBy(bm => bm.BookId)
                    .OrderByDescending(g => g.Count())
                    .Take(5)
                    .Select(g => g.Key)
                    .ToListAsync();

                return await _context.Books
                    .Where(b => topIssuedBookIds.Contains(b.BookId) && b.AvailableCopies > 0)
                    .ToListAsync();
            }

            var genres = history.Select(x => x.Genre).Distinct().ToList();
            var authors = history.Select(x => x.Author).Distinct().ToList();
            var borrowedBookIds = history.Select(x => x.BookId).ToList();

            return await _context.Books
                .Where(b =>
                    (genres.Contains(b.Genre) || authors.Contains(b.Author)) &&
                    !borrowedBookIds.Contains(b.BookId) && b.AvailableCopies > 0)
                .ToListAsync();
        }


        public async Task<List<Studentwishlist>> FindWishlistAsync(int bookId)
        {

            return await _context.studentwishlists.Where(w=> w.BookId == bookId).Include(w => w.User)
                .ToListAsync();
            
        }
    }


}

