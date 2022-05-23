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
        [Display(Name = "Carcass")]
        public Carcass? Carcass { get; set; }

        [Display(Name = "Primal cut")]
        public int? PrimalCutId { get; set; }
        [Display(Name = "Primal cut")]
        public PrimalCut? PrimalCut { get; set; }

        [Display(Name = "Storage place")]
        public int? StoragePlaceId { get; set; }
        [Display(Name = "Storage place")]
        public StoragePlace? StoragePlace { get; set; }

        /// <summary>
        /// Returns the <see cref="Weight"/> as a string with a unit.
        /// </summary>
        public string WeightAsString
        {
            get
            {
                return new Weight(this.Weight).ToString();
            }
        }

        /// <summary>
        /// True if the the expiry date is closer than 2 months.
        /// </summary>
        public bool IsExpiryDateNear
        {
            get
            {
                if (DateTime.Now.AddMonths(2) > this.ExpiryDate)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// True if the expiry date has passed.
        /// </summary>
        public bool HasExpiryDatePassed
        {
            get
            {
                if (DateTime.Now > this.ExpiryDate)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Returns a string with informaiton to identify this specific package.
        /// </summary>
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
