using MediaTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediaTracker.Infrastructure.Data.Configurations;

internal class TvShowConfiguration : IEntityTypeConfiguration<TvShow>
{
    public void Configure(EntityTypeBuilder<TvShow> builder)
    {
        builder.HasMany(t => t.Seasons)
            .WithOne()
            .HasForeignKey("TvShowId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}