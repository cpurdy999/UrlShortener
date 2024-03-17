namespace Domain.Data.Models
{
    public class ShortUri
    {
        public ShortUri(string accessTag, string destination)
        {
            ID = Guid.NewGuid();

            AccessTag = accessTag;
            Destination = destination;
        }

        public Guid ID { get; set; }
        public string AccessTag { get; set; }
        public string Destination { get; set; }
    }
}
