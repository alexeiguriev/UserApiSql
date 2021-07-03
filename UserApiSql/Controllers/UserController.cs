using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserApi.Models;
using UserApiSql.Data;
using UserApiSql.Models;
using UserApiSql.Repository;

namespace UserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly ILogger _logger;

        //public UserController(IUserRepository userRepository, IMapper mapper, ILogger logger)
        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            //_logger = logger;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            try
            {
                var users = _userRepository.Get();
                IEnumerable<UserDTO> userDTO = _mapper.Map<IEnumerable<UserDTO>>(users);
                //_logger.LogInformation("Get all users");
                return Ok(userDTO);
            }
            catch(Exception ex)
            {
                //_logger.LogError(ex, "Error: Get all users error");
                return NoContent();
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetUsers(int id)
        {
            try
            {
                var user = _userRepository.Get(id);
                UserDTO userDTO = _mapper.Map<UserDTO>(user);
                //_logger.LogInformation($"Get user: id: { userDTO.Id }, FirstName: {userDTO.FirstName}, LastName: {userDTO.LastName}");
                return Ok(userDTO);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "ERROR: Get user: id: { userDTO.Id }, FirstName: {userDTO.FirstName}, LastName: {userDTO.LastName}");
                return NoContent();
            }
        }

        [HttpPost]
        public IActionResult PostUsers([FromBody] User user)
        {
            try
            {
                var newUser = _userRepository.Create(user);
                //_logger.LogInformation($"Post user: id: { user.Id }, FirstName: {user.FirstName}, LastName: {user.LastName}");
                return Ok(CreatedAtAction(nameof(GetUsers), new { id = newUser.Id }, newUser));
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, $"ERROR: Post user: id: { user.Id }, FirstName: {user.FirstName}, LastName: {user.LastName}");
                return NoContent();
            }
        }

        [HttpPut("{id}")]
        public IActionResult PutUsers(int id, [FromBody] User user)
        {

            try
            {
                if (id != user.Id)
                {
                    return BadRequest();
                }
                //_logger.LogInformation($"Update user: id: { user.Id }, FirstName: {user.FirstName}, LastName: {user.LastName}");

                _userRepository.Update(user);

                return NoContent();
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, $"ERROR: Update user: id: { user.Id }, FirstName: {user.FirstName}, LastName: {user.LastName}");
                return NoContent();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {

            try
            {
                var userToDelete = _userRepository.Get(id);
                if (userToDelete == null)
                    return NotFound();
                //_logger.LogInformation($"Delete user: id: { userToDelete.Id }, FirstName: {userToDelete.FirstName}, LastName: {userToDelete.LastName}");

                _userRepository.Delete((int)userToDelete.Id);
                return NoContent();
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, $"Delete user: id: { userToDelete.Id }, FirstName: {userToDelete.FirstName}, LastName: {userToDelete.LastName}");
                return NoContent();
            }
        }
    }
}
