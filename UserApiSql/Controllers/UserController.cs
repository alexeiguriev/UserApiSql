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

namespace UserApi.Controllers
{
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

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _uof.UserRepository.Get();
                IEnumerable<UserDTO> userDTO = _mapper.Map<IEnumerable<UserDTO>>(users);
                _logger.LogInformation("Get all users");
                return Ok(userDTO);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error: Get all users error");
                return NoContent();
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetUsers(int id)
        {
            try
            {
                var user = _uof.UserRepository.Get(id);
                UserDTO userDTO = _mapper.Map<UserDTO>(user);
                _logger.LogInformation($"Get user: id:  FirstName: {userDTO.FirstName}, LastName: {userDTO.LastName}");
                return Ok(userDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ERROR: Get user: id: { id }");
                return NoContent();
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostUsers([FromBody] User user)
        {
            try
            {
                var newUser = await _uof.UserRepository.Create(user);
                _logger.LogInformation($"Post user: id: { user.Id }, FirstName: {user.FirstName}, LastName: {user.LastName}");
                return Ok(CreatedAtAction(nameof(GetUsers), new { id = newUser.Id }, newUser));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ERROR: Post user: id: { user.Id }, FirstName: {user.FirstName}, LastName: {user.LastName}");
                return NoContent();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsers(int id, [FromBody] User user)
        {

            try
            {
                _logger.LogInformation($"Update user: id: { user.Id }, FirstName: {user.FirstName}, LastName: {user.LastName}");

                await _uof.UserRepository.Update(id,user);

                return Ok("Element Modified");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ERROR: Update user: id: { user.Id }, FirstName: {user.FirstName}, LastName: {user.LastName}");
                return NoContent();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            try
            {
                var userToDelete = await _uof.UserRepository.Get(id);
                if (userToDelete == null)
                    return NotFound();
                _logger.LogInformation($"Delete user: id: { userToDelete.Id }, FirstName: {userToDelete.FirstName}, LastName: {userToDelete.LastName}");

               await _uof.UserRepository.Delete((int)userToDelete.Id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Delete user: id: { id }");
                return NoContent();
            }
        }
    }
}
