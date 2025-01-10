using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.CompanyClaims;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Shared.Result;

namespace Core.Domains.Companies.UseCases.GetCompanyById;

public class GetCompanyByIdHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context)
    : IRequestHandler<GetCompanyByIdRequest, Result<GetCompanyByIdResponse>>
{
    public async Task<Result<GetCompanyByIdResponse>> Handle(GetCompanyByIdRequest request,
        CancellationToken cancellationToken = default)
    {
        var company = await context.Companies.FindAsync([request.Id], cancellationToken);

        if (company is null)
            return Result<GetCompanyByIdResponse>.NotFound();

        if (!company.IsPublic)
        {
            var currentUserId = currentAccountService.GetId();

            if (currentUserId is null)
                return Result<GetCompanyByIdResponse>.Forbidden("Requested company profile is private.");
            
            var canEditProfile = await context.UserCompanyClaims
                .Where(
                    ucp => ucp.UserId == currentUserId 
                    && ucp.CompanyId == company.Id 
                    && ucp.ClaimId == CompanyClaim.CanEditProfile.Id)
                .AnyAsync(cancellationToken);

            if (!canEditProfile)
                return Result<GetCompanyByIdResponse>.Forbidden("Requested company profile is private.");
        }

        return new GetCompanyByIdResponse(company.Name, company.Description, company.CountryId);
    }
}