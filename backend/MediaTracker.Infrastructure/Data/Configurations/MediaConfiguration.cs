using MediaTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediaTracker.Infrastructure.Data.Configurations;

internal class MediaConfiguration : IEntityTypeConfiguration<Media>
{
    public void Configure(EntityTypeBuilder<Media> builder)
    {
        builder.ToTable("Media");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Title)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(m => m.UserRating)
            .IsRequired(false);

        builder.HasDiscriminator<string>("MediaType")
            .HasValue<Movie>("Movie")
            .HasValue<TvShow>("TvShow");

        builder.Property("MediaType")
            .IsRequired();
    }
}