namespace API.Domains._Shared.ValueEntities;

public record EmploymentTypeRecord(bool IsPartTime, bool IsFullTime, bool IsOnSite, 
    bool IsRemote, bool IsHybrid, bool IsMobile);