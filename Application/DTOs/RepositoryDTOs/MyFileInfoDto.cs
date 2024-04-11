namespace Application.DTOs.RepositoryDTOs;

public sealed record MyFileInfoDto
{
    public long Id { get; set; }
    public DateTimeOffset DateTimeCreated { get; set; }
    public string Name { get; set; } = "";
    public string Extension { get; set; } = "";
    public string Size { get; set; } = "";
}