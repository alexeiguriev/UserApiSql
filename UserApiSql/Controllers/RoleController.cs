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
    public class RoleController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uof;
        private readonly ILogger _logger;

        public RoleController(IUnitOfWork uof, IMapper mapper, ILogger<RoleController> logger)
        {
            _uof = uof;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                // Get from Database all the Roles
                var roles = await _uof.RoleRepository.Get();

                // Map data convertion
                IEnumerable<RoleDTO> roleDTO = _mapper.Map<IEnumerable<RoleDTO>>(roles);

                // Log information about get oaa roles
                _logger.LogInformation("Get all roles");

                // Return all the roles
                return Ok(roleDTO);
            }
            catch(Exception ex)
            {
                // Return information about get roles error
                _logger.LogError(ex, "Error: Get all roles error");

                // Return not found
                return NotFound(404);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoles(int id)
        {
            try
            {
                // Get the role from database according ID
                var role = await _uof.RoleRepository.Get(id);

                // Map data convertion
                RoleDTO roleDTO = _mapper.Map<RoleDTO>(role);

                // Log data about requested role.
                _logger.LogInformation($"Get role: Role Name: {roleDTO.Name}");

                // Return role data
                return Ok(roleDTO);
            }
            catch (Exception ex)
            {
                // Log information about get role error
                _logger.LogError(ex, $"ERROR: Get role: id: { id }");

                // Return not found
                return NotFound(404);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostRoles([FromBody] Role role)
        {
            try
            {
                // Create and put new role in database
                var newRole = await _uof.RoleRepository.Create(role);

                // Log information about new created role
                _logger.LogInformation($"Post role: {role.Name}");

                // Map data convertion
                RoleDTO roleDTO = _mapper.Map<RoleDTO>(newRole);

                // Return new created role
                return Ok(roleDTO);
            }
            catch (Exception ex)
            {
                // Log information about creating role error
                _logger.LogError(ex, $"ERROR: Post role: {role.Name}");

                // Return error
                return NotFound(404);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoles(int id, [FromBody] Role role)
        {

            try
            {
                // Update role in database according ID
                var newRole = await _uof.RoleRepository.Update(id,role);

                // Map data convertion
                RoleDTO roleDTO = _mapper.Map<RoleDTO>(newRole);

                // Log information about creating role
                _logger.LogInformation($"Update role: {role.Name}");

                // Return - element modified
                return Ok(roleDTO);
            }
            catch (Exception ex)
            {
                // Log information about updating role error
                _logger.LogError(ex, $"ERROR: Update role: {role.Name}");

                // Return error
                return NotFound(404);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            try
            {
                // Get wanted to delete role from database
                var roleToDelete = await _uof.RoleRepository.Get(id);

                // Check if wanted role exists
                if (roleToDelete == null)
                {
                    // Return information about role wasn't found
                    return NotFound("Role with this ID doesn't exist");
                }

                // Delete role from database
                await _uof.RoleRepository.Delete((int)roleToDelete.Id);

                // Log information about uset delete
                _logger.LogInformation($"Delete role: {roleToDelete.Name}");

                return Ok("The role has been deleted");
            }
            catch (Exception ex)
            {
                // Log information about delete role error
                _logger.LogError(ex, $"Delete role: id: { id }");
                
                // Return error
                return NotFound(404);
            }
        }
    }
}
