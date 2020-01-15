using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneySanction.Models
{
    public class MoneySanctionDbContext : DbContext
    {
        public MoneySanctionDbContext(DbContextOptions<MoneySanctionDbContext> options) : base(options)
        {

        }
        public DbSet<SourceSanction> SourceSanctions { get; set; }
        public DbSet<Sanction> Sanctions { get; set; }
    }
}
