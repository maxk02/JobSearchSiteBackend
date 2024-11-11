using Shared.Results;

namespace Domain.Entities.Users.ValueEntities;

public class WorkRecord
{
    public static WorkRecordValidator Validator { get; } = new();

    public static Result<WorkRecord> Create(string? position, string? company, string? location,
        string? description, ICollection<string> responsibilities)
    {
        var workRecord = new WorkRecord(position, company, location, description, responsibilities);

        var validationResult = Validator.Validate(workRecord);

        return validationResult.IsValid
            ? Result.Success(workRecord)
            : Result.Failure<WorkRecord>(validationResult.Errors);
    }
    
    private WorkRecord(string? position, string? company, string? location,
        string? description, ICollection<string> responsibilities)
    {
        Position = position;
        Company = company;
        Location = location;
        Description = description;
        _responsibilities = responsibilities.ToList();
    }
    
    public string? Position { get; private set; }
    public string? Company { get; private set; }
    public string? Location { get; private set; }
    public string? Description { get; private set; }

    private List<string> _responsibilities;
    public IReadOnlyCollection<string> Responsibilities => _responsibilities.AsReadOnly();
}