using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using oauth_service.Helpers;
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
    public class AuthController : ControllerBase
    {
        protected ILogger Logger;
        protected IUserRepository UserRepository;
        protected IJwtAuth Jwt;

        public AuthController(ILoggerFactory logger, IUserRepository userRepository, IJwtAuth jwt)
        {
            Logger = logger.CreateLogger(GetType()) ?? throw new ArgumentNullException(nameof(Logger));
            UserRepository = userRepository ?? throw new ArgumentNullException(nameof(UserRepository));
            Jwt = jwt ?? throw new ArgumentNullException(nameof(Jwt));
        }

        [HttpPost("token")]
        public IActionResult Token([FromBody] User user)
        {
            try
            {
                var userExists = UserRepository.GetByEmail(user.Email, user.Password);

                return Ok(Jwt.GenerateToken(userExists));
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Ocorreu algum erro interno na aplicação, por favor tente novamente." });
            }
        }
    }
}
