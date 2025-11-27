namespace JobSearchSiteBackend.API.Controllers.Account.Dtos;

public record ResetForgottenPasswordRequest(string Token, string NewPassword);