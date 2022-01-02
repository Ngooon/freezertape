using System.ComponentModel.DataAnnotations;

namespace FreezerTape2.Models
{
    public class Package
    {
        public int Id { get; set; }
        public double? Weight { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? PackingDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ExpiryDate { get; set; }
        public string? Comment { get; set; }

        public int? CarcassId { get; set; }
        public Carcass? Carcass { get; set; }

        public int? PrimalCutId { get; set; }
        public PrimalCut? PrimalCut { get; set; }

        public int? StoragePlaceId { get; set; }
        public StoragePlace? StoragePlace { get; set; }
    }
}
