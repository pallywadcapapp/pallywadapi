using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Domain
{
    public class Payment : PV
    {
        public string paymentMode { get; set; }
        public string currency { get; set; }
        public double? exchangeRate { get; set; }
        public string payState { get; set; }
        //public string transId { get; set; }
    }
}
