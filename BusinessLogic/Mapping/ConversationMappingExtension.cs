using SocialMediaApp.DataAccess.Entity;
using SocialMediaApp.DataAccess.Dtos.ConversationDto;
using System.Collections.Generic;
using System.Linq;

namespace SocialMediaApp.BusinessLogic.Mapping
{
	public static class ConversationMappingExtensions
	{
		public static Conversation ToConversation(this ConversationRequestDto dto)
		{
			return new Conversation
			{
				User1Id = dto.User1Id,
				User2Id = dto.User2Id,
				CreatedAt = DateTimeOffset.UtcNow
			};
		}

		public static ConversationResponseDto ToConversationResponseDto(this Conversation conversation, int currentUserId)
		{
			var last = conversation.Messages?
				.OrderByDescending(m => m.SentAt)
				.FirstOrDefault();

			var unreadCount = conversation.Messages == null
				? 0 
				: conversation.Messages
					.Count(m => !m.IsRead && m.SenderId != currentUserId);
			return new ConversationResponseDto
			{
				ConversationId = conversation.ConversationId,
				User1Id = conversation.User1Id,
				User1Username = conversation.User1?.Username,
				User1ProfilePicture = conversation.User1?.ProfilePicture,
				User2Id = conversation.User2Id,
				User2Username = conversation.User2?.Username,
				User2ProfilePicture = conversation.User2?.ProfilePicture,
				CreatedAt = conversation.CreatedAt,
				LastMessage = last?.Content,
				LastMessageSenderId = last?.SenderId,
				LastMessageSentAt = last?.SentAt,
				UnreadCount = unreadCount,
				HasUnread = unreadCount > 0
			};
		}

		public static List<ConversationResponseDto> ToListConversationResponseDto(this List<Conversation> conversations, int currentUserId)
		{
			return conversations.Select(c => c.ToConversationResponseDto(currentUserId)).ToList();
		}

		public static List<ParticipantDto> ToParticipantDtos(this Conversation conv)
		{
			return new List<ParticipantDto>
		{
			new ParticipantDto
			{
				UserId = conv.User1Id,
				Username = conv.User1.Username,
				ProfilePicture = conv.User1.ProfilePicture
			},
			new ParticipantDto
			{
				UserId = conv.User2Id,
				Username = conv.User2.Username,
				ProfilePicture = conv.User2.ProfilePicture
			}
		};
		}
	}
}
