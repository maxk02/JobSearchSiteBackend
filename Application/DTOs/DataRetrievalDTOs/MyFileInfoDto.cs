namespace Application.DTOs.DataRetrievalDTOs;

public sealed record MyFileInfoDto
{
    public int Id { get; set; }
    public DateTime DateTimeCreated { get; set; }
    public string Name { get; set; } = "";
    public string Extension { get; set; } = "";
    public string Size { get; set; } = "";
}