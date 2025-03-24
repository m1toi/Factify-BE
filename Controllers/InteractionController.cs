using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.BusinessLogic.Services.InteractionService;
using SocialMediaApp.DataAccess.Dtos.InteractionDto;

namespace SocialMediaApp.Controllers
{
	[Authorize]
	[Route("api/posts/{postId}/Interaction")]
	public class InteractionController : ControllerBase
	{
		private readonly IInteractionService _interactionService;

		public InteractionController(IInteractionService interactionService)
		{
			_interactionService = interactionService;
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public IActionResult Interact(int postId, [FromBody] InteractionRequestDto interactionDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if(!int.TryParse(userIdClaim, out int userId))
			{
				return Unauthorized();
			}

			_interactionService.HandleInteraction(userId, postId, interactionDto.Liked, interactionDto.Shared);
			return Ok();
		}
	}
}
