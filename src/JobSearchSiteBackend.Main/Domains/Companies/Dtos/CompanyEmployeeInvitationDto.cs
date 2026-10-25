namespace JobSearchSiteBackend.Main.Domains.Companies.Dtos;

public record CompanyEmployeeInvitationDto(long Id, DateTime DateTimeCreatedUtc, 
    DateTime DateTimeValidUtc, bool IsAccepted);