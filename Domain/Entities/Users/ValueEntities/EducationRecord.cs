using Shared.Results;

namespace Domain.Entities.Users.ValueEntities;

public sealed class EducationRecord
{
    private EducationRecord(string? institution, string? location,
        string? faculty, string? speciality, string? degree,
        string? description, DateOnly? dateOfStart, DateOnly? dateOfFinish)
    {
        Institution = institution;
        Location = location;
        Faculty = faculty;
        Speciality = speciality;
        Degree = degree;
        Description = description;
        DateOfStart = dateOfStart;
        DateOfFinish = dateOfFinish;
    }
    
    public static Result<EducationRecord> Create(string? institution, string? location,
        string? faculty, string? speciality, string? degree,
        string? description, DateOnly? dateOfStart, DateOnly? dateOfFinish)
    {
        var educationRecord = new EducationRecord(institution, location, faculty, speciality,
            degree, description, dateOfStart, dateOfFinish);
        
        return Result.Success(educationRecord);
    }
    
    public string? Institution { get; private set; }
    public string? Location { get; private set; }
    public string? Faculty { get; private set; }
    public string? Speciality { get; private set; }
    public string? Degree { get; private set; }
    public string? Description { get; private set; }
    public DateOnly? DateOfStart { get; private set; }
    public DateOnly? DateOfFinish { get; private set; }
}