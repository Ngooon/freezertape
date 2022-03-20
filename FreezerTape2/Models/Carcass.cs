using System.ComponentModel.DataAnnotations;

namespace FreezerTape2.Models
{
    public class Carcass
    {
        public int Id { get; set; }

        [Display(Name = "Shot date")]
        [DataType(DataType.Date)]
        public DateTime? ShotDate { get; set; }

        [Display(Name = "Shot place")]
        public string? ShotPlace { get; set; }

        [Display(Name = "Shot by")]
        public string? ShotBy { get; set; }

        [Display(Name = "Live weight")]
        public double? LiveWeight { get; set; }

        [Display(Name = "Dressed weight")]
        public double? DressedWeight { get; set; }

        [Display(Name = "Position of bulkhead")]
        public string? PositionOfBulkhead { get; set; }
        public string? Gender { get; set; }
        public double? Age { get; set; }
        public string? Comment { get; set; }

        [Display(Name = "Specie")]
        public int? SpecieId { get; set; }
        public Specie? Specie { get; set; }

        [Display(Name = "Package")]
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
