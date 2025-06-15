using System.Transactions;
using LibraryManagementSystem.DTOs.LibrarianDTOs;
using LibraryManagementSystem.DTOs.StudentDTOs;
using LibraryManagementSystem.Repositories.Interfaces;
using LibraryManagementSystem.Services.Interfaces;

namespace LibraryManagementSystem.Services.Implementations
{
    public class LibrarianService : ILibrarianService
    {
        private readonly ILibrarianRepository _librarianRepository;
        private readonly IAdminRepository _adminRepository;


        public LibrarianService(ILibrarianRepository librarianRepository, IAdminRepository adminRepository)
        {
           _librarianRepository = librarianRepository;
            _adminRepository = adminRepository;
        }

        public async Task<string> ApproveBorrowRequestAsync(int TransactionId)
        {
            var transaction = await _librarianRepository.GetTransactionById(TransactionId);
            var user = await _adminRepository.GetUserByIDAsync(transaction.UserId);
            var book = await _adminRepository.GetBookbyIdAsync(transaction.BookId);
            var today = DateOnly.FromDateTime(DateTime.Today);


            if (user.NoofBooks == 3)
            {
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
                return "Borrow Request Approved Successfully..";
            }

            return "Something went wrong!";
        }

        public async Task<string> RejectBorrowRequestAsync(int TransactionId)
        {
            var transaction = await _librarianRepository.GetTransactionById(TransactionId);
            //var user = await _adminRepository.GetUserByIDAsync(transaction.UserId);
            //var book = await _adminRepository.GetBookbyIdAsync(transaction.BookId);
            //var fine = await _librarianRepository.GetUserFineAsync(transaction.UserId);
            var today = DateOnly.FromDateTime(DateTime.Today);


            //if (user.NoofBooks == 3)
            //{
            //    return "Borrow Request cannot be approved because user has already borrowed maximum books";
            //}
            //transaction.RequestType = "Borrowed";
            //transaction.BorrowDate = today;
            //transaction.DueDate = today.AddDays(15);
            transaction.BorrowStatus = "Rejected";
            //user.NoofBooks = user.NoofBooks + 1;
            //book.AvailableCopies = book.AvailableCopies - 1;

            //await _adminRepository.UpdateBookAsync(book);
            //await _adminRepository.UpdateUserAsync(user);

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
                //transaction.BorrowDate = today;
                //transaction.DueDate = today.AddDays(15);
                transaction.ReturnStatus = "Approved";
                transaction.Fine = fine;
                user.NoofBooks = user.NoofBooks - 1;
                book.AvailableCopies = book.AvailableCopies + 1;

                await _adminRepository.UpdateBookAsync(book);
                await _adminRepository.UpdateUserAsync(user);

                if (await _librarianRepository.ApproveReturnRequestAsync(transaction))
                {
                    return "Return Request Approved Successfully..";
                }

               
            }
            return "Something went wrong!";

        }
    }
}
