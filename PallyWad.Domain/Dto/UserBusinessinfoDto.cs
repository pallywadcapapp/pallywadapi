using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Domain.Dto
{
	public class UserBusinessInfoDto
	{
		public string name { get; set; }
		public string type { get; set; }
		public string? info_medium { get; set; }
		public bool isRegistered { get; set; }
		public string address { get; set; }
        public string? landmark { get; set; }
        public string cacno { get; set; }
        public string business_phoneNo { get; set; }
        public string tin { get; set; }
        public string business_email { get; set; }
        public string sector { get; set; }
        public string services { get; set; }
    }
}
