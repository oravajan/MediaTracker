using MediaTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediaTracker.Infrastructure.Data.Configurations;

internal class SeasonConfiguration : IEntityTypeConfiguration<Season>
{
    public void Configure(EntityTypeBuilder<Season> builder)
    {
        builder.ToTable("Season");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.SeasonNumber)
            .IsRequired();

        builder.HasMany(s => s.Episodes)
            .WithOne()
            .HasForeignKey("SeasonId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}