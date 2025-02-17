﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PallyWad.Domain
{
    public class LoanCollateral: BaseModel
    {
        public int LoanSetupId { get; set; }
        public string collateralId { get; set; }
        [JsonIgnore]
        public virtual LoanSetup loanSetup { get; set; }

        /*public string refnumber { get; set; }

        public DateTime? transdate { get; set; }

        public string memberid { get; set; }

        public string collateralid { get; set; }

        public decimal? loanamount { get; set; }

        public DateTime? repaystartdate { get; set; }

        public DateTime? repayenddate { get; set; }

        public string loancode { get; set; }

        public string loanrefnumber { get; set; }

        public byte? Repay { get; set; }*/
    }
}
