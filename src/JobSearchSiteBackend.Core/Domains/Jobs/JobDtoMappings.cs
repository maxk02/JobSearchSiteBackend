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
    
    public static JobSalaryInfo ToJobSalaryInfo(this JobSalaryInfoDto jobSalaryInfoDto, long jobId = 0)
    {
        var jobSalaryInfo = new JobSalaryInfo(jobId, jobSalaryInfoDto.Minimum, jobSalaryInfoDto.Maximum,
            jobSalaryInfoDto.Currency, jobSalaryInfoDto.UnitOfTime, jobSalaryInfoDto.IsAfterTaxes);
        
        return jobSalaryInfo;
    }

    // public static JobCardDto ToJobCardDto(this Job job,
    //     JobSalaryInfoDto? jobSalaryInfoDto, string? avatarLink)
    // {
    //     
    // }
}