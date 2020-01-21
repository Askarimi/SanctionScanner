using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SanctionScanner.Models
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
