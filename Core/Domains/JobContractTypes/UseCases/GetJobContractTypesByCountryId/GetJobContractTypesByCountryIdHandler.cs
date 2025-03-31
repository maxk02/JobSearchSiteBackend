// using Core.Domains._Shared.UseCaseStructure;
// using Core.Domains.JobContractTypes.Dtos;
// using Core.Persistence.EfCore;
// using Microsoft.EntityFrameworkCore;
// using Ardalis.Result;
//
// namespace Core.Domains.JobContractTypes.UseCases.GetJobContractTypesByCountryId;
//
// public class GetJobContractTypesByCountryIdHandler(MainDataContext context)
//     : IRequestHandler<GetJobContractTypesByCountryIdRequest, Result<GetJobContractTypesByCountryIdResponse>>
// {
//     public async Task<Result<GetJobContractTypesByCountryIdResponse>> Handle(
//         GetJobContractTypesByCountryIdRequest request, CancellationToken cancellationToken = default)
//     {
//         var contractTypes = await context.ContractTypes
//             .Where(jobContractType => jobContractType.CountryId == request.CountryId)
//             .Select(jobContractType => new JobContractTypeDto(jobContractType.Id, jobContractType.Name))
//             .ToListAsync(cancellationToken);
//
//         return new GetJobContractTypesByCountryIdResponse(contractTypes);
//     }
// }