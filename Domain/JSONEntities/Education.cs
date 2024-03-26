namespace Domain.JSONEntities;

public class Education
{
    public string Institution { get; set; } = "";
    public string Location { get; set; } = "";
    public string? Faculty { get; set; } = "";
    public string Speciality { get; set; } = "";
    public string Degree { get; set; } = "";
    public string? Description { get; set; } = "";
    public DateOnly DateStarted { get; set; }
    public DateOnly? DateFinished { get; set; }
}