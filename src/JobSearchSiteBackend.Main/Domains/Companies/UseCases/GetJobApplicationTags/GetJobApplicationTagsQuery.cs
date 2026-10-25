using Ardalis.Result;
using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Main.Domains.Companies.UseCases.GetJobApplicationTags;

public record GetJobApplicationTagsQuery(long CompanyId, string? SearchQuery, int Size) : IRequest<Result<GetJobApplicationTagsResult>>;