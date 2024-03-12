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
                // Check if model state is valid.
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Create a new user object
                var user = new User
                {
                    UserName = registerDto.Username,
                    Email = registerDto.EmailAddress
                };

                // Create a new User with its password.
                var createUser = await _userManager.CreateAsync(user, registerDto.Password);

                if(createUser.Succeeded)
                {
                    // Give the role to the newly created user.
                    var role = await _userManager.AddToRoleAsync(user, "User");

                    if(role.Succeeded)
                    {
                        // Return the new user and its JWT token.
                        return Ok(
                            new NewUserDto
                            {
                                Username = user.UserName,
                                EmailAddress = user.Email,
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
            // Check if model state is valid.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Find the User that requests a login by its username.
            User? user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username);

            if(user == null)
            {
                return Unauthorized("Unknown user.");
            }

            //Check the password of the user trying to login.
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
            {
                return Unauthorized("Username and password don't match.");
            }

            // Return the logged user and its JWT token.
            return Ok(new NewUserDto
            {
                Username = user.UserName,
                EmailAddress = user.Email,
                Token = _tokenService.CreateToken(user)
            });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            // Log out the current logged in user.
            await _signInManager.SignOutAsync();

            return Ok("User signed out");
        }
    }
}
