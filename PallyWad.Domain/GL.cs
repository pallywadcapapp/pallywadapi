using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Domain
{
    public partial class GL : BaseModel
    {
        public string refnumber { get; set; }

        public DateTime transdate { get; set; }

        public string glaccta { get; set; }

        public string glacctb { get; set; }

        public string glacctc { get; set; }

        public string glacctd { get; set; }

        public string accountno { get; set; }

        public string batchno { get; set; }

        public string description { get; set; }

        public string chequeno { get; set; }

        public double debitamt { get; set; }

        public double creditamt { get; set; }

        public double balance { get; set; }

        public string userid { get; set; }
        public string acc_name { get; set; }
        public string month { get; set; }
        public int year { get; set; }
        public string ref_trans { get; set; }
        public DateTime? processedDate { get; set; }
    }


    public class GLComp : GL
    {
        public string Name { get; set; }
        public string MemberId { get; set; }
        public string AccName { get; set; }
    }


    public class GlAcc
    {
        public string memberId { get; set; }
        public string accountNo { get; set; }
        public string credit { get; set; }
        public string debit { get; set; }
        public string savBalance { get; set; }
        public string loanBalance { get; set; }
        public string loanGuarantee { get; set; }
        public string acctype { get; set; }
    }

    public class GlReport
    {
        public string memberId { get; set; }
        public string fullname { get; set; }
        public double credit { get; set; }
        public double debit { get; set; }
        public string description { get; set; }
        public string refnumber { get; set; }
        public DateTime transdate { get; set; }
    }
}
