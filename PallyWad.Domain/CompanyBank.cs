using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Domain
{

    public class CompanyBank : BaseModel
    {
        public string name { get; set; }
        public string accountno { get; set; }
        public string accountname { get; set; }
        public bool isDelete { get; set; }
    }
}
