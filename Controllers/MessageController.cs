using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.BusinessLogic.Services.MessageService;
using SocialMediaApp.DataAccess.Dtos.MessageDto;
using System.Security.Claims;

namespace SocialMediaApp.Controllers
{
	[Authorize]
	[Route("api/Messages")]
	public class MessageController : ControllerBase
	{
		private readonly IMessageService _messageService;

		public MessageController(IMessageService messageService)
		{
			_messageService = messageService;
		}

		[HttpGet("conversation/{conversationId}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<List<MessageResponseDto>> GetMessagesByConversation([FromRoute] int conversationId)
		{
			var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
			var messages = _messageService.GetMessagesByConversation(conversationId, userId);
			return Ok(messages);
		}

		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<MessageResponseDto> GetMessage([FromRoute] int id)
		{
			var message = _messageService.GetMessage(id);
			return Ok(message);
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		public async Task<IActionResult> SendMessage([FromBody] MessageRequestDto messageDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			// Validate sender ID from token
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(userId))
				return Unauthorized("Invalid user token.");

			messageDto.SenderId = int.Parse(userId);

			try
			{
				// Await the async service method
				var createdMessage = await _messageService.SendMessage(messageDto);
				return CreatedAtAction(
					nameof(GetMessage),
					new { id = createdMessage.MessageId },
					createdMessage);
			}
			catch (UnauthorizedAccessException ex)
			{
				return Forbid(ex.Message);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult DeleteMessage([FromRoute] int id)
		{
			// Optionally restrict delete to sender (depends on business rules)
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var message = _messageService.GetMessage(id);

			if (message.SenderId.ToString() != userId)
				return Forbid("You are not authorized to delete this message.");

			_messageService.DeleteMessage(id);
			return NoContent();
		}
	}
}
