using Heritage.Application.Models;

namespace Heritage.Application.Common.Exceptions;

public class ArticlePreviewNotFoundException(Guid id) 
    : NotFoundException($"The article preview with id: {id} doesn't exist int the database.")
{ }
