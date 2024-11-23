using Heritage.Application.Common.Exceptions;
using Heritage.Application.DataTransferObjects;
using Heritage.Application.Interfaces;
using Heritage.Application.Models;
using Heritage.Application.RequestFeatures;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Heritage.Persistance.Repositories;

public class ArticlePreviewRepository(HeritageDbContext dbContext) : IArticlePreviewRepository
{
    private readonly HeritageDbContext _dbContext = dbContext;

    public async Task<Guid> CreateArticlePreviewAsync(ArticlePreview preview)
    {
        await _dbContext.ArticlePreviews.AddAsync(preview);
        return preview.ArticleId;
    }

    public async Task DeleteArticlePreviewAsync(Guid id)
    {
        ArticlePreview preview = await _dbContext.ArticlePreviews.FindAsync(id) ??
            throw new ArticlePreviewNotFoundException(id);
        _dbContext.ArticlePreviews.Remove(preview);
    }

    public async Task<PagedList<ArticlePreview>> GetAllArticlePreviewsAsync(ArticlePreviewParameters parameters)
    {
        List<ArticlePreview> previews = await _dbContext.ArticlePreviews.AsNoTracking()
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync();

        int count = await _dbContext.ArticlePreviews.AsNoTracking()
            .CountAsync();

        return new PagedList<ArticlePreview>(
            previews, count, parameters.PageNumber, parameters.PageSize);
    }

    public async Task<ArticlePreview> GetArticlePreviewAsync(Guid id)
    {
        ArticlePreview articlePreview =
            await _dbContext.ArticlePreviews.AsNoTracking()
            .FirstOrDefaultAsync(preview => preview.ArticleId == id) ??
            throw new ArticleNotFoundException(id);

        return articlePreview;
    }

    public async Task UpdateArticlePreviewAsync(ArticlePreview preview)
    {
        ArticlePreview previewDb = await _dbContext.ArticlePreviews.FindAsync(preview.ArticleId) ??
            throw new ArticlePreviewNotFoundException(preview.ArticleId);

        preview.Adapt(previewDb);
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
