using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Domain.Entities
{
    public class AccSchema
    {
        public string type { get; set; }
        public string name { get; set; }
        public string memberId { get; set; }
        public string desc { get; set; }
        public string loancode { get; set; }
    }
}
