using JobSearchSiteBackend.Main.Domains.Jobs.Dtos;

namespace JobSearchSiteBackend.Main.Domains.Jobs.UseCases.GetJob;

public record GetJobResult(JobDetailedDto Job);