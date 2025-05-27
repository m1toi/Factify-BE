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

		public static ConversationResponseDto ToConversationResponseDto(this Conversation conversation)
		{
			return new ConversationResponseDto
			{
				ConversationId = conversation.ConversationId,
				User1Id = conversation.User1Id,
				User1Username = conversation.User1?.Username,
				User1ProfilePicture = conversation.User1?.ProfilePicture,
				User2Id = conversation.User2Id,
				User2Username = conversation.User2?.Username,
				User2ProfilePicture = conversation.User2?.ProfilePicture,
				CreatedAt = conversation.CreatedAt
			};
		}

		public static List<ConversationResponseDto> ToListConversationResponseDto(this List<Conversation> conversations)
		{
			return conversations.Select(c => c.ToConversationResponseDto()).ToList();
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
