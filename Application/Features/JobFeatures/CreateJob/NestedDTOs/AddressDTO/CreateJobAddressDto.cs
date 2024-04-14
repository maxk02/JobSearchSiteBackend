namespace Application.Features.JobFeatures.CreateJob.NestedDTOs.AddressDTO;

public record CreateJobAddressDto
{
    public long? LocationId { get; set; }
    public string? NonStandardLocationName { get; set; }
    public string? Line1 { get; set; }
    public string? Line2 { get; set; }
    public string? ZipCode { get; set; }
}