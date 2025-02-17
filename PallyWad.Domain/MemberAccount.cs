﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Domain
{
    public partial class MemberAccount: BaseModel
    {
        public string AppIdentityUserId { get; set; }
        public string memberid { get; set; }

        public string memgroupacct { get; set; }

        public string accountno { get; set; }

        public string deductcode { get; set; }

        public string transtype { get; set; }
        public virtual AppIdentityUser member { get; set; }
    }
}
