namespace FreezerTape2.Models
{
    public class StoragePlace
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public List<Package>? Packages { get; set; }

        public String IdentifyingName
        {
            get
            {
                if (this.Name != null)
                {
                    return this.Id.ToString() + " - " + this.Name.ToString();
                }
                else
                {
                    return this.Id.ToString();
                }
            }
        }
    }
}
