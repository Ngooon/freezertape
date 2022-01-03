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

        public int CountPackages
        {
            get
            {
                int count = 0;
                foreach (Carcass carcass in Carcasses)
                {
                    count += carcass.Packages.Count();
                }
                return count;
            }
        }
    }
}
