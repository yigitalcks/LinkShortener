using System.Security.Claims;
using LinkShortener.Models;
using LinkShortener.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

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
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ShortenedUrl>>> GetUrls()
    {
        var urls = await _service.GetUrlsAsync();
        if (urls.Any() == false)
        {
            return NotFound();
        }
        return Ok(urls);
    }

    [HttpGet("{key}")]
    public async Task<ActionResult<ShortenedUrl>> GetUrlByKey(long key)
    {
        var url = await _service.GetUrlAsync(key);
        if (url == null)
        {
            return NotFound();
        }
        return Ok(url);
    }

    
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ShortenedUrl>> SaveUrl([FromBody] ShortenedUrlDTO urlDTO)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            
            var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
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
            
            return CreatedAtAction(nameof(GetUrlByKey), new { key = url.Key }, url);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Sunucu hatası: {ex.Message}");
        }
    }
}