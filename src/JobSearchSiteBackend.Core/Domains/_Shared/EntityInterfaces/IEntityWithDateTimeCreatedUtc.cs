﻿namespace JobSearchSiteBackend.Core.Domains._Shared.EntityInterfaces;

public interface IEntityWithDateTimeCreatedUtc
{
    DateTime DateTimeCreatedUtc { get; }
}