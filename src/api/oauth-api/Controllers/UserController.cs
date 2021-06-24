using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using oauth_service.Models;
using oauth_service.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace oauth_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        protected ILogger Logger;
        protected IUserRepository UserRepository;

        public UserController(ILoggerFactory logger, IUserRepository userRepository)
        {
            Logger = logger.CreateLogger(GetType()) ?? throw new ArgumentNullException(nameof(Logger));
            UserRepository = userRepository ?? throw new ArgumentNullException(nameof(UserRepository));
        }

        [Authorize]
        [HttpPost("create")]
        public IActionResult Create([FromBody] User user)
        {
            try
            {
                Logger.LogInformation("Controller User Create");

                UserRepository.Create(user);

                return NoContent();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [Authorize]
        [HttpPut("update")]
        public IActionResult Update([FromBody] User user)
        {
            try
            {
                Logger.LogInformation("Controller User Update");

                UserRepository.Update(user);

                return NoContent();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [Authorize]
        [HttpGet("getAll")]
        public IActionResult GetAll()
        {
            try
            {
                Logger.LogInformation("Controller User GetAll");

                return Ok(UserRepository.GetAll());
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [Authorize]
        [HttpGet("getById")]
        public IActionResult GetById(int id)
        {
            try
            {
                Logger.LogInformation("Controller User GetById");

                var users = UserRepository.GetById(id);

                return null;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [Authorize]
        [HttpGet("getByEmail")]
        public IActionResult GetByEmail(string email, string password)
        {
            try
            {
                Logger.LogInformation("Controller User GetByEmail");

                var users = UserRepository.GetByEmail(email, password);

                return null;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
    }
}
