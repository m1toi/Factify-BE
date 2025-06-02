using SocialMediaApp.DataAccess.Entity.Enums;

namespace SocialMediaApp.DataAccess.Dtos.ReportDto
{
	public class ReportRequestDto
	{
		public int PostId { get; set; }
		public ReportReason Reason { get; set; }
	}
}
