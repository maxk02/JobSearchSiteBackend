using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetJobApplicationTags;

public record GetJobApplicationTagsQuery(long CompanyId, string? SearchQuery, int Size) : IRequest<Result<GetJobApplicationTagsResult>>;