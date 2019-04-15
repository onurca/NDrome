using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ndrome.Model.Authentication;
using Ndrome.Service.Interfaces;

namespace Ndrome.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]User userParam)
        {
            var user = _userService.Authenticate(userParam.Username, userParam.Password);

            if (user == null)
                return BadRequest(new { Message = "Username or password is incorrect" });

            return Ok(user);
        }
    }
}