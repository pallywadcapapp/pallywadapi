﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Domain
{
    public partial class LoanSetup : BaseModel
    {
        public string loancode { get; set; }

        public string loandesc { get; set; }

        public double loaninterest { get; set; }

        public string accountno { get; set; }

        public int duration { get; set; }

        public bool loanrequire { get; set; }

        public string sharecode { get; set; }

        public string savingscode { get; set; }

        public string interestcode { get; set; }

        public string shortname { get; set; }

        public double processrate { get; set; }

        public string chargecode { get; set; }

        public bool processind { get; set; }

        public bool normalloanind { get; set; }

        public string memgroupacct { get; set; }

        public double processamt { get; set; }
        public bool require_collateral { get; set; }
        //public string collateral { get; set; }
        public bool request_while_running { get; set; }
        public string category { get; set; }
        //public bool enforceGuarantor { get; set; }
        public int repayOrder { get; set; }
    }
}
