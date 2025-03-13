using LinkShortener.Models;
using LinkShortener.Services;
using Microsoft.AspNetCore.Mvc;

namespace LinkShortener.Controllers;

[ApiController]
[Route("[controller]")]
public class UrlController: ControllerBase
{
    private readonly ShortenedUrlService  _service;

    public UrlController(ShortenedUrlService service)
    {
        _service = service;
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
    public async Task<ActionResult<ShortenedUrl>> SaveUrl(ShortenedUrl url)
    {
        await _service.SaveUrlAsync(url);
        return CreatedAtAction(nameof(GetUrlByKey), new { key = url.Key }, url);
    }
    
    // USER-----------------------------
    [HttpGet("users/")]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        var users = await _service.GetUsersAsync();
        if (users.Any() == false)
        {
            return NotFound();
        }
        return Ok(users);
    }
    [HttpGet("users/{id}")]
    public async Task<ActionResult<User>> GetUserById(int id)
    {
        var user =  await _service.GetUserAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }
}