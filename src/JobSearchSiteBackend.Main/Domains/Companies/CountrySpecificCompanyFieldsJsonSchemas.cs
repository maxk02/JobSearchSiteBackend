using JobSearchSiteBackend.Core.Domains.Countries.Enums;

namespace JobSearchSiteBackend.Core.Domains.Companies;

public static class CountrySpecificCompanyFieldsJsonSchemas
{
    private static string Poland => """
                                    {
                                     'type': 'object',
                                     'properties': {
                                         'nip': {'type': 'string'},
                                     },
                                     'required': ['nip'],
                                     'additionalProperties': false
                                    }
                                    """;

    public static string GetForCountryId(long countryId) => countryId switch
    {
        (long)CountryIdsEnum.Poland => Poland,
        _ => throw new ArgumentOutOfRangeException(nameof(countryId), countryId, null)
    };
}