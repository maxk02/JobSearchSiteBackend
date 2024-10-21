using Domain.Errors.Validation;
using Domain.Shared.Errors;
using Domain.Shared.Results;

namespace Domain.Entities.Users.ValueEntities;

public sealed class EducationRecord
{
    public string? Institution { get; private set; }
    public string? Location { get; private set; }
    public string? Faculty { get; private set; }
    public string? Speciality { get; private set; }
    public string? Degree { get; private set; }
    public string? Description { get; private set; }
    public DateOnly? DateOfStart { get; private set; }
    public DateOnly? DateOfFinish { get; private set; }

    public static Result<EducationRecord> Create(string? institution, string? location,
        string? faculty, string? speciality, string? degree,
        string? description, DateOnly? dateOfStart, DateOnly? dateOfFinish)
    {
        var educationRecord = new EducationRecord(institution, location, faculty, speciality,
            degree, description, dateOfStart, dateOfFinish);
        
        return Result.Success(educationRecord);
    }
    
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

    public IList<DomainLayerError> SetInstitution(string? institution)
    {
        var errors = new List<DomainLayerError>();
        
        // if (institution == null)
        // {
        //     Institution = institution;
        //     return errors;
        // }
        //
        // if (string.IsNullOrWhiteSpace(institution))
        // {
        //     errors.Add(new ValidationErrors.Strings.NullOrEmptyStringError(
        //         nameof(EducationRecord), nameof(Institution))
        //     );
        // }
        //
        // if (institution.Length < 2 || institution.Length > 40)
        // {
        //     errors.Add(new ValidationErrors.Strings.StringInvalidLengthError(
        //         nameof(EducationRecord), nameof(Institution), 2, 40)
        //     );
        // }

        if (errors.Count == 0)
            Institution = institution;

        return errors;
    }
}