namespace JobSearchSiteBackend.Shared.FluentValidationAddons.StringFiltering;

public class CustomStringPolicy
{
    protected static readonly NullReferenceException ValueNullException = 
        new("This function is not intended to run with Value not set.");
    
    protected readonly List<Action> MethodCalls = [];
    public string? Value { get; private set; }
    
    //https://www.regular-expressions.info/unicode.html
    
    protected virtual void RunPreValidationsOperations() { }
    
    private void PerformChainedValidations()
    {
        foreach (var fun in MethodCalls)
        {
            fun();
        }
    }
    
    public void Check(string value)
    {
        Value = value;
        
        RunPreValidationsOperations();
        
        PerformChainedValidations();
    }
}
