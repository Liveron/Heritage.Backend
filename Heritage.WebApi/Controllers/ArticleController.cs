using Heritage.Application.DataTransferObjects;
using Heritage.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Heritage.Application.RequestFeatures;
using System.Text.Json;
using Heritage.Application.Models;

namespace Heritage.WebApi.Controllers;

[Route("api/articles")]
[ApiController]
public class ArticleController(IArticleService service) : ControllerBase
{
    private readonly IArticleService _service = service;

    [HttpPost(Name = "CreateArticle")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> CreateArticle([FromBody] CreateArticleDto article)
    {
        if (article is null)
            return BadRequest("Article object is null");

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        Guid id = await _service.CreateArticle(article);

        return CreatedAtRoute("GetArticle", new { id }, article);
    }

    [HttpGet(Name = "GetAllArticles")]
    public async Task<IActionResult> GetArticles([FromQuery] ArticleParameters parameters)
    {
        var (articles, metaData) = await _service.GetAllArticles(parameters);

        Response.Headers["X-Pagination"] = JsonSerializer.Serialize(metaData);

        return Ok(articles);
    }

    [HttpGet("{id:guid}", Name = "GetArticle")]
    public async Task<IActionResult> GetArticle(Guid id)
    {
        ArticleDto article = await _service.GetArticle(id, includePreview: true);

        return Ok(article);
    }

    [HttpPut("{id:guid}", Name = "UpdateArticle")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> UpdateArticle(Guid id, [FromBody] UpdateArticleDto updateArticleDto)
    {
        if (updateArticleDto is null)
            return BadRequest("Article object is null");

        await _service.UpdateArticle(updateArticleDto, id);

        return NoContent();
    }

    [HttpDelete("{id:guid}", Name = "DeleteArticle")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteArticle(Guid id)
    {
        await _service.DeleteArticle(id);

        return NoContent();
    }
}
