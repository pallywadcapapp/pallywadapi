using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Domain.Dto
{
    public class CompanyBalance
    {
        public string division { get; set; }
        public string company { get; set; }
        public double? balance { get; set; }
        public string reportMap { get; set; }
        public string product { get; set; }
        public string group { get; set; }
    }
}
