namespace JobSearchSiteBackend.Shared.FluentValidationAddons.StringFiltering;

public class BlacklistPolicy : CustomStringPolicy
{
    public HashSet<char> BlacklistedCharsFound { get; } = [];
    public HashSet<string> ForbiddenCharTypes { get; private set; } = [];
    
    //https://www.regular-expressions.info/unicode.html
    
    private void ExecuteForbidCustomChars(IEnumerable<char> customChars)
    {
        if (Value is null)
            throw ValueNullException;
        
        foreach (var x in customChars)
        {
            if (Value.Contains(x)) 
                BlacklistedCharsFound.Add(x);
        }
    }
    public BlacklistPolicy CustomChars(IEnumerable<char> customChars)
    {
        MethodCalls.Add(() => ExecuteForbidCustomChars(customChars));
        
        foreach (var x in customChars)
        {
            ForbiddenCharTypes.Add(x.ToString());
        }
        
        return this;
    }
    
    
    protected override void RunPreValidationsOperations()
    {
        if (Value is null)
            throw ValueNullException;
        
        BlacklistedCharsFound.Clear();
    }
}