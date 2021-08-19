using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HelperCSharp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserApiSql.Helpers;
using UserApiSql.Interfaces;
using UserApiSql.Models;
using UserApiSql.ModelsDTO;

namespace UserApiSql.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotiController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uof;
        private readonly ILogger _logger;
        private readonly JwtService _jwtService;

        public NotiController(IUnitOfWork uof, IMapper mapper, ILogger<AuthController> logger, JwtService jwtService)
        {
            _uof = uof;
            _mapper = mapper;
            _logger = logger;
            _jwtService = jwtService;
        }
        [HttpGet]
        public async Task<IActionResult> GetNotis()
        {
            try
            {
                // Get from Database all the notifications
                var noti = await _uof.NotiRepository.Get();

                // Map data convertion
                IEnumerable<NotiDTO> notiDTO = _mapper.Map<IEnumerable<NotiDTO>>(noti);

                // Log information about get oaa notifications
                _logger.LogInformation("Get all notifications");

                // Return all the notifications
                return Ok(notiDTO);
            }
            catch (Exception ex)
            {
                // Return information about get notifications error
                _logger.LogError(ex, "Error: Get all notifications error");

                // Return not found
                return NotFound(404);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNoti(int id)
        {
            try
            {
                // Get the notification from database according ID
                var noti = await _uof.NotiRepository.Get(id);

                // Map data convertion
                NotiDTO notiDTO = _mapper.Map<NotiDTO>(noti);

                // Log data about requested notification.
                _logger.LogInformation($"Get notification: id:  {noti.Id}");

                // Return notification data
                return Ok(notiDTO);
            }
            catch (Exception ex)
            {
                // Log information about get notification error
                _logger.LogError(ex, $"ERROR: Get notification: id: { id }");

                // Return not found
                return NotFound(404);
            }
        }
        [HttpGet("{id}/byuserto")]
        public async Task<IActionResult> GetNotisByUserTo(int id)
        {
            try
            {
                // Get from Database all the notifications
                IEnumerable<Noti> notiEnumerable = await _uof.NotiRepository.Get();
                List<Noti> noti = notiEnumerable.ToList();

                noti = noti.FindAll(n => n.ToUser.Id == id);

                // Map data convertion
                IEnumerable<NotiDTO> notiDTO = _mapper.Map<IEnumerable<NotiDTO>>(noti);

                // Log information about get oaa notifications
                _logger.LogInformation("Get all notifications");

                // Return all the notifications
                return Ok(notiDTO);
            }
            catch (Exception ex)
            {
                // Return information about get notifications error
                _logger.LogError(ex, "Error: Get all notifications error");

                // Return not found
                return NotFound(404);
            }
        }


        [HttpPost]
        public async Task<IActionResult> PostNotis([FromBody] NotiInput noti)
        {
            try
            {
                // Create and put new noti in database
                var newNoti = await _uof.NotiRepository.Create(noti);

                // Log information about new created noti
                _logger.LogInformation($"Post noti: id: { noti.Id }");

                // Map data convertion
                NotiDTO notiDTO = _mapper.Map<NotiDTO>(newNoti);

                // Return new created noti
                return Ok(notiDTO);
            }
            catch (Exception ex)
            {
                // Log information about creating noti error
                _logger.LogError(ex, $"ERROR: Post noti: id: { noti.Id }");

                // Return error
                return NotFound(404);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutNotis(int id, [FromBody] NotiInput noti)
        {

            try
            {

                // Update noti in database according ID
                var newNoti = await _uof.NotiRepository.Update(id, noti);

                // Map data convertion
                NotiDTO notiDTO = _mapper.Map<NotiDTO>(newNoti);

                // Log information about creating noti
                _logger.LogInformation($"Update noti: id: { noti.Id }");

                // Return - element modified
                return Ok(notiDTO);
            }
            catch (Exception ex)
            {
                // Log information about updating noti error
                _logger.LogError(ex, $"ERROR: Update noti: id: { noti.Id }");

                // Return error
                return NotFound($"This noti cannot be updated,\n {ex}");
            }
        }
        [HttpPut("{id}/isread")]
        public async Task<IActionResult> PutNotisAsIsRead(int id)
        {
            NotiInput noti = new NotiInput()
            {
                Id = id,
                IsRead = true,
                CreatedDate = DateTime.MinValue
            };
            try
            {

                // Update noti in database according ID
                var newNoti = await _uof.NotiRepository.Update(id, noti);

                // Map data convertion
                NotiDTO notiDTO = _mapper.Map<NotiDTO>(newNoti);

                // Log information about creating noti
                _logger.LogInformation($"Update noti: id: { noti.Id }");

                // Return - element modified
                return Ok(notiDTO);
            }
            catch (Exception ex)
            {
                // Log information about updating noti error
                _logger.LogError(ex, $"ERROR: Update noti: id: { noti.Id }");

                // Return error
                return NotFound($"This noti cannot be updated,\n {ex}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            try
            {
                // Get wanted to delete noti from database
                var notiToDelete = await _uof.NotiRepository.Get(id);

                // Check if wanted noti exists
                if (notiToDelete == null)
                {
                    // Return information about noti wasn't found
                    return NotFound("Noti with this ID doesn't exist");
                }

                // Delete noti from database
                await _uof.NotiRepository.Delete((int)notiToDelete.Id);

                // Log information about uset delete
                _logger.LogInformation($"Delete noti: id: { notiToDelete.Id }");

                return Ok("The noti has been deleted");
            }
            catch (Exception ex)
            {
                // Log information about delete noti error
                _logger.LogError(ex, $"Delete noti: id: { id }");

                // Return error
                return NotFound($"This noti cannot be deleted,\n {ex}");
            }
        }
    }
}