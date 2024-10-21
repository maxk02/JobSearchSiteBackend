namespace Domain.ValueObjects;

public class EmploymentTypeRecord
{
    public bool? IsPartTime { get; set; }
    public bool? IsFullTime { get; set; }
    public bool? IsOnSite { get; set; }
    public bool? IsRemote { get; set; }
    public bool? IsHybrid { get; set; }
    public bool? IsMobile { get; set; }
}