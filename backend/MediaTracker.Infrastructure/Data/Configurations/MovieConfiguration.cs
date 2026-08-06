using MediaTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediaTracker.Infrastructure.Data.Configurations;

internal class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.HasOne(m => m.NextMovie)
            .WithMany()
            .HasForeignKey(m => m.NextMovieId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.Property(m => m.IsWatched)
            .IsRequired();
    }
}