using Microsoft.AspNetCore.Mvc;
using LinkShortener.Models;
using LinkShortener.Services;

using Base62;

namespace LinkShortener.Controllers;

[ApiController]
public class RedirectController : ControllerBase
{
    private readonly ShortenedUrlService  _service;
    
    
    public RedirectController(ShortenedUrlService service)
    {
        _service = service;
    }
    
    [HttpGet("{key}")]
    public async Task<IActionResult> RedirectToUrl(string key)
    {
        var keyDecimal = key.FromBase62<long>();
        var url = await _service.GetUrlAsync(keyDecimal);
            
        if (url == null)
            return NotFound("Geçersiz bağlantı.");
        
        return Redirect(url.Url); 
    }
}