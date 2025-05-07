using System.Text;
using Microsoft.AspNetCore.Mvc;
using LinkShortener.Models;
using LinkShortener.Services;
using LinkShortener.Utilities;
using Standart.Hash.xxHash;

namespace LinkShortener.Controllers;

[ApiController]
public class RedirectController : ControllerBase
{
    private readonly IShortenedUrlService  _service;
    
    public RedirectController(IShortenedUrlService service)
    {
        _service = service;
    }
    
    [HttpGet("{key}")]
    public async Task<IActionResult> RedirectToUrl(string key)
    {
        bool isExtended = !Base62Validator.IsValidBase62(key);
        
        var url = await _service.GetUrlAsync(key, isExtended);
            
        if (url == null)
            return NotFound("Geçersiz bağlantı.");
        
        return Redirect(url.Url); 
    }
}