using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Domain
{
    public class Interest: BaseModel
    {
        public string interestcode { get; set; }

        public string interestdesc { get; set; }

        public string accountno { get; set; }

        public string payableacctno { get; set; }

        public string shortname { get; set; }
    }
}
