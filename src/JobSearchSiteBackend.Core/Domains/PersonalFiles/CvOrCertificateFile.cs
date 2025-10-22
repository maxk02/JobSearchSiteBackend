namespace JobSearchSiteBackend.Core.Domains.PersonalFiles;

public class CvOrCertificateFile : PersonalFile
{
    public CvOrCertificateFile(long userId, string name, string extension, long size, string text)
        : base(userId, name, extension, size)
    {
        Text = text;
    }
    
    public string Text { get; set; }
}