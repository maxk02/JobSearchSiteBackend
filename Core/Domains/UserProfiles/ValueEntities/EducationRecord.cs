namespace Core.Domains.UserProfiles.ValueEntities;

public record EducationRecord(string? Institution, string? Location, string? Faculty,
    string? Speciality, string? Degree, string? Description, DateOnly? DateOfStart, DateOnly? DateOfFinish);