using System.ComponentModel.DataAnnotations;

namespace FreezerTape2.Models
{
    public class StoragePlace
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        [Display(Name = "Package")]
        public List<Package>? Packages { get; set; }

        /// <summary>
        /// Returns a string with informaiton to identify this specific storage place.
        /// </summary>
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
