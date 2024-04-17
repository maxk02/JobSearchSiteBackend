namespace Application.Features.JobFeatures.EditJob.NestedDTOs.AddressDTO;

public record EditJobAddressDto
{
    public int? LocationId { get; set; }
    public string? NonStandardLocationName { get; set; }
    public string? Line1 { get; set; }
    public string? Line2 { get; set; }
    public string? ZipCode { get; set; }
}