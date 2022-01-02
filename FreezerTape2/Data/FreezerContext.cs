#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FreezerTape2.Models;

namespace FreezerTape2.Data
{
    public class FreezerContext : DbContext
    {
        public FreezerContext (DbContextOptions<FreezerContext> options)
            : base(options)
        {
        }

        public DbSet<FreezerTape2.Models.Carcass> Carcass { get; set; }

        public DbSet<FreezerTape2.Models.Package> Package { get; set; }

        public DbSet<FreezerTape2.Models.PrimalCut> PrimalCut { get; set; }

        public DbSet<FreezerTape2.Models.Specie> Specie { get; set; }

        public DbSet<FreezerTape2.Models.StoragePlace> StoragePlace { get; set; }
    }
}
