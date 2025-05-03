namespace LinkShortener.Models;

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ShortenedUrl
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Key { get; private set; }
    
    [Required]
    [MaxLength(2048)]
    public string Url { get; set; }
    
    [StringLength(30, MinimumLength = 3, ErrorMessage = "Custom URL must be between 3 and 30 characters")]
    public string? CustomUrl { get; set; }
    
    [Required]
    public string UserId { get; set; }
        
    [ForeignKey("UserId")]
    public virtual IdentityUser User { get; set; }
}

public class ShortenedUrlRequestDTO
{
    [Required]
    [MaxLength(2048)]
    public string Url { get; set; }
    
    [StringLength(30, MinimumLength = 3, ErrorMessage = "Custom URL must be between 3 and 30 characters")]
    public string? CustomUrl { get; set; }
}

public class ShortenedUrlResponseDTO
{
    public string Key { get; set; }
}