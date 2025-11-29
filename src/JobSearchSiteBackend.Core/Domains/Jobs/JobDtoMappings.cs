using JobSearchSiteBackend.Core.Domains.Jobs.Dtos;

namespace JobSearchSiteBackend.Core.Domains.Jobs;

public static class JobDtoMappings
{
    public static JobSalaryInfoDto ToJobSalaryInfoDto(this JobSalaryInfo jobSalaryInfo)
    {
        var jobSalaryInfoDto = new JobSalaryInfoDto(jobSalaryInfo.Minimum, jobSalaryInfo.Maximum,
            jobSalaryInfo.Currency, jobSalaryInfo.UnitOfTime, jobSalaryInfo.IsAfterTaxes);
        
        return jobSalaryInfoDto;
    }

    // public static JobCardDto ToJobCardDto(this Job job,
    //     JobSalaryInfoDto? jobSalaryInfoDto, string? avatarLink)
    // {
    //     
    // }
}