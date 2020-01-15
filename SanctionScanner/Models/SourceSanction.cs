using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneySanction.Models
{
    public class SourceSanction : BaseEntity
    {
        public SourceSanction()
        {
            Sanctions = new HashSet<Sanction>();
        }
        public string SourceName { get; set; }
        public string SourceCode { get; set; }
        public string NameFile { get; set; }
        public string FormatFile { get; set; }
        public bool HasFile { get; set; }
       //public IFormFile  FormFile { get; set; }
        public virtual ICollection<Sanction> Sanctions { get; set; }
    }
}
