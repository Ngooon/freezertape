namespace FreezerTape2.Models
{
    public class PrimalCut
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public ICollection<Specie>? Species { get; set; }

        public List<Package>? Packages { get; set; }

        public void Copy(PrimalCut primalCut)
        {
            this.Id = primalCut.Id;
            this.Name = primalCut.Name;
            this.Packages = primalCut.Packages;
            this.Species = primalCut.Species;
        }

        public String ShortName
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
