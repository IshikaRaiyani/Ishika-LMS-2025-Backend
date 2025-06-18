using System;
using System.Security.Cryptography;
using System.Text;
using LibraryManagementSystem.DTOs.BookDTOs;
using LibraryManagementSystem.DTOs.StudentDTOs;
using LibraryManagementSystem.DTOs.UserDTOs;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories.Implementations;
using LibraryManagementSystem.Repositories.Interfaces;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Reflection.Metadata.BlobBuilder;

namespace LibraryManagementSystem.Services.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly ILibrarianRepository _librarianRepository;
        private readonly IEmailService _emailService;

        public StudentService(IStudentRepository studentRepository, IAdminRepository adminRepository, ILibrarianRepository librarianRepository, IEmailService emailService)
        {
            _studentRepository = studentRepository;
            _adminRepository = adminRepository;
            _librarianRepository = librarianRepository;
            _emailService = emailService;

        }
        public async Task<string> RegistrationStudentAsync(RegisterStudentDTO registerStudentDTO)
        {
            try
            {
                if (registerStudentDTO == null)
                {
                    throw new Exception("Please enter the valid User Admin Details.");
                }

                if (await _studentRepository.IsEmailExistsAsync(registerStudentDTO.Email))
                {
                    return "This Email Address already exists. Please enter a valid Email Address.";
                }

                var sha256 = SHA256.Create();
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(registerStudentDTO.Password));

                var UserAdmin = new User
                {
                    FullName = registerStudentDTO.FullName,
                    Email = registerStudentDTO.Email,
                    Password = Convert.ToBase64String(hashBytes),

                };

            
                

                await _studentRepository.AddStudentAsync(UserAdmin);
                await _emailService.SendEmailAsync(
               UserAdmin.Email,
           "Welcome to Our Library!",
           $"Hi {UserAdmin.FullName},\n\n" +
"We're thrilled to have you on board. Your registration was successful, and you now have full access to explore a world of books, reserve your favorites, and manage your reading journey with ease.\n\n" +
"Here’s what you can do next:\n" + "Search and explore our digital catalog\n" + "Add books to your wishlist\n" + "Track your borrowings and history\n\n" + "— The Library Team"+ 
           "You can now log in using your email and password to browse and borrow books.\n\n" +
           "Happy Reading!\nLibrary Team"
       );
                return "Student Registered Successfully";
            }

            catch (Exception ex)
            {
                return ex.InnerException?.Message ?? ex.Message;
            }
        }

        public async Task<string> BorrowBookRequestAsync(BorrowBookRequestDTO borrowBookRequestDTO)
        {
            try
            {

                var noofbooks = await _studentRepository.GetUserBooksAsync(borrowBookRequestDTO.UserId);
                var userid = borrowBookRequestDTO.UserId;
                var bookid = borrowBookRequestDTO.BookId;
                var borrowStatus = "Approved";
                var borrowStatus1 = "Pending";
                var returnStatus = "None";
                var returnStatus1 = "Pending";
                var isBookExisting = await _studentRepository.isBookExistingAsync(userid, bookid, borrowStatus, borrowStatus1, returnStatus,returnStatus1);
                //Console.WriteLine(noofbooks);
                if (noofbooks < 3 )
                {
                    if (!isBookExisting)
                    {
                        var BorrowBookRequest = new BookManagement
                        {

                            UserId = borrowBookRequestDTO.UserId,
                            BookId = borrowBookRequestDTO.BookId,
                            RequestType = "BorrowRequested",
                            BorrowRequestDate = DateOnly.FromDateTime(DateTime.Today),
                            BorrowStatus = "Pending",
                            ReturnStatus = "None"
                        };
                        await _studentRepository.BorrowBookRequestAsync(BorrowBookRequest);
                        return "Borrow Request Submitted Successfully";

                    }
                    else
                    {
                        return "You have already borrowed this book!";
                    }
                    
                }
                else
                {
                    return "You have issued maximum number of books. You cad add this book to your wishlist..";
                }
            }
            catch (Exception ex)
            {
                return "Something went wrong!";
            }

        }

        public async Task<List<BookingHistoryDTO>> GetBookingHistoryAsync(int UserId)
        {
            return await _studentRepository.GetBookingHistoryAsync(UserId);


        }

        public async Task<List<PendingRequestDTO>> GetPendingRequestAsync(int UserId)
        {
            return await _studentRepository.GetPendingRequestAsync(UserId);


        }

        public async Task<List<CurrentBookingDTO>> CurrentBookingAsync(int UserId)
        {
            return await _studentRepository.CurrentBookingAsync(UserId);


        }

        public async Task<string> ReturnBookRequestAsync(int TransactionId)
        {
            var transaction = await _librarianRepository.GetTransactionById(TransactionId);
           
      
            var today = DateOnly.FromDateTime(DateTime.Today);

            transaction.RequestType = "ReturnRequested";
            transaction.ReturnDate = today;
           transaction.ReturnStatus = "Pending";
           


            if (await _studentRepository.ReturnBookRequestAsync(transaction))
            {
                return "Return Request Submitted Successfully..";
            }

            return "Something went wrong!";
        }

        public async Task<string> AddtoWishlistAsync(int userId, int bookId)
        {
            try 
            {
                if (await _studentRepository.IsBookInWishlistAsync(userId, bookId))
                {
                    Console.WriteLine($"Checking wishlist for UserId: {userId}, BookId: {bookId}");

                    return "This book is already in your wishlist.";
                }
                var wishlist = new Studentwishlist
                {
                    UserId = userId,
                    BookId = bookId,
                    AddedOn = DateOnly.FromDateTime(DateTime.Today)

                };

                await _studentRepository.AddtoWishlistAsync(wishlist);
                return "Book successfully added to wishlist";
            }
            catch (Exception ex)
            {
                return ex.InnerException?.Message ?? ex.Message;
            }


        }

        public async Task<List<Studentwishlist>> GetUserWishlistsAsync(int userId)
        {
            try
            {
                var wishlists = await _studentRepository.GetUserWishlistsAsync(userId);

                return wishlists;
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching user wishlists", ex);
            }
        }

        public async Task<bool> RemoveFromWishlistAsync(int wishlistId)
        {
            return await _studentRepository.RemoveFromWishlistAsync(wishlistId);
        }

        public async Task<List<GetAllBooksDTO>> GetBestSellingBooksAsync()
        {
            try
            {
                var allBooks = await _studentRepository.GetBestSellingBooksAsync();

                if (allBooks == null)
                {
                    throw new Exception("Books Details Not Found!!");
                }
                return allBooks;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception("Something went wrong");
            }

        }

        public async Task<List<GetAllBooksDTO>> GetNewArrivalsAsync()
        {
            try
            {
                var allBooks = await _studentRepository.GetNewArrivalsAsync();

                if (allBooks == null)
                {
                    throw new Exception("Books Details Not Found!!");
                }
                return allBooks;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception("Something went wrong");
            }

        }


        public async Task<List<Book>> BookRecommendationsAsync(int userid)
        {
            try
            {
                return await _studentRepository.GetRecommendedBooksAsync(userid);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception("Something went wrong");
            }

        }

       


    }

}
