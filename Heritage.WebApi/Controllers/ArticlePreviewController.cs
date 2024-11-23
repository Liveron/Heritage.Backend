using Heritage.Application.DataTransferObjects;
using Heritage.Application.RequestFeatures;
using Heritage.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Heritage.WebApi.Controllers;

[Route("api/articlePreviews")]
[ApiController]
public class ArticlePreviewController(IArticlePreviewService service) : ControllerBase
{
    private readonly IArticlePreviewService _service = service;

    [HttpGet(Name = "GetAllArticlePreviews")]
    public async Task<IActionResult> GetArticlePreviews([FromQuery] ArticlePreviewParameters parameters)
    {
        var (previews, metaData) = await _service.GetAllArticlePreviews(parameters);

        Response.Headers["X-Pagination"] = JsonSerializer.Serialize(metaData);

        return Ok(previews);
    }

    [HttpGet("{id:guid}", Name = "GetArticlePreview")]
    public async Task<IActionResult> GetArticlePreview(Guid id)
    {
        ArticlePreviewDto preview = await _service.GetArticlePreview(id);

        return Ok(preview);
    }

    [HttpPost(Name = "CreateArticlePreview")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> CreateArticlePreview([FromQuery] ArticlePreviewParameters parameters,
        [FromBody] CreateArticlePreviewDto previewDto)
    {
        if (previewDto == null)
            return BadRequest("Article preivew object is null.");

        if (parameters.ArticleId == Guid.Empty)
            return NotFound();

        Guid id = await _service.CreateArticlePreview(parameters.ArticleId, previewDto);

        return Ok(id);
    }

    [HttpPut("{id:guid}", Name = "UpdateArticlePreview")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> UpdateArticlePreview(Guid id, [FromBody] UpdateArticlePreviewDto previewDto)
    {
        if (previewDto is null)
            return BadRequest("Article preview object is null");

        await _service.UpdateArticlePreview(previewDto, id);

        return NoContent();
    }

    [HttpDelete("{id:guid}", Name = "DeleteArticlePreview")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteArticlePreview(Guid id)
    {
        await _service.DeleteArticlePreview(id);

        return NoContent();
    }
}