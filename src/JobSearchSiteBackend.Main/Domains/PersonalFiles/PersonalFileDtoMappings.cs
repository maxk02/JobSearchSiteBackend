using JobSearchSiteBackend.Core.Domains.PersonalFiles.Dtos;

namespace JobSearchSiteBackend.Core.Domains.PersonalFiles;

public static class PersonalFileDtoMappings
{
    public static PersonalFileInfoDto ToPersonalFileInfoDto(this PersonalFile personalFile)
    {
        var personalFileInfoDto = new PersonalFileInfoDto(personalFile.Id,
            personalFile.Name, personalFile.Extension, personalFile.Size);
        
        return personalFileInfoDto;
    }
    
    public static PersonalFileTagDto ToPersonalFileTagDto(this PersonalFile personalFile)
    {
        var personalFileInfoDto = new PersonalFileTagDto(personalFile.Id,
            personalFile.Name, personalFile.Extension);
        
        return personalFileInfoDto;
    }
}