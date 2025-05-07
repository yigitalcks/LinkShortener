using LinkShortener.Models;

namespace LinkShortener.Services;

public interface IShortenedUrlService
{
    Task<ShortenedUrl?> GetUrlAsync(long key);
    Task<ShortenedUrl?> GetUrlAsync(ulong key);
    Task<ShortenedUrl?> GetUrlAsync(string key, bool isExtended = false);
    
    Task<ShortenedUrl?> SaveUrlAsync(ShortenedUrl shortenedUrl);
}