using Microsoft.EntityFrameworkCore;
using LinkShortener.Models;

namespace LinkShortener.Data;

public class ShortenedUrlContext: DbContext
{
    public ShortenedUrlContext(DbContextOptions<ShortenedUrlContext> options) 
        : base(options)
    {
    }
    
    public DbSet<ShortenedUrl> ShortenedUrls { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShortenedUrl>()
            .HasKey(u => u.Key);
        
        modelBuilder.Entity<ShortenedUrl>()
            .Property(u => u.Key)
            .UseIdentityAlwaysColumn();

        modelBuilder.Entity<ShortenedUrl>()
            .HasOne(u => u.User)
            .WithMany(u => u.Urls)
            .HasForeignKey(u => u.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
        
}