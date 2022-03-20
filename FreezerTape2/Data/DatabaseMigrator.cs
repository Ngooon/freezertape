using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;

namespace FreezerTape2.Data
{
    public sealed class DatabaseMigrator
    {
        public static void Migrate()
        {
            string connectionString = "server = mysql; database = freezertapedb; user = freezertape; password = freezertape";
            var options = new DbContextOptionsBuilder<FreezerContext>()
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                .Options;

            using (var context = new FreezerContext(options))
            {
                context.Database.Migrate();
                FreezerContext.IsMigrationChecked = true;
            }
        }
    }
}
