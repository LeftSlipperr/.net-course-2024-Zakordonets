using BankSystem.App.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AspControllerExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IClientStorage _clientStorage;

        public UserController(IClientStorage clientStorage)
        {
            _clientStorage = clientStorage;
        }

        [HttpGet]
        public async Task<IActionResult>  GetUser([FromQuery] Guid guid, CancellationToken cancellationToken) 
        {
            var response = await _clientStorage.GetAsync(guid, cancellationToken);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserDto user, CancellationToken cancellationToken)
        { 
            var response = await _userService.AddUser(user, cancellationToken);
            return Ok(response);
        }
    }
}