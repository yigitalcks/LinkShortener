using System.Security.Claims;
using LinkShortener.Models;
using LinkShortener.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Base62;

namespace LinkShortener.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UrlController: ControllerBase
{
    private readonly ShortenedUrlService  _service;
    private readonly UserManager<IdentityUser> _userManager;

    public UrlController(ShortenedUrlService service, UserManager<IdentityUser> userManager)
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
        
        var keyBase62 = url.Key.ToBase62();
        var urlDTO = new ShortenedUrlResponseDTO
        {
            Key = keyBase62
        };
        
        return Ok(urlDTO);
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
            
            var url = new ShortenedUrl
            {
                Url = urlDTO.Url,
                UserId = userId,
                User = user
            };

            if (!string.IsNullOrEmpty(url.CustomUrl))
            {
                url.CustomUrl = urlDTO.CustomUrl;
                if (IsBase62(urlDTO.CustomUrl))
                {
                    try
                    {
                        long customKey = urlDTO.CustomUrl.FromBase62<long>();
                        
                        var existingUrl = await _service.GetUrlAsync(customKey);
                        if (existingUrl != null)
                        {
                            return BadRequest("Bu özel URL zaten kullanımda. Lütfen başka bir özel URL seçin.");
                        }
                    
                        // CustomUrl'i Key olarak ayarla
                        url.Key = customKey;
                    }
                    catch (Exception)
                    {
                        return BadRequest("Özel URL geçersiz bir formatta.");
                    }
                }
                    
            }
            
            await _service.SaveUrlAsync(url);

            var keyBase62 = url.Key.ToBase62();
            var shortenedUrl = new ShortenedUrlResponseDTO
            {
                Key = keyBase62
            };
            
            return CreatedAtAction(nameof(GetUrlByKey), new { key = url.Key }, shortenedUrl);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Sunucu hatası: {ex.Message}");
        }
    }

    public bool IsBase62(string customUrl)
    {
        if (customUrl.Length > 8)
            return false;
        
        return customUrl.All(c => 
            (c >= 'A' && c <= 'Z') || 
            (c >= 'a' && c <= 'z') || 
            (c >= '0' && c <= '9'));
    }
}