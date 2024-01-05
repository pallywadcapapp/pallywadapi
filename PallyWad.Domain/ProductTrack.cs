using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Domain
{
    public class ProductTrack : BaseModel
    {
        public int productrange { get; set; }
        [Key, Column(Order = 0)]
        public string productname { get; set; }
    }
}
