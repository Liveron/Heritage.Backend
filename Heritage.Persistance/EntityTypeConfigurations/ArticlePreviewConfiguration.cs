using Heritage.Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Heritage.Persistance.EntityTypeConfigurations;

internal class ArticlePreviewConfiguration : IEntityTypeConfiguration<ArticlePreview>
{
    public void Configure(EntityTypeBuilder<ArticlePreview> builder)
    {
        builder.HasKey(preview => preview.ArticleId);
    }
}
