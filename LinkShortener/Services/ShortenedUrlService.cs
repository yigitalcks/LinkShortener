using LinkShortener.Data;
using Microsoft.EntityFrameworkCore;

namespace LinkShortener.Services;

using LinkShortener.Models;

public class ShortenedUrlService
{
    private readonly ShortenedUrlContext _context;

    public ShortenedUrlService(ShortenedUrlContext context)
    {
        _context = context;
    }
    
    /////////////////////TEST AMAÇLI//////////////////////////////
    public async Task<IEnumerable<ShortenedUrl>> GetUrlsAsync()
    {
        return await _context.ShortenedUrls.AsNoTracking().Take(10).ToListAsync();
    }
    ///////////////////////////////////////////////////

    public async Task<ShortenedUrl?> GetUrlAsync(long key)
    {
        return await _context.ShortenedUrls
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Key == key);
    }

    public async Task<ShortenedUrl?> SaveUrlAsync(ShortenedUrl shortenedUrl)
    {
        await _context.ShortenedUrls.AddAsync(shortenedUrl);
        await _context.SaveChangesAsync();
        
        return shortenedUrl;
    }
}