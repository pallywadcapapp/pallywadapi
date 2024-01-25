using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Domain.Dto
{
    public class SetupDto
    {
        public string name { get; set; }
        public bool status { get; set; }
        public string type { get; set; }
        public string description { get; set; }
    }

    public class AcctypeDto
    {
        public string name { get; set; }
        public string description { get; set; }
    }

    public class InterestDto
    {
        public string interestcode { get; set; }

        public string interestdesc { get; set; }

        public string accountno { get; set; }

        public string payableacctno { get; set; }

        public string shortname { get; set; }
    }
    public class ChargesDto{
        public string chargecode { get; set; }

        public string chargedesc { get; set; }

        public string accountno { get; set; }

        public string shortname { get; set; }
    }
}
