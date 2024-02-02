using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Domain.Dto
{
    public class DepositDto
    {
        [Required]
        public double amount { get; set; }
        public string otherdetails { get; set; }
        public string channel { get; set; }
    }
}
