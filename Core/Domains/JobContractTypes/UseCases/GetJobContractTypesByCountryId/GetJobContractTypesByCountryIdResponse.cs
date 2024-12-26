namespace Core.Domains.JobContractTypes.UseCases.GetJobContractTypesByCountryId;

public record GetJobContractTypesByCountryIdResponse(long CountryId, string Name);