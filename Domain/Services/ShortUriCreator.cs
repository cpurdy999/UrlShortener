using Domain.Data;
using Domain.Data.Models;
using System.Text;

namespace Domain.Services
{
    public class ShortUriCreator : IShortUriCreator
    {
        private static readonly char[] tagCharacters;

        private readonly IRepository<ShortUri> _repository;
        private readonly Random _random;

        static ShortUriCreator()
        {
            tagCharacters = Enumerable.Range('A', 26)
                .Concat(Enumerable.Range('a', 26))
                .Concat(Enumerable.Range('0', 10))
                .Select(Convert.ToChar)
                .ToArray();
        }

        public ShortUriCreator(IRepository<ShortUri> repository, Random random)
        {
            _repository = repository;
            _random = random;
        }

        public ShortUri Create(Uri input)
        {
            var result = new ShortUri(CreateAccessTag(), input);
            _repository.Add(result);
            _repository.SaveChanges();
            return result;
        }

        private string CreateAccessTag()
        {
            StringBuilder stringBuilder = new StringBuilder();

            for (int x = 0; x < 8; x++)
            {
                stringBuilder.Append(tagCharacters[_random.Next(63)]);
            }

            return stringBuilder.ToString();
        }
    }
}
