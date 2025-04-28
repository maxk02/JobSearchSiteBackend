using Core.Domains._Shared.UseCaseStructure;

namespace Core.Domains.Companies.UseCases.GetDashboardStats;

public record GetDashboardStatsRequest(long CompanyId) : IRequest<GetDashboardStatsResponse>;