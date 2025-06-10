using LibraryManagementSystem.Models;
using System.Text;
using LibraryManagementSystem.Repositories.Interfaces;
using LibraryManagementSystem.DTOs.AdminDTOs;
using System.Security.Cryptography;
using LibraryManagementSystem.Services.Interfaces;
using LibraryManagementSystem.DTOs.LibrarianDTOs;

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

                if (userRole.Equals("Librarian")){
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


    }
}
