using LibraryManagementSystem.Data;
using LibraryManagementSystem.DTOs.AccessTokenDTOs;
using LibraryManagementSystem.DTOs.ResetPasswordDTOs;
using LibraryManagementSystem.DTOs.UserAuthenticationDTOs;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LibraryManagementSystem.Services.Implementations
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly DataContext _context;
        private readonly JwtService _jwtService;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;


        public UserAuthenticationService(DataContext context, JwtService jwtService, IConfiguration configuration, IEmailService emailService)
        {
            _context = context;
            _jwtService = jwtService;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task<AccessTokenResponseDTO?> LogInUserAsync(UserLoginDTO userDto)
        {
            try
                {
                if (userDto == null)
                {
                    Console.WriteLine("Please Enter the valid User object!");
                    return null;
                }




                var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == userDto.Email);

                if (user == null)
                {
                    Console.WriteLine("Email Mismatch. Please enter a valid email address!");
                    return null;
                }

                if(user.RoleName=="Student" && user.Status=="Blocked")
                {
                    throw new Exception("Your account is blocked. Please contact your admin!");
                }

                var sha256 = SHA256.Create();
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(userDto.Password));
                Console.WriteLine("\n\n\n\n\n" + Convert.ToBase64String(hashBytes));
                if (user.Password != Convert.ToBase64String(hashBytes))
                {
                    Console.WriteLine("Please check the Password. Invalid Password!");
                    return null;
                }

                var tokens = await _jwtService.GenerateAccessToken(user);

                string message = user.RoleName.ToLower() switch
                {
                    "admin" => "Admin login successfully",
                    "student" => "Student login successfully",
                    "librarian" => "Librarian login successfully",

                };

                return new AccessTokenResponseDTO
                {
                    AccessToken = tokens,
                    Message = message,
                    FullName = user.FullName,
                    Email = user.Email,
                    RoleName = user.RoleName,
                    UserId = user.UserId


                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task<string> ResetPasswordRequestAsync(string userEmail)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userEmail))
                {
                    throw new Exception("Enter Email to send request for reset password");
                }

               
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == userEmail);
                if (user == null)
                {
                    throw new Exception("User email not found");
                }

                //generating JWt reset token
                    var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("IshikaSRaiyaniIshikaSRaiyaniIshikaSRaiyaniIshikaSRaiyaniIshikaSRaiyaniIshikaSRaiyani");
                //var expiryTime = DateTime.UtcNow.AddMinutes(1);

                //var claims = new List<Claim>
                //{
                //  new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                //  new Claim(ClaimTypes.Email, user.Email),
                //  new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(expiryTime).ToUnixTimeSeconds().ToString())
                //};

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] { new Claim("nameid", user.UserId.ToString()) }),
                    Expires = DateTime.UtcNow.AddMinutes(15),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);  
                var tokenstring = tokenHandler.WriteToken(token);

                var resetLink = $"http://localhost:4200/auth/reset-password?token={tokenstring}";


                // Email content
                var subject = "Reset Your Password";
                var message = $@"
        <h3>Hello,</h3>
        <p>Please click the link below to reset your password:</p>
        <a href='{resetLink}'>{resetLink}</a>
        <p>This link is valid for 15 minutes.</p>";

                await _emailService.SendEmailAsync(userEmail, "Password Reset Request", $"Click the link to reset your password: <a href='{resetLink}'>Reset Password</a>");

                return "Reset link sent successfully.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ResetPasswordRequestAsync: {ex.Message}");
                return "An error occurred while processing the password reset request.";
            }
        }

        public async Task<string> ResetPasswordAsync(ResetPasswordDTO resetPasswordDto)
        {
            try
            {
                if (resetPasswordDto == null)
                {
                    throw new Exception("ENTER VALID RESET PASSWORD OBJECT");
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("IshikaSRaiyaniIshikaSRaiyaniIshikaSRaiyaniIshikaSRaiyaniIshikaSRaiyaniIshikaSRaiyani");

               
                    var tokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero // No time delay for token expiration
                    };

                    var principal = tokenHandler.ValidateToken(resetPasswordDto.Token, tokenValidationParameters, out SecurityToken validatedToken);
                    var jwtToken = (JwtSecurityToken)validatedToken;
                   var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid");


                if (userIdClaim == null)
                {
                        throw new Exception("Invalid reset password token");
                }

                    int userId = int.Parse(userIdClaim.Value);

                    var user = await _context.Users.SingleOrDefaultAsync(u => u.UserId == userId);
                    if (user == null)
                    {
                        throw new Exception("User Not Found");
                    }

                    
                    if (resetPasswordDto.NewPassword != resetPasswordDto.ConfirmPassword)
                    {
                        throw new Exception("Confirm Password Does not Match with New Password");
                    }

                   
                    using var sha256 = SHA256.Create();
                    byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(resetPasswordDto.NewPassword));
                    string hashedNewPassword = Convert.ToBase64String(hashBytes);

                   
                    if (user.Password == hashedNewPassword)
                    {
                        throw new Exception("New Passsword Cannot be Same as Previous Password");
                    }

                    
                    user.Password = hashedNewPassword;
                    await _context.SaveChangesAsync();

                    return "PASSWORD RESET SUCCESSFULLY";
                
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "Invalid Token";
            }
        }






    }
}   
