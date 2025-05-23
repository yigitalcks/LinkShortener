using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LinkShortener.Models;

namespace LinkShortener.Data;
public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<ShortenedUrl> ShortenedUrls { get; set; }
        
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ShortenedUrl>()
            .Property(u => u.Key)
            .UseIdentityByDefaultColumn();

        modelBuilder.Entity<ShortenedUrl>()
            .Property(u => u.CreatedAt)
            .HasDefaultValueSql("now()");

        modelBuilder.Entity<ShortenedUrl>()
            .HasOne<IdentityUser>(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<ShortenedUrl>()
            .HasIndex(u => u.UserId);
        modelBuilder.Entity<ShortenedUrl>()
            .HasIndex(u => u.CreatedAt);
    }
}