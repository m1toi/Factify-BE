using SocialMediaApp.DataAccess.Entity;
using System.Collections.Generic;

namespace SocialMediaApp.DataAccess.Repositories.ReportRepository
{
	public interface IReportRepository
	{
		Report CreateReport(Report report);
		Report GetPendingByPostId(int postId);
		Report GetReportById(int reportId);
		List<Report> GetAllPending();
		void UpdateReport(Report report);
	}
}
