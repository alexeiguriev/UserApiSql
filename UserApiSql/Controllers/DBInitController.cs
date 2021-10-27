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
    public class DBInitController : ControllerBase
    {

        private readonly UserContext dc;

        public DBInitController(UserContext dc)
        {
            this.dc = dc;
        }
        [HttpPost]
        public IActionResult PostDBInit()
        {
            Console.WriteLine("--> DB Initalization Request ....");
            try
            {
                string state = DbInitializer.Initialize(dc);
                return Ok(state);
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }
    }
}
