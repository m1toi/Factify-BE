﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.BusinessLogic.Services.ConversationService;
using SocialMediaApp.DataAccess.Dtos.ConversationDto;
using System.Security.Claims;

namespace SocialMediaApp.Controllers
{
	[Authorize]
	[Route("api/Conversations")]
	public class ConversationController : ControllerBase
	{
		private readonly IConversationService _conversationService;

		public ConversationController(IConversationService conversationService)
		{
			_conversationService = conversationService;
		}

		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<ConversationResponseDto> GetById([FromRoute] int id)
		{
			var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
			var conversation = _conversationService.GetConversation(id, userId);
			return Ok(conversation);
		}

		[HttpGet("{id}/participants")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<List<ParticipantDto>> GetParticipants([FromRoute] int id)
		{
			try
			{
				var participants = _conversationService.GetConversationParticipants(id);
				return Ok(participants);
			}
			catch (Exception ex)
			{
				return NotFound(ex.Message);
			}
		}

		[HttpGet("between/{otherUserId}")]
		public ActionResult<ConversationResponseDto> GetBetweenUsers(int otherUserId)
		{
			var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
			var dto = _conversationService.GetConversationBetweenUsers(userId, otherUserId, userId);
			if (dto == null) return NotFound();
			return Ok(dto);
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult CreateConversation([FromBody] ConversationRequestDto conversationDto)
		{
			var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

			if (conversationDto.User1Id != userId && conversationDto.User2Id != userId)
				return Forbid("You must be one of the participants to create a conversation.");

			try
			{
				var createdConversation = _conversationService.CreateConversation(conversationDto, userId);
				return CreatedAtAction(nameof(GetById), new { id = createdConversation.ConversationId }, createdConversation);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("mine")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<List<ConversationResponseDto>> GetMyConversations()
		{
			var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
			var conversations = _conversationService.GetUserConversations(userId);
			return Ok(conversations);
		}

		[HttpPost("{id}/mark-read")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		public async Task<IActionResult> MarkRead([FromRoute] int id)
		{
			var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
			try
			{
				// ▶ apel asincron în service
				await _conversationService.MarkConversationAsReadAsync(id, userId);
				return Ok();
			}
			catch (UnauthorizedAccessException)
			{
				return Forbid();
			}
		}
	}
}
