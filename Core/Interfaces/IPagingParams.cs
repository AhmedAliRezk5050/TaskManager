﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IPagingParams
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
