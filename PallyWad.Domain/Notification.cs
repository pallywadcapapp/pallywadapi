using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Domain
{
    public class Notification: BaseModel
    {
        [Required]
        public string memberId { get; set; }
        [Required]
        public string message { get; set; }
        //public string pre { get; set; }
        public bool readStatus { get; set; }
        public string? senderId { get; set; }
        //public int MyProperty { get; set; }
    }
}
