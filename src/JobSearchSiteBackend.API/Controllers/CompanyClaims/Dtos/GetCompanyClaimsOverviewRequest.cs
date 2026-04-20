using Ardalis.Result;
using JobSearchSiteBackend.API.ModelBinders;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Microsoft.AspNetCore.Mvc;

namespace JobSearchSiteBackend.API.Controllers.CompanyClaims.Dtos;

public record GetCompanyClaimsOverviewRequest(
    [ModelBinder(BinderType = typeof(CommaSeparatedArrayModelBinder))]
    ICollection<long>? CompanyClaimIds,
    string? UserQuery,
    int Page,
    int Size) : IRequest<Result<GetCompanyClaimsOverviewResponse>>;