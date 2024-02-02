using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Domain
{
    public class AppUploadedFiles : BaseModel
    {
        [Required]
        [Key, Column(Order = 0)]
        public string filename { get; set; }
        public string fileurl { get; set; }
        public string uploaderId { get; set; }
        public string comment { get; set; }
        public bool extractedStatus { get; set; }
        public string type { get; set; }
        public int year { get; set; } = DateTime.Now.Year;
        public string? transOwner { get; set; }
    }
}
