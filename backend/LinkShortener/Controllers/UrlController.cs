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
}