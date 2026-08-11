using DentStarLab.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DentStarLab.Infrastructure.Persistence.Configurations;
public class WorkConfiguration : IEntityTypeConfiguration<Work>
{
    public void Configure(EntityTypeBuilder<Work> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.PatientName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.WorkDate)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.HasOne(x => x.Doctor)
            .WithMany(x => x.Works)
            .HasForeignKey(x => x.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}