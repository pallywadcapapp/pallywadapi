﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Domain
{
    public partial class Collateral: BaseModel
    {
        public string name { get; set; }
        public bool status { get; set; }
        public string description { get; set; }

    }
}
