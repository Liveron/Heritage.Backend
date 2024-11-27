using Heritage.Application.DataTransferObjects;
using Heritage.Application.Interfaces;
using Heritage.Application.Models;
using Heritage.Application.RequestFeatures;
using Mapster;

namespace Heritage.Application.Services;

public interface IArticlePreviewService
{
    public Task<(List<ArticlePreviewDto> previews, MetaData metaData)> GetAllArticlePreviews(
        ArticlePreviewParameters parameters);
    public Task<ArticlePreviewDto> GetArticlePreview(Guid id);
    public Task<Guid> CreateArticlePreview(Guid articleId, CreateArticlePreviewDto preview);
    public Task UpdateArticlePreview(UpdateArticlePreviewDto previewDto, Guid id);
    public Task DeleteArticlePreview(Guid id);
}

public class ArticlePreviewService(IArticlePreviewRepository repository) : IArticlePreviewService
{
    private readonly IArticlePreviewRepository _repository = repository;

    public async Task<Guid> CreateArticlePreview(Guid articleId, CreateArticlePreviewDto previewDto)
    {
        ArticlePreview article = previewDto.BuildAdapter()
            .AddParameters("id", articleId)
            .AdaptToType<ArticlePreview>();

        Guid id = await _repository.CreateArticlePreviewAsync(article);
        await _repository.SaveChangesAsync();
        return id;
    }

    public async Task<(List<ArticlePreviewDto> previews, MetaData metaData)> GetAllArticlePreviews(
        ArticlePreviewParameters parameters)
    {
        PagedList<ArticlePreview> previewsWithMetaData = await _repository.GetAllArticlePreviewsAsync(parameters);
        List<ArticlePreviewDto> previews = previewsWithMetaData.Adapt<List<ArticlePreviewDto>>();
        return (previews, metaData: previewsWithMetaData.MetaData);
    }

    public async Task<ArticlePreviewDto> GetArticlePreview(Guid id)
    {
        ArticlePreview previewDb = await _repository.GetArticlePreviewAsync(id);
        ArticlePreviewDto previewDto = previewDb.Adapt<ArticlePreviewDto>();
        return previewDto;
    }

    public async Task UpdateArticlePreview(UpdateArticlePreviewDto previewDto, Guid id)
    {
        ArticlePreview preview = previewDto.BuildAdapter()
            .AddParameters("id", id)
            .AdaptToType<ArticlePreview>();

        await _repository.UpdateArticlePreviewAsync(preview);
        await _repository.SaveChangesAsync();
    }

    public async Task DeleteArticlePreview(Guid id)
    {
        await _repository.DeleteArticlePreviewAsync(id);
        await _repository.SaveChangesAsync();
    }
}
