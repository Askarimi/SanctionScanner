﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SanctionScanner.Models
{
    public class SourceSanctionModel
    {
        public string SourceName { get; set; }
        public string SourceCode { get; set; }
        public string NameFile { get; set; }
        public string FormatFile { get; set; }
        public bool HasFile { get; set; }
    }
}
