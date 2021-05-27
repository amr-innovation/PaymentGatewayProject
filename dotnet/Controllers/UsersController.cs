using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Repositories.IRepositories;
using Entity.Model;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Ardalis.GuardClauses;
using Microsoft.Extensions.Logging;

namespace server.Controllers
{

    /// <summary>
    /// This is the controller to generate the token for Auth
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        private IUserRepository _userRepo;
        public UsersController(IUserRepository userRepo, ILogger<UsersController> logger)
            : base(logger)
        {
            _userRepo = userRepo;
        }

        [HttpPost("authenticate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            Guard.Against.Null(model, nameof(model));

            try
            {
                var response = _userRepo.Authenticate(model);

                if (response == null)
                {
                    return NoContent();
                }

                return Ok(response);
            }
            catch (Exception exception)
            {
                Logger.LogInformation(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
