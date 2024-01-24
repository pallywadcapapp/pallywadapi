using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Domain
{
    public class LoanUserDocument: BaseModel
    {
        public int loanRequestId { get; set; }
        public string userDocumentlId { get; set; }
        public virtual LoanRequest loanRequest { get; set; }
    }
}
