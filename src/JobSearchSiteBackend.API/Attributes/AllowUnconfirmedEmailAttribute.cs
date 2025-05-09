namespace JobSearchSiteBackend.API.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class AllowUnconfirmedEmailAttribute : Attribute
{
}