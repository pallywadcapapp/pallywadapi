using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Services.Attributes
{
    public class ScopedRegistrationAttribute : Attribute { }

    public class SingletonRegistrationAttribute : Attribute { }

    public class TransientRegistrationAttribute : Attribute { }
}
