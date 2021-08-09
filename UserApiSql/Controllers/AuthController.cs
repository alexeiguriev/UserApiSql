using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using HelperCSharp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserApiSql.Helpers;
using UserApiSql.Interfaces;
using UserApiSql.Models;
using UserApiSql.ModelsDTO;

namespace UserApiSql.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uof;
        private readonly ILogger _logger;
        private readonly JwtService _jwtService;

        public AuthController(IUnitOfWork uof, IMapper mapper, ILogger<AuthController> logger, JwtService jwtService)
        {
            _uof = uof;
            _mapper = mapper;
            _logger = logger;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserInput userInput)
        {
            try
            {
                userInput.Password = Crypt.DecodeFrom64(userInput.Password);

                // Map data convertion
                UserInput userNew = _mapper.Map<UserInput>(userInput);
                // Create and put new user in database
                var newUser = await _uof.UserRepository.Create(userNew);

                // Log information about new created user
                _logger.LogInformation($"Post user: id: { userNew.Id }, FirstName: {userNew.FirstName}, LastName: {userNew.LastName}");

                // Map data convertion
                UserDTO userDTO = _mapper.Map<UserDTO>(newUser);

                return Created("success", userDTO);
            }
            catch (Exception ex)
            {
                // Log information about creating user error
                _logger.LogError(ex, $"ERROR: Post user: FirstName: {userInput.FirstName}, LastName: {userInput.LastName}");

                // Return error
                return NotFound(404);
            }

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserInput userInput)
        {
            userInput.Password = Crypt.DecodeFrom64(userInput.Password);
            // Get the user from database according ID
            var userNew = await _uof.UserRepository.Get(userInput.EmailAddress);

            // Map data convertion
            UserDTO userDTO = _mapper.Map<UserDTO>(userNew);

            // Check if user exists
            if (userNew == null) return BadRequest(new { message = "Invalid Credentials" });

            // Check the password
            if (userNew.Password != userInput.Password)
            {
                return BadRequest(new { message = "Invalid Credentials" });
            }

            var jwt = _jwtService.Generate(userNew);

            Response.Cookies.Append("JwtBearer", jwt, new CookieOptions
            {
                HttpOnly = true
            });
            var output = new
            {
                Access_Token = jwt,
                user = userDTO
            };

            return Ok(output);
        }
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");

            return Ok(new
            {
                message = "success"
            });
        }
    }
}