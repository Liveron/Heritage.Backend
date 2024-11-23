using Heritage.Application.Common.Exceptions;
using Heritage.Application.Interfaces;
using Heritage.Application.Models;
using Microsoft.EntityFrameworkCore;
using Mapster;
using Heritage.Application.RequestFeatures;

namespace Heritage.Persistance.Repositories;

public class ArticleRepository(HeritageDbContext dbContext) : IArticleRepository
{
    readonly HeritageDbContext _dbContext = dbContext;

    static ArticleRepository()
    {
        TypeAdapterConfig<Article, Article>.NewConfig()
            .Ignore(dest => dest.Id);

        TypeAdapterConfig<ArticlePreview, ArticlePreview>.NewConfig()
            .Ignore(dest => dest.ArticleId);
    }

    public async Task<Guid> CreateArticleAsync(Article article)
    {
        await _dbContext.AddAsync(article);
        return article.Id;
    }

    public async Task DeleteArticleAsync(Guid id)
    {
        Article article = await _dbContext.Articles.AsNoTracking()
            .FirstOrDefaultAsync(article => article.Id == id) ??
            throw new ArticleNotFoundException(id);
        _dbContext.Remove(article);
    }

    public async Task<PagedList<Article>> GetAllArticlesAsync(ArticleParameters parameters, bool includePreview)
    {
        IQueryable<Article> query = _dbContext.Articles.AsNoTracking();

        if (includePreview)
            query = query.Include(article => article.Preview);

        List<Article> articles = await query.Skip(parameters.PageSize * (parameters.PageNumber - 1))
            .Take(parameters.PageSize)
            .ToListAsync();

        int count = await _dbContext.Articles.CountAsync();

        return new PagedList<Article>(articles, count, parameters.PageNumber, parameters.PageSize);
    }

    public async Task<Article> GetArticle(Guid id, bool includePreview)
    {
        IQueryable<Article> query = _dbContext.Articles.AsNoTracking();

        if (includePreview)
            query = query.Include(article => article.Preview);

        Article article = await query.FirstOrDefaultAsync(x => x.Id == id) ??
            throw new ArticleNotFoundException(id);

        return article;
    }

    public async Task UpdateArticleAsync(Article article)
    {
        _ = await _dbContext.Articles.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == article.Id)
            ?? throw new ArticleNotFoundException(article.Id);

        _dbContext.Entry(article).State = EntityState.Modified;
        _dbContext.Entry(article.Preview!).State = EntityState.Modified;
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
