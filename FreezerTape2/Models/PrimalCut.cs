namespace FreezerTape2.Models
{
    public class PrimalCut
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        //public int? SpecieId { get; set; }
        //public Specie? Specie { get; set; }
        public ICollection<Specie>? Species { get; set; }
        //public ICollection<SpeciePrimalCut> SpeciePrimalCuts { get; set; }


        public List<Package>? Packages { get; set; }

        public void Update(PrimalCut primalCut)
        {
            this.Id = primalCut.Id;
            this.Name = primalCut.Name;
            this.Packages = primalCut.Packages;
            this.Species = primalCut.Species;
        }
    }
}
