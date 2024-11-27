namespace Heritage.Application.Common.Exceptions;

public class ArticleNotFoundException(Guid articleId)
    : NotFoundException($"The article with id: {articleId} doesn't exist int the database.")
{ }
