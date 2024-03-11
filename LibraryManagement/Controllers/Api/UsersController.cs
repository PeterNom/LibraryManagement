using LibraryManagement.Dtos.User;
using LibraryManagement.Interfaces;
using LibraryManagement.Models;
using LibraryManagement.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace LibraryManagement.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;

        public UsersController(UserManager<User> userManager, ITokenService tokenService, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = new User
                {
                    UserName = registerDto.Username,
                    Email = registerDto.EmailAddress
                };

                var createUser = await _userManager.CreateAsync(user, registerDto.Password);

                if(createUser.Succeeded)
                {
                    var role = await _userManager.AddToRoleAsync(user, "User");

                    if(role.Succeeded)
                    {
                        return Ok(
                            new NewUserDto
                            {
                                Username = user.UserName,
                                Email = user.Email,
                                Token = _tokenService.CreateToken(user)
                            });
                    }
                    else
                    {
                        return StatusCode(500, role.Errors.ToString());
                    }
                }
                else
                {
                    return StatusCode(500, createUser.Errors.ToString());
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username);

            if(user == null)
            {
                return Unauthorized("Unknown user.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
            {
                return Unauthorized("Username and password don't match.");
            }

            return Ok(new NewUserDto
            {
                Username = user.UserName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            });
        }

    }
}
