using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Domain
{
    public partial class LoanTrans: BaseModel
    {

        public string memberid { get; set; }

        public string loanrefnumber { get; set; }

        public DateTime transdate { get; set; }

        public string loancode { get; set; }

        public DateTime repaystartdate { get; set; }
        public DateTime nextRepayDate { get; set; }

        public double loanamount { get; set; }

        public double totrepayable { get; set; }

        public double repayamount { get; set; }

        public double interestamt { get; set; }

        public int repay { get; set; }

        public string description { get; set; }

        public string accountno { get; set; }

        public string payableacctno { get; set; }

        public string grefnumber { get; set; }

        public bool gapproved { get; set; }

        public double loaninterest { get; set; }

        public double processrate { get; set; }

        public int duration { get; set; }

        public double processamt { get; set; }

        public bool updated { get; set; }

        public bool stopdoubleinterest { get; set; }

        public string glrefnumber { get; set; }

        public string glbankaccount { get; set; }
        public int repayOrder { get; set; }
        public double? monthlyInterest { get; set; }
    }

    public class LoanTransExt : LoanTrans
    {
        public double balance { get; set; }
        public string category { get; set; }
    }
}
