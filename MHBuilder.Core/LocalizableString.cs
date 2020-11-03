using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHBuilder.Core
{
    public record Language(string DisplayName, string Code);

    [DebuggerDisplay("{ToString()}")]
    public class LocalizableString : Dictionary<string, string>
    {
        private new string this[string key]
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string this[Language key]
        {
            get
            {
                return this[key.Code];
            }
        }

        public bool TryGetValue(Language key, [MaybeNullWhen(false)] out string value)
        {
            return TryGetValue(key.Code, out value);
        }

        public override string ToString()
        {
            if (LocalizationContext.DefaultContext == null)
                return "<no-localization-context>";

            return this[LocalizationContext.DefaultContext.DefaultLanguage.Code];
        }
    }
}
