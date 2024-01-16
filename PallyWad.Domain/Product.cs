using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Domain
{
    public class Product: BaseModel
    {
        public string Name { get; set; }
        public string type { get; set; }
        public bool isDefault { get; set; }
    }
}
