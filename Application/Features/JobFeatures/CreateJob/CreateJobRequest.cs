using Domain.ValueObjects;
using MediatR;

namespace Application.Features.JobFeatures.CreateJob;

public sealed record CreateJobRequest : IRequest<CreateJobResponse>
{
    public long? CompanyId { get; init; }
    public long? CategoryId { get; init; }
    
    public string? Title { get; init; }
    public DateTime? DateTimeExpiringUtc { get; init; }
    public SalaryRecord? SalaryInfo { get; init; }
    public string? Description { get; init; }
    public IList<string?>? Responsibilities { get; init; }
    public IList<string?>? Requirements { get; init; }
    public IList<string?>? Advantages { get; init; }
    public EmploymentTypeRecord? EmploymentTypeRecord { get; init; }
    
    public bool? IsHidden { get; init; }

    public IList<string?>? Addresses { get; init; }
    public IList<long?>? ContractTypeIds { get; init; }
}