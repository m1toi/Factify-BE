using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.BusinessLogic.Services.UserService;
using SocialMediaApp.DataAccess.Dtos.UserDto;

namespace SocialMediaApp.Controllers
{
    [Route("/api/Users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult Register([FromBody] UserRequestDto userDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _userService.Register(userDto);
            return Ok();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<UserResponseDto>> GetAll()
        {
            return Ok(_userService.GetAll());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<UserResponseDto> GetById([FromRoute] int id)
        {
            return Ok(_userService.GetById(id));
        }

        [HttpPut("{id}")]        
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult Update([FromRoute] int id, [FromBody] UserRequestDto updatedUserDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _userService.Update(id, updatedUserDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete([FromRoute] int id)
        {
            _userService.Delete(id);
            return Ok();
        }
    }
}
