using SocialMediaApp.DataAccess.Dtos.ReportDto;
using System.Collections.Generic;

namespace SocialMediaApp.BusinessLogic.Services.ReportService
{
	public interface IReportService
	{
		void SubmitReport(int reporterUserId, ReportRequestDto dto);
		List<ReportResponseDto> GetPendingReports();
		void ResolveReport(int reportId, bool shouldDeletePost, int adminUserId);
		ReportResponseDto GetReportById(int reportId);
	}
}
