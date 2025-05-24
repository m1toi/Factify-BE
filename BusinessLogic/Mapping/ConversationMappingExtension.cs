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
				CreatedAt = DateTime.UtcNow
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
	}
}
