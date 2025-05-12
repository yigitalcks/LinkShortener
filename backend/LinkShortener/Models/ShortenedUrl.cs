namespace LinkShortener.Models;

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ShortenedUrl
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Key { get; set; }
    
    [StringLength(16)]
    public string? ShortUrl { get; set; }
    
    [Required]
    [MaxLength(2048)]
    public string Url { get; set; }
    
    [Required]
    public string UserId { get; set; }
        
    [ForeignKey("UserId")]
    public virtual IdentityUser User { get; set; }
    
    [Required]
    public DateTime CreatedAt { get; private set; }
}

public class ShortenedUrlRequestDTO
{
    [Required]
    [MaxLength(2048)]
    public string Url { get; set; }
    
    [StringLength(16, MinimumLength = 3, ErrorMessage = "Custom URL must be between 3 and 16 characters")]
    public string? CustomKey { get; set; }
}

public class ShortenedUrlResponseDTO
{
    public string Key { get; set; }
    public DateTime CreatedAt { get; set; }
}