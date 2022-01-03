using System.ComponentModel.DataAnnotations;

namespace FreezerTape2.Models
{
    public class Carcass
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ShotDate { get; set; }

        public string? ShotPlace { get; set; }
        public string? ShotBy { get; set; }

        public double? LiveWeight { get; set; }
        public double? DressedWeight { get; set; }

        public string? PositionOfBulkhead { get; set; }
        public string? Gender { get; set; }
        public double? Age { get; set; }
        public string? Comment { get; set; }

        public int? SpecieId { get; set; }
        public Specie? Specie { get; set; }

        public List<Package>? Packages { get; set; }

        public String IdentifyingName
        {
            get
            {
                if (this.Specie != null)
                {
                    if (this.ShotDate != null)
                    {
                        return this.Id.ToString() + " - " + this.Specie.Name.ToString() + " - " + this.ShotDate.GetValueOrDefault().ToShortDateString();
                    }
                    else
                    {
                        return this.Id.ToString() + " - " + this.Specie.Name.ToString();
                    }
                }
                else
                {
                    if (this.ShotDate != null)
                    {
                        return this.Id.ToString() + " - " + this.ShotDate.ToString();
                    }
                    else
                    {
                        return this.Id.ToString();
                    }
                }
            }
        }
    }
}
