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
        public string description { get; set; }
    }

    public class AcctypeDto
    {
        public string name { get; set; }
        public string description { get; set; }
    }
}
