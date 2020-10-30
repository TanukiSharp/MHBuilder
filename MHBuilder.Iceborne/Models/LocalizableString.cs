using MHBuilder.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHBuilder.Iceborne.Models
{
    [DebuggerDisplay("{ToString()}")]
    public class LocalizableString : Dictionary<string, string>
    {
        public override string ToString()
        {
            if (LocalizationContext.DefaultContext == null)
                return "<no-localization-context>";

            return this[LocalizationContext.DefaultContext.DefaultLanguage.Code];
        }
    }
}
