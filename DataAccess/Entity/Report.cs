using SocialMediaApp.DataAccess.Entity.Enums;

namespace SocialMediaApp.DataAccess.Entity
{
	public class Report
	{
		public int ReportId { get; set; }

		public int PostId { get; set; }
		public Post Post { get; set; }

		public int ReporterUserId { get; set; }
		public User ReporterUser { get; set; }

		public ReportReason Reason { get; set; }

		public ReportStatus Status { get; set; }

		public DateTime CreatedAt { get; set; }
		public DateTime? SolvedAt { get; set; }

		public int? AdminUserId { get; set; }
		public User AdminUser { get; set; }
	}
}
