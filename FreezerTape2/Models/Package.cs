using System.ComponentModel.DataAnnotations;
using FreezerTape2.Helpers;

namespace FreezerTape2.Models
{
    public class Package
    {
        public int Id { get; set; }
        public double? Weight { get; set; }

        [Display(Name = "Packing date")]
        [DataType(DataType.Date)]
        public DateTime? PackingDate { get; set; }

        [Display(Name = "Expiry date")]
        [DataType(DataType.Date)]
        public DateTime? ExpiryDate { get; set; }

        public string? Comment { get; set; }

        [Display(Name = "Carcass")]
        public int? CarcassId { get; set; }
        public Carcass? Carcass { get; set; }

        [Display(Name = "Primal cut")]
        public int? PrimalCutId { get; set; }
        public PrimalCut? PrimalCut { get; set; }

        [Display(Name = "Storage place")]
        public int? StoragePlaceId { get; set; }
        public StoragePlace? StoragePlace { get; set; }

        public string WeightAsString
        {
            get
            {
                return new Weight(this.Weight).ToString();
            }
        }

        public String IdentifyingName
        {
            get
            {
                if (this.Carcass != null)
                {
                    if (this.PrimalCut != null)
                    {
                        return this.Id.ToString() + " - " + this.PrimalCut.Name + " (" + this.Carcass.IdentifyingName + ")";
                    }
                    else
                    {
                        return this.Id.ToString() + " (" + this.Carcass.IdentifyingName + ")";
                    }
                }
                else
                {
                    if (this.PrimalCut != null)
                    {
                        return this.Id.ToString() + " - " + this.PrimalCut.Name;
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
