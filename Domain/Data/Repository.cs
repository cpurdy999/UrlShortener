using Domain.Data.Models;

namespace Domain.Data
{
    public class ShortUriRepository : IRepository<ShortUri>
    {
        private readonly UrlShortenerContext _context;

        public ShortUriRepository(UrlShortenerContext context)
        {
            _context = context;
        }

        public IQueryable<ShortUri> Items => _context.ShortUris;

        public void Add(ShortUri item) => _context.ShortUris.Add(item);

        public void Remove(ShortUri entity) => _context.Remove(entity);

        public void SaveChanges() => _context.SaveChanges();
    }
}
