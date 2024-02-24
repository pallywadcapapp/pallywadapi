using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Domain
{
    public class UserBank: BaseModel
    {
        public string name { get; set; }
        public string accountno { get; set; }
        public bool isDefault { get; set; }
        public string memberId { get; set; }
    }
}
