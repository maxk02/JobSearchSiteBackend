namespace JobSearchSiteBackend.Infrastructure.BackgroundJobs;

public static class BackgroundJobQueues
{
    public static readonly string Default = nameof(Default);
    public static readonly string CompanySearch = nameof(CompanySearch);
    public static readonly string CvSearch = nameof(CvSearch);
    public static readonly string JobSearch = nameof(JobSearch);
    public static readonly string LocationSearch = nameof(LocationSearch);
    public static readonly string PersonalFileTextExtractionAndSearch = nameof(PersonalFileTextExtractionAndSearch);
    public static readonly string PersonalFileStorage = nameof(PersonalFileStorage);
    public static readonly string EmailSending = nameof(EmailSending);

    public static readonly string[] AllValues =
    [
        JobSearch,
        CompanySearch,
        CvSearch,
        LocationSearch,
        EmailSending,
        PersonalFileTextExtractionAndSearch,
        PersonalFileStorage,
        Default
    ];
}