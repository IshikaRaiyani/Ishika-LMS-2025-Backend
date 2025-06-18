using System.Transactions;
using LibraryManagementSystem.DTOs.LibrarianDTOs;
using LibraryManagementSystem.DTOs.StudentDTOs;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories.Interfaces;
using LibraryManagementSystem.Services.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace LibraryManagementSystem.Services.Implementations
{
    public class LibrarianService : ILibrarianService
    {
        private readonly ILibrarianRepository _librarianRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IEmailService _emailService;
        private readonly INotificationService _notificationService;


        public LibrarianService(ILibrarianRepository librarianRepository, IAdminRepository adminRepository, INotificationRepository notificationRepository, IEmailService emailService, INotificationService notificationService)
        {
           _librarianRepository = librarianRepository;
            _adminRepository = adminRepository;
            _notificationRepository = notificationRepository;
            _emailService = emailService;
            _notificationService = notificationService;
        }

        public async Task<string> ApproveBorrowRequestAsync(int TransactionId)
        {
            var transaction = await _librarianRepository.GetTransactionById(TransactionId);
            if (transaction == null)
                return "Transaction not found";

            var user = await _adminRepository.GetUserByIDAsync(transaction.UserId);
            if (user == null)
                return "User not found";
           
            var book = await _adminRepository.GetBookbyIdAsync(transaction.BookId);
            if (book == null)
                return "Book not found";

            var today = DateOnly.FromDateTime(DateTime.Today);

            
            if (user.NoofBooks == 3)
            {
                Console.WriteLine(user.NoofBooks);
                return "Borrow Request cannot be approved because user has already borrowed maximum books";
            }
            transaction.RequestType = "Borrowed";
            transaction.BorrowDate = today;
            transaction.DueDate = today.AddDays(15);
            transaction.BorrowStatus = "Approved";
            user.NoofBooks = user.NoofBooks + 1;
            book.AvailableCopies = book.AvailableCopies - 1;

            await _adminRepository.UpdateBookAsync(book);
            await _adminRepository.UpdateUserAsync(user);

            if (await _librarianRepository.ApproveBorrowRequestAsync(transaction))
            {
                var notification = new StudentNotification
                {
                    UserId = user.UserId,
                    Message = $"Your borrow request for the book {book.Title} has been approved. Please collect it from the library.",
                    CreatedAt = DateTime.Now,
                    IsRead = false
                };

                await _notificationRepository.AddNotificationAsync(notification);
                await _emailService.SendEmailAsync(
              user.Email,
              "Borrow Request Approved",
               $"Your request for the book '{book.Title}' has been approved. Please collect it from the library."
);

               
                return "Borrow Request Approved Successfully..";
            }

            return "Something went wrong!";
        }

        public async Task<string> RejectBorrowRequestAsync(int TransactionId)
        {
            var transaction = await _librarianRepository.GetTransactionById(TransactionId);
            var today = DateOnly.FromDateTime(DateTime.Today);

            transaction.BorrowStatus = "Rejected";
            transaction.ReturnStatus = "Rejected";
        
            if (await _librarianRepository.RejectBorrowRequestAsync(transaction))
            {
                return "Borrow Request Rejected Successfully..";
            }

            return "Something went wrong!";
        }
        public async Task<List<GetPendingBorrowRequestsDTO>> GetPendingBorrowRequestsAsync()
        {
            try
            {
                var pendingrequest = await _librarianRepository.GetPendingBorrowRequestsAsync();

                if (pendingrequest == null)
                {
                    throw new Exception("No pending borrow request found!!");
                }

                return pendingrequest;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception("Something went wrong");
            }
        }

        public async Task<List<GetPendingReturnRequestDTO>> GetPendingReturnRequestsAsync()
        {
            try
            {
                var today = DateOnly.FromDateTime(DateTime.Today);
                var pendingrequest = await _librarianRepository.GetPendingReturnRequestsAsync();
                var result = new List<GetPendingReturnRequestDTO>();
                var fine = 0;
                foreach (var request in pendingrequest)
                {
                   if(request.DueDate < request.ReturnDate)
                    {
                        TimeSpan returndate = request.ReturnDate.Value.ToDateTime(TimeOnly.MinValue) - request.DueDate.Value.ToDateTime(TimeOnly.MinValue);
                        int diff = returndate.Days;
                        fine = diff * 20;
                    }
                    else
                    {
                        fine = 0;
                    }
                    var pendingReturn = new GetPendingReturnRequestDTO
                    {
                        TransactionId = request.TransactionId,
                       UserId = request.UserId,
                        BookId = request.BookId,
                        FullName = request.User.FullName,
                        Title = request.Book.Title,
                        BorrowDate = (DateOnly)request.BorrowDate,
                        DueDate = (DateOnly)request.DueDate,
                       
                        ApplicableFine = fine

                    };
                    result.Add(pendingReturn);
                }

                if (pendingrequest == null)
                {
                    throw new Exception("No pending return request found!!");
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception("Something went wrong");
            }
        }
        public async Task<string> ApproveReturnRequestAsync(int TransactionId)
        {
            var transaction = await _librarianRepository.GetTransactionById(TransactionId);
            var user = await _adminRepository.GetUserByIDAsync(transaction.UserId);
            var book = await _adminRepository.GetBookbyIdAsync(transaction.BookId);
            //var fine = await _librarianRepository.GetUserFineAsync(transaction.UserId);
            
            var today = DateOnly.FromDateTime(DateTime.Today);
            TimeSpan returndate = transaction.ReturnDate.Value.ToDateTime(TimeOnly.MinValue) - transaction.DueDate.Value.ToDateTime(TimeOnly.MinValue);
            int diff = returndate.Days;
            var fine = 0;
            if(diff > 0)
            {
                fine = diff * 20;
            }
            

            if (transaction.ReturnStatus == "Pending")
            {
                transaction.RequestType = "Returned";
               
                transaction.ReturnStatus = "Approved";
                transaction.Fine = fine;
                user.NoofBooks = user.NoofBooks - 1;
                int previousCopies = book.AvailableCopies;
                book.AvailableCopies = book.AvailableCopies + 1;

                await _adminRepository.UpdateBookAsync(book);
                await _adminRepository.UpdateUserAsync(user);

                if (await _librarianRepository.ApproveReturnRequestAsync(transaction))
                {
                    var notification = new StudentNotification
                    {
                        UserId = user.UserId,
                        Message = $"Your return request for the book {book.Title} has been approved with total applicable fine of Rs.'{transaction.Fine}'. Please submit it to the library.",
                        CreatedAt = DateTime.Now,
                        IsRead = false
                    };
                    await _notificationRepository.AddNotificationAsync(notification);

                    await _emailService.SendEmailAsync(
                  user.Email,
                  "Return Request Approved",
                   $"Your return request for the book '{book.Title}' has been approved with total applicable fine of Rs. '{transaction.Fine}'. Please submit it to the library.");
                   
                }

                if(previousCopies == 0 && book.AvailableCopies > 0)
                {
                    await _notificationService.NotifyWishlistUsersIfBookBecomesAvailable(book.BookId);

                 
                    

                }

                return "Return Request Approved Successfully..";


            }
            return "Something went wrong!";

        }
    }
}
