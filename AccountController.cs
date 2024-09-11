using JoRide.IServices;
using JoRide.Model;
using JORide.Model.DTO.Register;
using JORide.Model.DTO.Register_and_Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JoRide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        // using to manage user [ updating , deleting , add ]  
        private readonly UserManager<ApplicationUser> _userManager;

        // using to deleting or creating the roles
        private readonly RoleManager<IdentityRole> _roleManager;

        // using to sign-in or sign-out the user 
        private readonly SignInManager<ApplicationUser> _signInManager;

        // create JWT token
        private readonly IJWTService _jwtService;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            IJWTService jwtService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
        }

        // Customer Register 
        [HttpPost("/register/customer")]
        [AllowAnonymous]
        public async Task<IActionResult> PostCustomerRegister(RegisterPersonDTO registerDto)
        {
            // Validation Model State
            if (!ModelState.IsValid)
            {
                string ErrorMessage =
                    string.Join(" | ", ModelState.Values.SelectMany(error => error.Errors)
                                                        .Select(error => error.ErrorMessage));
                return Problem(ErrorMessage);
            }

            // Validation user Registerd
            ApplicationUser? user = await _userManager.FindByEmailAsync(registerDto.Email);
            if (user != null)
            {
                return Problem("The user is already registerd");
            }

            // Create User 
            ApplicationUser applicationUser = new()
            {
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                PhoneNumber = registerDto.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(applicationUser, registerDto.Password);
            if (result.Succeeded)
            {
                // sign-in user
                await _userManager.AddToRoleAsync(applicationUser, "Customer");
                await _signInManager.SignInAsync(applicationUser, isPersistent: false);

                var authenticationResponse = _jwtService.CreateJwtToken(applicationUser);

                return Ok(authenticationResponse);
            }
            else
            {
                // problem1 | problem2 ... etc
                string errorMessage = string.Join(" | ", result.Errors.Select(error => error.Description));
                return Problem(errorMessage);
            }
        }

        // Agency Register
        [HttpPost("/register/agency")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PostAgencyRegister(RegisterAgencyDTO registerDto)
        {
            // Validation Model State
            if (!ModelState.IsValid)
            {
                string ErrorMessage =
                    string.Join(" | ", ModelState.Values.SelectMany(error => error.Errors)
                                                        .Select(error => error.ErrorMessage));
                return Problem(ErrorMessage);
            }

            // Validation user Registerd
            ApplicationUser? user = await _userManager.FindByEmailAsync(registerDto.Email);
            if (user != null)
            {
                return Problem("The user is already registerd");
            }

            // Create User 
            ApplicationUser applicationUser = new()
            {
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                AgencyName = registerDto.AgencyName,
                PhoneNumber = registerDto.PhoneNumber,
                IsAgency = true
            };

            var result = await _userManager.CreateAsync(applicationUser, registerDto.Password);
            await _userManager.AddToRoleAsync(applicationUser, "Agency");
            if (!result.Succeeded)
            {
                // sign-in user
                await _signInManager.SignInAsync(applicationUser, isPersistent: false);
                return Ok();
            }
            else
            {
                // problem1 | problem2 ... etc
                string errorMessage = string.Join(" | ", result.Errors.Select(error => error.Description));
                return Problem(errorMessage);
            }
        }

        // Employee Register
        [HttpPost("register/employee")]
        [Authorize(Roles = "AGENCY")]
        public async Task<IActionResult> PostEmployeeRegister(RegisterPersonDTO registerDto)
        {
            // Validation Model State
            if (!ModelState.IsValid)
            {
                string ErrorMessage =
                    string.Join(" | ", ModelState.Values.SelectMany(error => error.Errors)
                                                        .Select(error => error.ErrorMessage));
                return Problem(ErrorMessage);
            }

            // Get the current logged-in agency
            ApplicationUser? currentAgencyUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (currentAgencyUser.AgencyId == null)
            {
                return Unauthorized();
            }


            // Validation user Registerd
            ApplicationUser? user = await _userManager.FindByEmailAsync(registerDto.Email);
            if (user == null)
            {
                return Problem("The user is already registerd");
            }

            // Create User 
            ApplicationUser applicationUser = new()
            {
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                PhoneNumber = registerDto.PhoneNumber,
                AgencyId = currentAgencyUser.ToString()
            };

            var result = await _userManager.CreateAsync(applicationUser, registerDto.Password);
            await _userManager.AddToRoleAsync(applicationUser, "Employee");
            if (result.Succeeded)
            {
                // sign-in user
                await _signInManager.SignInAsync(applicationUser, isPersistent: false);
                return Ok();
            }
            else
            {
                // problem1 | problem2 ... etc
                string errorMessage = string.Join(" | ", result.Errors.Select(error => error.Description));
                return Problem(errorMessage);
            }
        }


        // User Log-In
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> PostLogin(LoginDTO loginDto)
        {
            if (!ModelState.IsValid)
            {
                string errorMessage = string.Join(" | "
                         , ModelState.Values.SelectMany(error => error.Errors)
                            .Select(error => error.ErrorMessage));
                return Problem(errorMessage);
            }
            ApplicationUser? user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return NotFound("User does not exist.");
            }

            // log-in 
            await _signInManager.SignInAsync(user, isPersistent: false);
            var authenticationResponse = _jwtService.CreateJwtToken(user);
            /*var token = _jwtService.CreateJwtToken(user);*/

            return Ok(authenticationResponse);
        }
        [HttpGet]
        public async Task<IActionResult> GetLogin()
        {
            await _signInManager.SignOutAsync();
            return NoContent();
        }
    }
}
