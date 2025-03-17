using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using LinkShortener.Models;

namespace LinkShortener.Data;
public class ShortenedUrlContext : IdentityDbContext<IdentityUser>
{
    public ShortenedUrlContext(DbContextOptions<ShortenedUrlContext> options)
        : base(options)
    {
    }
        
    public DbSet<ShortenedUrl> ShortenedUrls { get; set; }
        
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ShortenedUrl>()
            .Property(u => u.Key)
            .UseIdentityAlwaysColumn();
        
        modelBuilder.Entity<ShortenedUrl>()
            .HasOne<IdentityUser>(s => s.User)
            .WithMany() 
            .HasForeignKey(s => s.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}