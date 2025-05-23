using System.Security.Claims;
using System.Text;
using LinkShortener.Models;
using LinkShortener.Utilities;
using LinkShortener.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Base62;
using Standart.Hash.xxHash;

namespace LinkShortener.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UrlController: ControllerBase
{
    private readonly IShortenedUrlService  _service;
    private readonly UserManager<IdentityUser> _userManager;

    public UrlController(IShortenedUrlService service, UserManager<IdentityUser> userManager)
    {
        _service = service;
        _userManager = userManager;
    }
    
    [HttpGet("detail/{key:long}")]
    [Authorize]
    public async Task<ActionResult<ShortenedUrlResponseDTO>> GetUrlByKey(long key)
    {
        var url = await _service.GetUrlAsync(key);
        if (url == null)
        {
            return NotFound();
        }
        
        return Ok(url);
    }

    [HttpGet("history")]
    [Authorize]
    public async Task<ActionResult<List<ShortenedUrlResponseDTO>>> GetUrlHistory()
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Kullanıcı doğrulaması başarısız.");
            
            var user = await _userManager.FindByIdAsync(userId);
            if(user == null)
                return Unauthorized("Kullanıcı bulunamadı.");
            
            var entries = await _service.GetLast10EntriesAsync(userId);

            if (!entries.Any())
            {
                return Ok(new List<ShortenedUrlResponseDTO>());
            }

            return Ok(entries);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Sunucu hatası: {ex.Message}");
        }
    }
    
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ShortenedUrl>> SaveUrl([FromBody] ShortenedUrlRequestDTO urlDTO)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Kullanıcı doğrulaması başarısız.");
            
            var user = await _userManager.FindByIdAsync(userId);
            if(user == null)
                return Unauthorized("Kullanıcı bulunamadı.");

            long? key = null;
            bool isBase62 = true;
            if (!string.IsNullOrEmpty(urlDTO.CustomKey))
            {
                isBase62 = Base62Validator.IsValidBase62(urlDTO.CustomKey);
                if (isBase62)
                {
                    var isExist = await _service.GetUrlAsync(urlDTO.CustomKey);
                    if (isExist != null)
                    {
                        return Conflict("Bu Custom URL zaten kullanılıyor.");
                    }
                    key = urlDTO.CustomKey.FromBase62();
                }
                else
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(urlDTO.CustomKey);
                    var hashedKey = await xxHash64.ComputeHashAsync(new MemoryStream(bytes));
                    hashedKey = hashedKey & 0x7FFFFFFFFFFFFFFF;
                    hashedKey = hashedKey + Base62Validator.Pow62_8;
                    
                    var isExist = await _service.GetUrlAsync(hashedKey);
                    if (isExist != null)
                    {
                        return Conflict("Bu Custom URL zaten kullanılıyor.");
                    }
                    
                    key = (long)hashedKey;
                }
            }
            
            var url = new ShortenedUrl
            {
                Url = urlDTO.Url,
                UserId = userId,
                User = user
            };
            if (key.HasValue)
            {
                url.Key = key.Value;
            }
            
            await _service.SaveUrlAsync(url);
            
            var keyBase62 = isBase62 ? url.Key.ToBase62(): urlDTO.CustomKey;
            url.ShortUrl = keyBase62;

            await _service.UpdateUrlAsync(url);
            
            var shortenedUrl = new ShortenedUrlResponseDTO
            {
                Key = url.ShortUrl,
                CreatedAt = url.CreatedAt
            };
            
            return CreatedAtAction(nameof(GetUrlByKey), new { key = url.Key }, shortenedUrl);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Sunucu hatası: {ex.Message}");
        }
    }
}