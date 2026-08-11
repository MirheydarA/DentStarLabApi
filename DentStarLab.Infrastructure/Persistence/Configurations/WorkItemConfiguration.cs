using DentStarLab.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DentStarLab.Infrastructure.Persistence.Configurations;
public class WorkItemConfiguration : IEntityTypeConfiguration<WorkItem>
{
    public void Configure(EntityTypeBuilder<WorkItem> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ToothCount)
            .IsRequired();

        builder.Property(x => x.UnitPrice)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.TotalAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.HasOne(x => x.Work)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.WorkId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.WorkType)
            .WithMany(x => x.WorkItems)
            .HasForeignKey(x => x.WorkTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}