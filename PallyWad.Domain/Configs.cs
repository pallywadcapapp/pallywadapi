using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Domain
{
    public class Configs: BaseModel
    {
        [Key]
        public string configname { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }

    public class SmtpConfig : Configs
    {
        
        public string smtp { get; set; }
        public int port { get; set; }
        public bool isSSL { get; set; }
    }

    public class SMSConfig: Configs {
        public string URL { get; set; } 
        public string key { get; set; }
    }
}
