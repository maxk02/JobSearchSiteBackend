using Domain.Common;

namespace Domain.Entities;

public class Company : BaseEntity, IHideableEntity
{
    public string Name { get; set; } = "";
    public DateOnly? DateFounded { get; set; }
    public string? Description { get; set; }
    public bool IsHidden { get; set; }
}