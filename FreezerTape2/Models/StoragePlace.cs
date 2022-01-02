namespace FreezerTape2.Models
{
    public class StoragePlace
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public List<Package>? Packages { get; set; }
    }
}
