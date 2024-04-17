using Application.Features.JobFeatures.EditJob.NestedDTOs.AddressDTO;
using Domain.JSONEntities;
using MediatR;

namespace Application.Features.JobFeatures.EditJob;

public sealed record EditJobRequest : IRequest
{
    public int? Id { get; init; }
    public int? CategoryId { get; init; }
    
    public string? Title { get; init; }
    public DateTime? DateTimeExpiringUtc { get; init; }
    public SalaryRecord? SalaryInfo { get; init; }
    public string? Description { get; init; }
    public IList<string?>? Responsibilities { get; init; }
    public IList<string?>? Requirements { get; init; }
    public IList<string?>? Advantages { get; init; }
    public EmploymentTypeRecord? EmploymentTypeRecord { get; init; }
    
    public bool? IsHidden { get; init; }

    public IList<EditJobAddressDto?>? Addresses { get; init; }
    public IList<int?>? ContractTypeIds { get; init; }
}