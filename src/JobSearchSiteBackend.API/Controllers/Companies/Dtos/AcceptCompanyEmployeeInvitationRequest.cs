namespace JobSearchSiteBackend.API.Controllers.Companies.Dtos;

public record AcceptCompanyEmployeeInvitationRequest(long UserId, string Token);