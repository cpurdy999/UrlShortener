namespace Domain.Data
{
    public interface IRepository<T>
    {
        IQueryable<T> Items { get; }
        void Add(T entity);
        void Remove(T entity);
        void SaveChanges();
    }
}
