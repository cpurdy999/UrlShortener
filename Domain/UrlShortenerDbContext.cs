using Domain.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Domain
{
    public class UrlShortenerContext : DbContext
    {
        public UrlShortenerContext(DbContextOptions<UrlShortenerContext> options)
            : base(options)
        {
        }

        public DbSet<UriEntry> UriEntries { get; set; }
    }
}