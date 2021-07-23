using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
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
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            try
            {
                // Map data convertion
                UserInput user = _mapper.Map<UserInput>(dto);
                // Create and put new user in database
                var newUser = await _uof.UserRepository.Create(user);

                // Log information about new created user
                _logger.LogInformation($"Post user: id: { user.Id }, FirstName: {user.FirstName}, LastName: {user.LastName}");

                // Map data convertion
                UserDTO userDTO = _mapper.Map<UserDTO>(newUser);

                return Created("success", userDTO);
            }
            catch (Exception ex)
            {
                // Log information about creating user error
                _logger.LogError(ex, $"ERROR: Post user: FirstName: {dto.FirstName}, LastName: {dto.LastName}");

                // Return error
                return NotFound(404);
            }

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            // Get the user from database according ID
            var user = await _uof.UserRepository.Get(dto.EmailAddress);

            // Check if user exists
            if (user == null) return BadRequest(new { message = "Invalid Credentials" });

            // Check the password
            if (user.Password != dto.Password)
            {
                return BadRequest(new { message = "Invalid Credentials" });
            }

            var jwt = _jwtService.Generate(user.Id);

            Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = true
            });

            return Ok(new
            {
                message = "success"
            });
        }
        [HttpPost("logout")]
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