using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.GetJobApplicationTagsForCompany;

public record GetJobApplicationTagsForCompanyQuery(long CompanyId, string? SearchQuery, int Size) : IRequest<Result<GetJobApplicationTagsForCompanyResult>>;