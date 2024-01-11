using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Domain.Dto
{
    public class ConfigDto
    {
        public string configname { get; set; }
        public string username { get; set; }
        public string mailfrom { get; set; }
        public string password { get; set; }
        public string smtp { get; set; }
        public int port { get; set; }
        public bool isSSL { get; set; }
        public string URL { get; set; }
        public string key { get; set; }
    }
}
