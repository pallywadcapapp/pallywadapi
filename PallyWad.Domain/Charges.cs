using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Domain
{
    public class Charges: BaseModel
    {
        public string chargecode { get; set; }

        public string chargedesc { get; set; }

        public string accountno { get; set; }

        public string shortname { get; set; }
    }
}
