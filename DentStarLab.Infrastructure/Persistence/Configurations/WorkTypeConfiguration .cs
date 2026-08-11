using DentStarLab.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DentStarLab.Infrastructure.Persistence.Configurations;
public class WorkTypeConfiguration : IEntityTypeConfiguration<WorkType>
{
    public void Configure(EntityTypeBuilder<WorkType> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.PricePerTooth)
            .HasPrecision(18, 2);

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();
    }
}