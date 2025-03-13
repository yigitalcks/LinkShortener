using System.Text.Json.Serialization;

namespace LinkShortener.Models;

public class User
{
    public  int Id { get; set; }
    
    [JsonIgnore]
    public ICollection<ShortenedUrl>? Urls { get; set; }
}   