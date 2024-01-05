using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Domain
{
    public class NumbComp : BaseModel
    {
        [Key, Column(Order = 1)]
        public string component { get; set; }
        [Key, Column(Order = 0)]
        public string code { get; set; }
    }
}
