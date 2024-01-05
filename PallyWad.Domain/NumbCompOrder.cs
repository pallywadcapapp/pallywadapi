using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Domain
{
    public class NumbCompOrder : BaseModel
    {
        [Key, Column(Order = 1)]
        public int position { get; set; }
        public string productcode { get; set; }
        [Key, Column(Order = 0)]
        public string productname { get; set; }
    }
}
