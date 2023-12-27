using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Domain
{
    public class TB : BaseModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public new string TenantId { get; set; }
        public string acc_name { get; set; }
        public double debit { get; set; }
        public double credit { get; set; }
        public double balance { get; set; }
        public string company { get; set; }
        public string tgl_crt_acc_number { get; set; }
        public string category { get; set; }
        public string spoke { get; set; }
        public string sbu { get; set; }
        public string units { get; set; }
        public double dr_amount { get; set; }
        public double cr_amount { get; set; }
        public string accountno { get; set; }

    }
}
