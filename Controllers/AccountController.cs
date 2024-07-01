using AutoMapper;
using CRUDApi.Context;
using CRUDApi.DTOs;
using CRUDApi.Entities;
using CRUDApi.Interfaces;
using CRUDApi.Models;
using CRUDApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.WebRequestMethods;
namespace CRUDApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseApiController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationUserRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly LMSContext _context;
        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationUserRole> roleManager,
            ITokenService tokenService,
            IMapper mapper, LMSContext context)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _context = context;
        }
        private bool IsAdmin()
        {
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (emailClaim == null)
            {
                return false;
            }

            var email = emailClaim.Value;
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                return false;
            }

            var userRoles = _context.UserRoles.Where(ur => ur.UserId == user.UserId)
                                              .Select(ur => ur.RoleId)
                                              .ToList();

            var adminRole = _context.Roles.FirstOrDefault(r => r.Name == "Admin");
            if (adminRole == null)
            {
                return false;
            }

            return userRoles.Contains(adminRole.RoleId);
        }
        #region Register *
        /*[HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDTO model)
        {
            if (await IsUserExists(model.DisplayName)) return BadRequest("UserName is already Exist");
            var User = new ApplicationUser()
            { 
                DisplayName = model.DisplayName.ToLower(),
                Email = model.Email,//Ahmed.nasser@gmail.com
                UserName = model.Email.Split('@')[0],//Ahmed.nasser
                PhoneNumber = model.PhoneNumber,
                CurrentUserRole = model.RoleOfUser
            };

            var result = await _userManager.CreateAsync(User, model.Password);
            if (!result.Succeeded)
                return BadRequest("not Success");

            var userEntity = new CRUDApi.Models.User
            {
                UserId = User.Id,
                FullName = User.DisplayName,
                Email = User.Email,
                Password = model.Password, // Consider hashing this if it's in plain text
                Phone = User.PhoneNumber,
                CreatedAt = DateTime.UtcNow,
                Status = "Active",
                FacultyId = "FAC001",
                ImagePath = null,

            };

            _context.Users.Add(userEntity);

            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == model.RoleOfUser);
            if (role == null)
            {
                return BadRequest("Role does not exist");
            }

            var userRole = new UserRole
            {
                UserId = userEntity.UserId,
                RoleId = role.RoleId
            };
            _context.UserRoles.Add(userRole);

            await _context.SaveChangesAsync();

            
            var ReturnedUser = new UserDto()
            {
                DisplayName = User.DisplayName,
                Email = User.Email,
                Token = await _tokenService.CreateTokenAsync(User, _userManager),
                UserRole = model.RoleOfUser
            };

            return Ok(ReturnedUser);
        }*/
        #endregion

        #region Staff Register
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        [HttpPost("Staff Register")]

        public async Task<ActionResult<UserDto>> Register(RegisterDTO model)
        {
            if (!IsAdmin())
            {
                return Unauthorized();
            }
            if (await IsUserExists(model.Email))
                return BadRequest("UserName already exists");

            var applicationUser = new ApplicationUser()
            {
                DisplayName = model.DisplayName.ToLower(),
                Email = model.Email,
                UserName = model.Email.Split('@')[0],
                PhoneNumber = model.PhoneNumber,
                CurrentUserRole = model.RoleOfUser
            };

            var result = await _userManager.CreateAsync(applicationUser, model.Password);
            if (!result.Succeeded)
                return BadRequest("Registration failed");

            var userEntity = new CRUDApi.Models.User
            {
                UserId = applicationUser.Id,
                FullName = applicationUser.DisplayName,
                Email = applicationUser.Email,
                Password = applicationUser.PasswordHash, // Save the hashed password
                Phone = applicationUser.PhoneNumber,
                CreatedAt = DateTime.UtcNow,
                Status = "Active",
                FacultyId = "FAC001",
                ImagePath = "https://nabilramadan.runasp.net/WhatsApp%20Image%202024-06-30%20at%2004.15.42_10878c6a.jpg"
            };

            _context.Users.Add(userEntity);

            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == model.RoleOfUser);
            if (role == null)
            {
                return BadRequest("Role does not exist");
            }

            var userRole = new UserRole
            {
                UserId = userEntity.UserId,
                RoleId = role.RoleId
            };
            _context.UserRoles.Add(userRole);

            await _context.SaveChangesAsync();

            var returnedUser = new UserDto()
            {
                DisplayName = applicationUser.DisplayName,
                Email = applicationUser.Email,
                Token = await _tokenService.CreateTokenAsync(applicationUser, _userManager),
                UserRole = model.RoleOfUser
            };

            return Ok(returnedUser);
        }

        #endregion#region Register

        #region Student Register
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("Student Register")]
        public async Task<ActionResult<UserDto>> studentRegister(StudentRigesterDto model)
        {
            if (!IsAdmin())
            {
                return Unauthorized();
            }
            if (await IsUserExists(model.Email))
                return BadRequest("UserName already exists");

            var applicationUser = new ApplicationUser()
            {
                DisplayName = model.DisplayName.ToLower(),
                Email = model.Email,
                UserName = model.Email.Split('@')[0],
                PhoneNumber = model.PhoneNumber,
                CurrentUserRole = model.RoleOfUser,
                
            };

            var result = await _userManager.CreateAsync(applicationUser, model.Password);
            if (!result.Succeeded)
                return BadRequest("Registration failed");

            var userEntity = new CRUDApi.Models.User
            {
                UserId = applicationUser.Id,
                FullName = applicationUser.DisplayName,
                Email = applicationUser.Email,
                Password = applicationUser.PasswordHash,
                Phone = applicationUser.PhoneNumber,
                CreatedAt = DateTime.UtcNow,
                Status = "Active",
                FacultyId = "FAC001",
                ImagePath = "https://nabilramadan.runasp.net/WhatsApp%20Image%202024-06-30%20at%2004.15.42_10878c6a.jpg"
            };
            _context.Users.Add(userEntity);
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == model.RoleOfUser);
            if (role == null)
            {
                return BadRequest("Role does not exist");
            }

            var userRole = new UserRole
            {
                UserId = userEntity.UserId,
                RoleId = role.RoleId
            };
            _context.UserRoles.Add(userRole);

            var studentInf = new StudentInfo
            {
                UserId = applicationUser.Id,
                AcademicId = model.AcademicId,
                DepartmentId = model.DepartmentId,
                Level = model.Level,
            };
            await _context.StudentInfos.AddAsync(studentInf);

            await _context.SaveChangesAsync();

            var returnedUser = new UserDto()
            {
                DisplayName = applicationUser.DisplayName,
                Email = applicationUser.Email,
                Token = await _tokenService.CreateTokenAsync(applicationUser, _userManager),
                UserRole = model.RoleOfUser
            };

            return Ok(returnedUser);
        }
        #endregion



        #region Login

        [HttpPost("login")]//post: api/accounts/login
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
                return Unauthorized();
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded)
                return Unauthorized();

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager),
                UserRole = user.CurrentUserRole
            });
        } 
        #endregion

        #region Get Current User
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized();
            }

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                // User not found in the database, return 404 Not Found
                return NotFound("User not found");
            }

            var returnedUser = new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager),
                UserRole = user.CurrentUserRole,
            };

            return Ok(returnedUser);
        }

        #endregion

        #region LogOut
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("Logout")]
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Ok("User logged out successfully");
        }

        #endregion

        #region Update Password
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("UpdatePassword")]
        public async Task<ActionResult> UpdatePassword(UpdatePasswordDTO model)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized();
            }

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                // User not found in the database, return 404 Not Found
                return NotFound("User not found");
            }


            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("Password updated successfully");
        }
        #endregion

        #region update Photo
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("update Photo")]
        public async Task<IActionResult> UpdatePhoto([FromForm] IFormFile file)
        {
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }

            var email = emailClaim.Value;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            //var existingUser = _context.Users.FirstOrDefault(u => u.UserId == userId);

            if (file == null || file.Length == 0 || user == null)
            {
                return BadRequest("Missing Data !");
            }
            var userId = user.UserId;

            var ftpServer = "site1439.siteasp.net";
            var ftpPort = 21;
            var ftpLogin = "site1439";
            var ftpPassword = "p@5Y6gZ!Q_e7";
            var uploadPath = "/wwwroot/AccountPhoto";
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var ftpUri = new Uri($"ftp://{ftpServer}:{ftpPort}{uploadPath}/{fileName}");
            var ftpRequest = (FtpWebRequest)WebRequest.Create(ftpUri);
            ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
            ftpRequest.Credentials = new NetworkCredential(ftpLogin, ftpPassword);

            using (var fileStream = file.OpenReadStream())
            using (var ftpStream = ftpRequest.GetRequestStream())
            {
                fileStream.CopyTo(ftpStream);
            }
            var image = user.ImagePath.Split('/').LastOrDefault();
            await Console.Out.WriteLineAsync("++++++++image is :"+ image);
            if(user.ImagePath != null)
            {
                var deleteFtpUri = new Uri($"ftp://{ftpServer}:{ftpPort}{uploadPath}/{image}");

                var deleteFtpRequest = (FtpWebRequest)WebRequest.Create(deleteFtpUri);
                deleteFtpRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                deleteFtpRequest.Credentials = new NetworkCredential(ftpLogin, ftpPassword);

                using (var deleteResponse = (FtpWebResponse)deleteFtpRequest.GetResponse())
                {
                    if (deleteResponse.StatusCode != FtpStatusCode.FileActionOK)
                    {
                        return BadRequest($"Failed to delete existing file. Server response: {deleteResponse.StatusDescription}");
                    }
                }
            }
            user.ImagePath = $"https://nabilramadan.runasp.net/AccountPhoto/{fileName}";
            _context.Users.Update(user);
            _context.SaveChanges();
            return Ok("Updated Succefuly");


        }

        #endregion



        private async Task<bool> IsUserExists(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.DisplayName == username.ToLower());
        }


    }
}