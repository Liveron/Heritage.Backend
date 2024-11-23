using Heritage.Application.DataShaping;
using Heritage.Application.DataTransferObjects;
using Heritage.Application.Interfaces;
using Heritage.Application.Models;
using Heritage.Application.RequestFeatures;
using Mapster;
using System.Dynamic;

namespace Heritage.Application.Services;

public interface IArticleService
{
    public Task UpdateArticle(UpdateArticleDto articleDto, Guid id);
    public Task DeleteArticle(Guid id);
    public Task<Guid> CreateArticle(CreateArticleDto articleDto);
    public Task<(IEnumerable<ExpandoObject> articles, MetaData metaData)> GetAllArticles(ArticleParameters parameters);
    public Task<ArticleDto> GetArticle(Guid id, bool includePreview);
}

public class ArticleService(IArticleRepository articleRepository, IDataShaper<ArticleDto> dataShaper) : IArticleService
{
    private readonly IArticleRepository _articleRepository = articleRepository;
    private readonly IDataShaper<ArticleDto> _dataShaper = dataShaper;

    public async Task<Guid> CreateArticle(CreateArticleDto articleDto)
    {
        Article article = articleDto.Adapt<Article>();
        Guid id = await _articleRepository.CreateArticleAsync(article);
        await _articleRepository.SaveChangesAsync();
        return id;
    }

    public async Task DeleteArticle(Guid id)
    {
        await _articleRepository.DeleteArticleAsync(id);
        await _articleRepository.SaveChangesAsync();
    }

    public async Task<(IEnumerable<ExpandoObject> articles, MetaData metaData)> GetAllArticles(
        ArticleParameters parameters)
    {
        bool includePreview = parameters.Fields?.Contains("preview") ?? true;

        PagedList<Article> articlesWithMetaData = 
            await _articleRepository.GetAllArticlesAsync(parameters, includePreview: includePreview);

        List<ArticleDto> articlesDto = articlesWithMetaData.Adapt<List<ArticleDto>>();

        IEnumerable<ExpandoObject> articles = _dataShaper.ShapeData(articlesDto, parameters.Fields);

        return (articles, metaData: articlesWithMetaData.MetaData);
    }

    public async Task<ArticleDto> GetArticle(Guid id, bool includePreview)
    {
        Article articleDb = await _articleRepository.GetArticle(id, includePreview);
        return articleDb.Adapt<ArticleDto>();
    }

    public async Task UpdateArticle(UpdateArticleDto articleDto, Guid id)
    {
        Article article = articleDto.BuildAdapter()
            .AddParameters("id", id)
            .AdaptToType<Article>();

        await _articleRepository.UpdateArticleAsync(article);
        await _articleRepository.SaveChangesAsync();
    }
}
