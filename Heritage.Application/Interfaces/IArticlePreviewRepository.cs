using Heritage.Application.DataTransferObjects;
using Heritage.Application.Models;
using Heritage.Application.RequestFeatures;

namespace Heritage.Application.Interfaces;

public interface IArticlePreviewRepository
{
    public Task<Guid> CreateArticlePreviewAsync(ArticlePreview articlePreview);
    public Task UpdateArticlePreviewAsync(ArticlePreview articlePreview);
    public Task DeleteArticlePreviewAsync(Guid id);
    public Task<ArticlePreview> GetArticlePreviewAsync(Guid id);
    public Task<PagedList<ArticlePreview>> GetAllArticlePreviewsAsync(ArticlePreviewParameters parameters);
    public Task SaveChangesAsync();
}
