using System;
using System.Security.Cryptography;
using System.Text;
using LibraryManagementSystem.DTOs.StudentDTOs;
using LibraryManagementSystem.DTOs.UserDTOs;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories.Implementations;
using LibraryManagementSystem.Repositories.Interfaces;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Services.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly ILibrarianRepository _librarianRepository;

        public StudentService(IStudentRepository studentRepository, IAdminRepository adminRepository, ILibrarianRepository librarianRepository)
        {
            _studentRepository = studentRepository;
            _adminRepository = adminRepository;
            _librarianRepository = librarianRepository;

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

                var noofbooks = await _studentRepository.GetUserBooks(borrowBookRequestDTO.UserId);
                var userid = borrowBookRequestDTO.UserId;
                var bookid = borrowBookRequestDTO.BookId;
                var borrowStatus = "Approved";
                var borrowStatus1 = "Pending";
                var returnStatus = "None";
                var returnStatus1 = "Pending";
                var isBookExisting = await _studentRepository.isBookExistingAsync(userid, bookid, borrowStatus, borrowStatus1, returnStatus,returnStatus1);
                //Console.WriteLine(noofbooks);
                if (noofbooks < 3 && !isBookExisting)
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
                    return "You already have borrowed this book or you have issued maximum number of books!!";
                }
            }
            catch (Exception ex)
            {
                return "Something went wrong!";
            }

        }

        public async Task<List<BookingHistoryDTO>> GetBookingHistory(int UserId)
        {
            return await _studentRepository.GetBookingHistory(UserId);


        }

        public async Task<List<PendingRequestDTO>> GetPendingRequest(int UserId)
        {
            return await _studentRepository.GetPendingRequest(UserId);


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


    }

}
