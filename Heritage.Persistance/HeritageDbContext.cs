using Heritage.Application.Models;
using Heritage.Domain;
using Heritage.Persistance.EntityTypeConfigurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Heritage.Persistance;

public class HeritageDbContext(DbContextOptions<HeritageDbContext> options) 
    : IdentityDbContext<User>(options)
{
    public DbSet<Article> Articles { get; set; }
    public DbSet<ArticlePreview> ArticlePreviews { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfiguration(new ArticlePreviewConfiguration())
            .ApplyConfiguration(new RoleConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
}
