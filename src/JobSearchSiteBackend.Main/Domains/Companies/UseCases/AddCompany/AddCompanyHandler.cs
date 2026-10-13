using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.CompanyClaims;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Serialization;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.AddCompany;

public class AddCompanyHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context)
    : IRequestHandler<AddCompanyCommand, Result<AddCompanyResult>>
{
    public async Task<Result<AddCompanyResult>> Handle(AddCompanyCommand command,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var settings = new JsonSerializerSettings
        {
            MissingMemberHandling = MissingMemberHandling.Error,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
        
        var schema = JSchema.Parse(CountrySpecificCompanyFieldsJsonSchemas.GetForCountryId(command.CountryId));
        var fields = JObject.Parse(command.CountrySpecificFieldsJson);
        var isValid = fields.IsValid(schema);

        //creating company and checking result
        var company = new Company(command.Name, command.Description,
            true, command.CountryId, command.CountrySpecificFieldsJson);

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        //adding company and saving early to retrieve generated id
        await context.Companies.AddAsync(company, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        // flagging company for update
        context.Companies.Update(company);
        
        //adding full permission set to user
        company.UserCompanyClaims = CompanyClaim.AllIds
            .Select(x => new UserCompanyClaim(currentUserId, company.Id, x)).ToList();
        
        // saving changes
        await context.SaveChangesAsync(cancellationToken);
        
        //committing transaction
        await transaction.CommitAsync(cancellationToken);

        return new AddCompanyResult(company.Id);
    }
}