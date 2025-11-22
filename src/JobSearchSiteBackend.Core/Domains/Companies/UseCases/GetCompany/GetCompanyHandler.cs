using Ardalis.Result;
using AutoMapper;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Companies.Dtos;
using JobSearchSiteBackend.Core.Domains.CompanyClaims;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompany;

public class GetCompanyHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context,
    IMapper mapper)
    : IRequestHandler<GetCompanyQuery, Result<GetCompanyResult>>
{
    public async Task<Result<GetCompanyResult>> Handle(GetCompanyQuery query,
        CancellationToken cancellationToken = default)
    {
        var company = await context.Companies.FindAsync([query.Id], cancellationToken);

        if (company is null)
            return Result<GetCompanyResult>.NotFound();

        if (!company.IsPublic)
        {
            var currentUserId = currentAccountService.GetId();

            if (currentUserId is null)
                return Result<GetCompanyResult>.Forbidden("Requested company profile is private.");
            
            var canEditProfile = await context.UserCompanyClaims
                .Where(
                    ucp => ucp.UserId == currentUserId 
                    && ucp.CompanyId == company.Id 
                    && ucp.ClaimId == CompanyClaim.CanEditProfile.Id)
                .AnyAsync(cancellationToken);

            if (!canEditProfile)
                return Result<GetCompanyResult>.Forbidden("Requested company profile is private.");
        }
        
        var companyInfoDto = mapper.Map<CompanyDto>(company);
        
        return new GetCompanyResult(companyInfoDto);
    }
}