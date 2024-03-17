using Domain.Data.Models;

namespace Domain.Services
{
    public interface IShortUriCreator
    {
        public ShortUri Create(string input);
    }
}
