namespace Domain.Data.Models
{
    public class ShortUri
    {
        public ShortUri(string accessTag, Uri destination)
        {
            ID = Guid.NewGuid();

            AccessTag = accessTag;
            Destination = destination;
        }

        public Guid ID { get; set; }
        public string AccessTag { get; set; }
        public Uri Destination { get; set; }
    }
}
