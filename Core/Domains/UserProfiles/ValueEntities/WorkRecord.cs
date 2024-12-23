namespace Core.Domains.UserProfiles.ValueEntities;

public record WorkRecord(string? Position, string? Company, string? Location,
    string? Description, IReadOnlyCollection<string> Responsibilities);