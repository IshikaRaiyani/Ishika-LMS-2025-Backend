using LibraryManagementSystem.Models;
using System.Text;
using LibraryManagementSystem.Repositories.Interfaces;
using LibraryManagementSystem.DTOs.AdminDTOs;
using System.Security.Cryptography;
using LibraryManagementSystem.Services.Interfaces;
using LibraryManagementSystem.DTOs.LibrarianDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Features;
using LibraryManagementSystem.DTOs.BookDTOs;
using static System.Reflection.Metadata.BlobBuilder;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<string> AddAdminAsync(AddAdminDTO addAdminDto)
        {
            try
            {
                if (addAdminDto == null)
                {
                    throw new Exception("Please enter the valid User Admin Details.");


                }

                if (await _adminRepository.IsEmailExistsAsync(addAdminDto.Email))
                {
                    return "This Email Address already exists. Please enter a valid Email Address.";
                }

                var sha256 = SHA256.Create();
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(addAdminDto.Password));
                var UserAdmin = new User
                {
                    FullName = addAdminDto.FullName,
                    Email = addAdminDto.Email,
                    Password = Convert.ToBase64String(hashBytes),
                    RoleName = "Admin"
                };


                await _adminRepository.AddAdminAsync(UserAdmin);
                return "Admin Added Successfully";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }



        public async Task<string> AddLibrarianAsync(AddLibrarianDTO addLibrarianDto)
        {
            try
            {
                if (addLibrarianDto == null)
                {
                    throw new Exception("Please enter the valid User Admin Details.");


                }

                if (await _adminRepository.IsEmailExistsAsync(addLibrarianDto.Email))
                {
                    return "This Email Address already exists. Please enter a valid Email Address.";
                }

                var sha256 = SHA256.Create();
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(addLibrarianDto.Password));
                var UserLibrarian = new User
                {
                    FullName = addLibrarianDto.FullName,
                    Email = addLibrarianDto.Email,
                    Password = Convert.ToBase64String(hashBytes),
                    RoleName = "Librarian"
                };


                await _adminRepository.AddLibrarianAsync(UserLibrarian);
                return "Librarian Added Successfully";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }


        public async Task<string> DeleteLibrarianAsync(int userId)
        {
            try

            {
                var user = await _adminRepository.GetUserByIDAsync(userId);
                if (user == null)
                {
                    throw new Exception("User Id does not exist..");


                }


                var userRole = await _adminRepository.GetUserRoleAsync(userId);

                if (userRole.Equals("Librarian"))
                {
                    var users = await _adminRepository.DeleteLibrarianAsync(user);

                    return "Librarian Deleted Successfully";
                }
                else
                {
                    return "Provided UserId is not a Librarian";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public async Task<string> BlockStudentAsync(UpdateStudentStatusDTO updateStudentStatusDto)
        {
            try
            {
                if (updateStudentStatusDto == null)
                {
                    throw new Exception("UserId is required.");
                }
                var user = await _adminRepository.GetUserByIDAsync(updateStudentStatusDto.UserId);

                if (user == null)
                {
                    throw new Exception("Student Not Found. Please enter the valid UserId..:");
                }

                if (user.RoleName.Equals("Student"))
                {
                    if (user.Status.Equals("Blocked"))
                    {


                        return "Student status is already blocked!";
                    }
                    else
                    {
                        user.Status = "Blocked";
                        await _adminRepository.StudentStatusUpdateAsync(user);
                        return "Student Blocked Successfully!";
                    }
                }
                else
                {
                    return "Provided UserId is not a Student";
                }
            }
            catch (Exception ex)    
            {
                Console.WriteLine(ex);
                return ex.Message;
            }
        }

        public async Task<string> UnblockStudentAsync(UpdateStudentStatusDTO updateStudentStatusDto)
        {
            try
            {
                if (updateStudentStatusDto == null)
                {
                    throw new Exception("UserId is required.");
                }
                var user = await _adminRepository.GetUserByIDAsync(updateStudentStatusDto.UserId);

                if (user == null)
                {
                    throw new Exception("Student Not Found. Please enter the valid UserId..:");
                }

                if (user.RoleName.Equals("Student"))
                {
                    if (user.Status.Equals("Active"))
                    {


                        return "Student status is already Unblocked!";
                    }
                    else
                    {
                        user.Status = "Active";
                        await _adminRepository.StudentStatusUpdateAsync(user);
                        return "Student Unblocked Successfully!";
                    }
                }
                else
                {
                    return "Provided UserId is not a Student";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return ex.Message;
            }
        }

        public async Task<string> GetActiveUsersAsync()
        {
            try
            {

                var activeUsers = await _adminRepository.GetActiveUsersAsync();

                if (activeUsers == null || !activeUsers.Any())
                {
                    throw new Exception("Active User Not Found!");
                }
                return activeUsers;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return "Something went wrong";
            }
        }

        public async Task<string> GetBlockedUsersAsync()
        {
            try
            {

                var blockedUsers = await _adminRepository.GetBlockedUsersAsync();

                if (blockedUsers == null || !blockedUsers.Any())
                {
                    throw new Exception("Blocked Users Not Found!");
                }
                return blockedUsers;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return "Something went wrong";
            }
        }

        public async Task<string> GetUserCountAsync()
        {
            try
            {

                var userCount = await _adminRepository.GetUserCountAsync();

                if (userCount == null || !userCount.Any())
                {
                    throw new Exception("No User Found!");
                }
                return userCount;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return "Something went wrong";
            }
        }

        public async Task<List<AllUsersDTO>> GetAllUsersAsync()
        {
            try
            {
                var allUsers = await _adminRepository.GetAllUsersAsync();

                if (allUsers == null)
                {
                    throw new Exception("Users Details Not Found!!");
                }


                //var allUsersDto = allUsers.Select(user => new AllUsersDTO
                //{
                //    UserId = user.UserId,
                //    FullName = user.FullName,
                //    Email = user.Email,
                //    RoleName = user.RoleName,
                //    Status = user.Status,
                //    NoofBooks = user.NoofBooks
                //}).ToList();

                return allUsers;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception("Something went wrong");
            }
        }

        public async Task<string> AddBookAsync(Book book)
        {
            try
            {
                if (book == null)
                {
                    throw new Exception("Please enter the valid Book Details.");


                }


                var addBook = new Book
                {
                    Title = book.Title,
                    Author = book.Author,
                    Genre = book.Genre,
                    TotalCopies = book.TotalCopies,
                    AvailableCopies = book.AvailableCopies,
                    Description = book.Description,
                    ImageUrl = book.ImageUrl,
                    IsBestSelling = book.IsBestSelling,
                    AddedOn = DateOnly.FromDateTime(DateTime.Today),
                   

                };


                await _adminRepository.AddBookAsync(addBook);
                return "Book Added Successfully";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public async Task<string> GetTotalBookCountAsync()
        {
            try
            {

                var bookCount = await _adminRepository.GetTotalBookCountAsync();

                if (bookCount == null || !bookCount.Any())
                {
                    throw new Exception("No Book Found!");
                }
                return bookCount;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return "Something went wrong";
            }
        }

        public async Task<string> GetTotalAvailableBooksAsync()
        {
            try
            {

                var avaiable_books_count = await _adminRepository.GetTotalAvailableBooksAsync();

                if (avaiable_books_count == null || !avaiable_books_count.Any())
                {
                    throw new Exception("No Book Found!");
                }
                return avaiable_books_count;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return "Something went wrong";
            }
        }

        public async Task<GetBookByIdDTO> GetBookByIdAsync(int Bookid)
        {
            var book = await _adminRepository.GetBookByIDAsync(Bookid);
            if (book == null) return null;

            // manual mapping (or use AutoMapper)
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


        public async Task<List<GetAllBooksDTO>> GetAllBooksAsync()
        {
            try
            {
                var allBooks = await _adminRepository.GetAllBooksAsync();

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


        public async Task<string> DeleteBookAsync(int bookId)
        {
            try

            {
                var book = await _adminRepository.GetBookByIDAsync(bookId);
                if (book == null)
                {
                    throw new Exception("Book Id does not exist..");



                }
                await _adminRepository.DeleteBookAsync(book);

                return "Book Deleted Successfully";

            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public async Task<bool> UpdateBookAsync(UpdateBookDTO dto)
        {
            var book = await _adminRepository.GetBookbyIdAsync(dto.BookId);
            if (book == null) return false;

            book.Title = dto.Title;
            book.Author = dto.Author;
            book.Genre = dto.Genre;
            book.TotalCopies = dto.TotalCopies;
            book.AvailableCopies = dto.AvailableCopies;
            book.Description = dto.Description;
            book.IsBestSelling = dto.IsBestSelling;
            

            return await _adminRepository.UpdateBookAsync(book);
        }








    }
}
