using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserApiSql.ModelsDTO;
using UserApiSql.Data;
using UserApiSql.Interfaces;
using UserApiSql.Models;
using Microsoft.AspNetCore.Authorization;

namespace UserApi.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uof;
        private readonly ILogger _logger;

        public UserController(IUnitOfWork uof, IMapper mapper, ILogger<UserController> logger)
        {
            _uof = uof;
            _mapper = mapper;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                string email = HttpContext.User.Identity.Name;
                // Get from Database all the users
                var users = await _uof.UserRepository.Get();

                // Map data convertion
                IEnumerable<UserDTO> userDTO = _mapper.Map<IEnumerable<UserDTO>>(users);

                // Log information about get oaa users
                _logger.LogInformation("Get all users");

                // Return all the users
                return Ok(userDTO);
            }
            catch(Exception ex)
            {
                // Return information about get users error
                _logger.LogError(ex, "Error: Get all users error");

                // Return not found
                return NotFound(404);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsers(int id)
        {
            try
            {
                // Get the user from database according ID
                var user = await _uof.UserRepository.Get(id);

                // Map data convertion
                UserDTO userDTO = _mapper.Map<UserDTO>(user);

                // Log data about requested user.
                _logger.LogInformation($"Get user: id:  FirstName: {userDTO.FirstName}, LastName: {userDTO.LastName}");

                // Return user data
                return Ok(userDTO);
            }
            catch (Exception ex)
            {
                // Log information about get user error
                _logger.LogError(ex, $"ERROR: Get user: id: { id }");

                // Return not found
                return NotFound(404);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostUsers([FromBody] UserInput user)
        {
            try
            {
                // Create and put new user in database
                var newUser = await _uof.UserRepository.Create(user);

                // Log information about new created user
                _logger.LogInformation($"Post user: id: { user.Id }, FirstName: {user.FirstName}, LastName: {user.LastName}");

                // Map data convertion
                UserDTO userDTO = _mapper.Map<UserDTO>(newUser);

                // Return new created user
                return Ok(userDTO);
            }
            catch (Exception ex)
            {
                // Log information about creating user error
                _logger.LogError(ex, $"ERROR: Post user: id: { user.Id }, FirstName: {user.FirstName}, LastName: {user.LastName}");

                // Return error
                return NotFound(404);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsers(int id, [FromBody] UserInput user)
        {

            try
            {
                // Update user in database according ID
                var newUser = await _uof.UserRepository.Update(id,user);

                // Map data convertion
                UserDTO userDTO = _mapper.Map<UserDTO>(newUser);

                // Log information about creating user
                _logger.LogInformation($"Update user: id: { user.Id }, FirstName: {user.FirstName}, LastName: {user.LastName}");

                // Return - element modified
                return Ok(userDTO);
            }
            catch (Exception ex)
            {
                // Log information about updating user error
                _logger.LogError(ex, $"ERROR: Update user: id: { user.Id }, FirstName: {user.FirstName}, LastName: {user.LastName}");

                // Return error
                return NotFound($"This user cannot be updated,\n {ex}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            try
            {
                // Get wanted to delete user from database
                var userToDelete = await _uof.UserRepository.Get(id);

                // Check if wanted user exists
                if (userToDelete == null)
                {
                    // Return information about user wasn't found
                    return NotFound("User with this ID doesn't exist");
                }

                // Delete user from database
                await _uof.UserRepository.Delete((int)userToDelete.Id);

                // Log information about uset delete
                _logger.LogInformation($"Delete user: id: { userToDelete.Id }, FirstName: {userToDelete.FirstName}, LastName: {userToDelete.LastName}");

                return Ok("The user has been deleted");
            }
            catch (Exception ex)
            {
                // Log information about delete user error
                _logger.LogError(ex, $"Delete user: id: { id }");
                
                // Return error
                return NotFound($"This user cannot be deleted,\n {ex}");
            }
        }
    }
}
