using Heritage.Application.Models;

namespace Heritage.Application.DataTransferObjects;

public record ArticleDto(Guid Id, string Content, string Title, ArticlePreviewDto Preview);
