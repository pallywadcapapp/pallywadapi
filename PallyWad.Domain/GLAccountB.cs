﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Domain
{
    public partial class GLAccountB : BaseModel
    {
        public string glaccta { get; set; }

        public string glacctb { get; set; }

        public string accountno { get; set; }

        public string accttype { get; set; }

        public string shortdesc { get; set; }

        public string fulldesc { get; set; }

        public int acctlevel { get; set; }
    }
}
