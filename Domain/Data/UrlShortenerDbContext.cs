using Domain.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Domain.Data
{
    public class UrlShortenerContext : DbContext
    {
        public UrlShortenerContext(DbContextOptions<UrlShortenerContext> options)
            : base(options)
        {
        }

        public DbSet<ShortUri> ShortUris { get; set; }
    }
}