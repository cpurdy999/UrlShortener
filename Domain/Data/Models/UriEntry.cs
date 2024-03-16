namespace Domain.Data.Models
{
    public class UriEntry
    {
        public UriEntry(string accessTag, Uri destination)
        {
            AccessTag = accessTag;
            Destination = destination;
        }

        public Guid ID { get; set; }
        public string AccessTag { get; set; }
        public Uri Destination { get; set; }
    }
}
