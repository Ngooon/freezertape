namespace FreezerTape2.Models
{
    public class Specie
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public ICollection<PrimalCut>? PrimalCuts { get; set; }

        public List<Carcass>? Carcasses { get; set; }

        public Specie(int id)
        {
            this.Id = id;
        }
    }
}
