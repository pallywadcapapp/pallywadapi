using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Domain
{
    public class ProductNo : BaseModel
    {
        [Key, Column(Order = 0)]
        public string component { get; set; }

        public int number { get; set; }
    }

}
