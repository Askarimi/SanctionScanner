using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SanctionScanner.Models
{
    public class SanctionScannerDbContext : DbContext
    {
        public SanctionScannerDbContext(DbContextOptions<SanctionScannerDbContext> options) : base(options)
        {

        }
        public DbSet<Sanction> Sanctions { get; set; }
        public DbSet<SourceSanction> SourceSanctions { get; set; }
    }
}
