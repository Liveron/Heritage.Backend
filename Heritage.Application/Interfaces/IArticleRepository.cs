using Heritage.Application.Models;
using Heritage.Application.RequestFeatures;

namespace Heritage.Application.Interfaces;

public interface IArticleRepository
{
    public Task<Guid> CreateArticleAsync(Article article);
    public Task UpdateArticleAsync(Article article);
    public Task DeleteArticleAsync(Guid id);
    public Task<PagedList<Article>> GetAllArticlesAsync(ArticleParameters parameters, bool includePreview);
    public Task<Article> GetArticle(Guid id, bool includePreview);
    public Task SaveChangesAsync();
}
