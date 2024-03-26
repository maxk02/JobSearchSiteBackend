namespace Domain.JSONEntities;

public class EmploymentType
{
    public bool IsPartTime { get; set; } = false;
    public bool IsFullTime { get; set; } = false;
    public bool IsOnSite { get; set; } = false;
    public bool IsRemote { get; set; } = false;
    public bool IsHybrid { get; set; } = false;
    public bool IsMobile { get; set; } = false;
}