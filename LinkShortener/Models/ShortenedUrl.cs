namespace LinkShortener.Models;

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
    
    [Required]
    public User User { get; set; }
    public int UserId { get; set; }
}