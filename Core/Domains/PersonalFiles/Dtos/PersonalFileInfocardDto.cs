namespace Core.Domains.PersonalFiles.Dtos;

public record PersonalFileInfocardDto(long Id, long UserId, string Name, string Extension, long Size);