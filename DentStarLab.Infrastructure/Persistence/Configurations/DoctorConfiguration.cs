using DentStarLab.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DentStarLab.Infrastructure.Persistence.Configurations;
public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Surname)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Phone)
            .HasMaxLength(50);

        builder.Property(x => x.Email)
            .HasMaxLength(200);

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();
    }
}