using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Domain.Dto
{
    public class DeductionDto
    {
        public double amount { get; set; }
        public string loanRef { get; set; }
        public string memberId { get; set; }
        public double chargesAmount { get; set; }
    }
}
