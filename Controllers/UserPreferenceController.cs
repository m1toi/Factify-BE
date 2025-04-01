using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.BusinessLogic.Services.UserPreferenceService;
using SocialMediaApp.DataAccess.Dtos.UserPreferenceDto;
using SocialMediaApp.DataAccess.Entity;

namespace SocialMediaApp.Controllers
{
	[Authorize(Roles = "User")]
	[Route("/api/UserPreference")]
	public class UserPreferenceController : ControllerBase
	{
		private readonly IUserPreferenceService _userPreferenceService;

		public UserPreferenceController(IUserPreferenceService userPreferenceService)
		{
			_userPreferenceService = userPreferenceService;
		}

		[HttpPost]
		public IActionResult Create([FromBody] UserPreferenceDto userPreferenceDto)
		{
			if(userPreferenceDto == null)
			{
				return BadRequest("No request from body received");
			}

			var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

			if (userIdClaim == null)
			{
				return Unauthorized();
			}

			var userId = int.Parse(userIdClaim.Value);

			_userPreferenceService.Create(userPreferenceDto, userId);

			return Ok();
		}

		[HttpGet("has-preferences")]
		public IActionResult HasPreferences()
		{
			var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

			if (userIdClaim == null)
			{
				return Unauthorized();
			}

			var userId = int.Parse(userIdClaim.Value);

			bool hasPreferences = _userPreferenceService.HasPreference(userId);

			return Ok(hasPreferences);
		}
	}
}
