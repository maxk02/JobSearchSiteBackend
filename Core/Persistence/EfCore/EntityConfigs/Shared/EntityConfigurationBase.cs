using Core.Domains._Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EfCore.Context.EntityConfigs.Shared;

public class EntityConfigurationBase<T> : IEntityTypeConfiguration<T> where T : EntityBase
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder
            .HasKey(entity => entity.Id);
    }
}