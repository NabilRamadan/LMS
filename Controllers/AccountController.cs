using AutoMapper;
using CRUDApi.DTOs;
using CRUDApi.Entities;
using CRUDApi.Interfaces;
using CRUDApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
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
        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationUserRole> roleManager,
            ITokenService tokenService,
            IMapper mapper)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
        }
        [HttpPost("Register")]
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
                return BadRequest();
            var ReturnedUser = new UserDto()
            {
                DisplayName = User.DisplayName,
                Email = User.Email,
                Token = await _tokenService.CreateTokenAsync(User, _userManager),
                UserRole = model.RoleOfUser
            };

            return Ok(ReturnedUser);
        }



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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("Logout")]
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Ok("User logged out successfully");
        }


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



        private async Task<bool> IsUserExists(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.DisplayName == username.ToLower());
        }


    }
}