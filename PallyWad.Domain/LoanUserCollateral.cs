using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Domain
{
    public class LoanUserCollateral: BaseModel
    {
        public int loanRequestId { get; set; }
        public string userCollateralId { get; set; }
        public virtual LoanRequest loanRequest { get; set; }
    }
}
