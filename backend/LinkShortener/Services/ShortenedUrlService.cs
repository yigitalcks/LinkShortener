using System.Text;
using LinkShortener.Data;
using Microsoft.EntityFrameworkCore;
using Base62;
using LinkShortener.Utilities;
using Standart.Hash.xxHash;

namespace LinkShortener.Services;

using LinkShortener.Models;

public class ShortenedUrlService : IShortenedUrlService
{
    private readonly ApplicationDbContext _context;

    public ShortenedUrlService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ShortenedUrl?> GetUrlAsync(long key)
    {
        return await _context.ShortenedUrls
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Key == key);
    }
    
    public async Task<ShortenedUrl?> GetUrlAsync(ulong key)
    {
        return await _context.ShortenedUrls
            .AsNoTracking()
            .FirstOrDefaultAsync(u => (ulong)u.Key == key);
    }
    
    public async Task<ShortenedUrl?> GetUrlAsync(string key, bool isExtended = false)
    {
        long? keyLong;
        if (isExtended)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(key);
            var hashedKey = await xxHash64.ComputeHashAsync(new MemoryStream(bytes));
            hashedKey = hashedKey & 0x7FFFFFFFFFFFFFFF;
            hashedKey = hashedKey + Base62Validator.Pow62_8;
            
            keyLong = (long)hashedKey;
        }
        else
        {
            keyLong = key.FromBase62();
        }
        
        return await _context.ShortenedUrls
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Key == keyLong);
    }
    
    public async Task<ShortenedUrl?> SaveUrlAsync(ShortenedUrl shortenedUrl)
    {
        await _context.ShortenedUrls.AddAsync(shortenedUrl);
        await _context.SaveChangesAsync();
        
        return shortenedUrl;
    }

    public async Task<bool> UpdateUrlAsync(ShortenedUrl shortenedUrl)
    {
        _context.ShortenedUrls.Update(shortenedUrl);
        return await (_context.SaveChangesAsync()) > 0;
    }
    public async Task<List<ShortenedUrlResponseDTO>> GetLast10EntriesAsync(string userId)
    {
        return await _context.ShortenedUrls
            .AsNoTracking()
            .Where(u => u.UserId == userId)
            .OrderByDescending(u => u.CreatedAt)
            .Take(10)
            .Select(u => new ShortenedUrlResponseDTO
            {
                Key = u.ShortUrl,
                CreatedAt = u.CreatedAt
            })
            .ToListAsync();
    }
}