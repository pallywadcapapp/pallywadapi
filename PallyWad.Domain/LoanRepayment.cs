using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Domain
{
    public partial class LoanRepayment: BaseModel
    {
        public string memberid { get; set; }

        public string loanrefnumber { get; set; }

        public string repayrefnumber { get; set; }

        public DateTime transdate { get; set; }

        public string loancode { get; set; }

        public double loanamount { get; set; }

        public double repayamount { get; set; }

        public double interestamt { get; set; }

        public string description { get; set; }

        public int transmonth { get; set; }

        public int transyear { get; set; }

        public int updated { get; set; }
        public double? interestRate { get; set; }
    }
}
