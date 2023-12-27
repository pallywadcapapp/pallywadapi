using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Domain
{
    public partial class LoanSetup : BaseModel
    {
        public string Loancode { get; set; }

        public string Loandesc { get; set; }

        public double Loaninterest { get; set; }

        public string Accountno { get; set; }

        public int Duration { get; set; }

        public bool Loanrequire { get; set; }

        public string Sharecode { get; set; }

        public string Savingscode { get; set; }

        public string Interestcode { get; set; }

        public string Shortname { get; set; }

        public double Processrate { get; set; }

        public string Chargecode { get; set; }

        public bool Processind { get; set; }

        public bool Normalloanind { get; set; }

        public string Memgroupacct { get; set; }

        public double Processamt { get; set; }
        public bool requireGuarantor { get; set; }
        public int no_of_guarantor { get; set; }
        public bool request_while_running { get; set; }
        public string category { get; set; }
        public bool enforceGuarantor { get; set; }
        public int repayOrder { get; set; }
    }
}
