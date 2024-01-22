using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Domain
{
    public class AccountBase: BaseModel
    {
        public string Type { get; set; }
        public string Code { get; set; }
        public string LedgerGroup { get; set; }
    }
}
