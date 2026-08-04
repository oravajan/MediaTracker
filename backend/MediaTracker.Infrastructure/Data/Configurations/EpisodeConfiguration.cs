using MediaTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediaTracker.Infrastructure.Data.Configurations;

internal class EpisodeConfiguration : IEntityTypeConfiguration<Episode>
{
    public void Configure(EntityTypeBuilder<Episode> builder)
    {
        builder.ToTable("Episode");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.EpisodeNumber)
            .IsRequired();

        builder.Property(e => e.Title)
            .IsRequired(false)
            .HasMaxLength(255);

        builder.Property(e => e.IsWatched)
            .IsRequired();
    }
}