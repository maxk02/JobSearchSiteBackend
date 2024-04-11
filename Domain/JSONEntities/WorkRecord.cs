namespace Domain.JSONEntities;

public class WorkRecord
{
    public string Company { get; set; } = "";
    public string Location { get; set; } = "";
    public IList<string> Responsibilities { get; set; } = [];
    public string? Description { get; set; }
}