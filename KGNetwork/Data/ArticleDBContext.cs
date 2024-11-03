using Microsoft.EntityFrameworkCore;

namespace KGNetwork.Data
{
    public class ArticleDBContext : DbContext
    {
        public ArticleDBContext(DbContextOptions<ArticleDBContext> options) : base(options) { }

        public DbSet<Article> Articles { get; set; }

        public DbSet<Graph> Graphs { get; set; }
    }
}
