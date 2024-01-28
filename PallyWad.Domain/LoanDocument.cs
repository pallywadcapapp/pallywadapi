using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PallyWad.Domain
{
    public class LoanDocument: BaseModel
    {
        public int LoanSetupId { get; set; }
        public string documentId { get; set; }
        [JsonIgnore]
        public virtual LoanSetup loanSetup { get; set; }
    }
}
