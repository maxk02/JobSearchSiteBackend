namespace JobSearchSiteBackend.API.Controllers.Companies.Dtos;

public record GetJobApplicationTagsRequest(string? SearchQuery, int Size);