using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHBuilder.Core
{
    public record Language(string DisplayName, string Code);

    public class LocalizableString : Dictionary<Language, string>
    {
        public LocalizableString() : base() { }
        public LocalizableString(IDictionary<Language, string> dictionary) : base(dictionary) { }
        public LocalizableString(IEnumerable<KeyValuePair<Language, string>> collection) : base(collection) { }
        public LocalizableString(IEqualityComparer<Language>? comparer) : base(comparer) { }
        public LocalizableString(int capacity) : base(capacity) { }
        public LocalizableString(IDictionary<Language, string> dictionary, IEqualityComparer<Language>? comparer) : base(dictionary, comparer) { }
        public LocalizableString(IEnumerable<KeyValuePair<Language, string>> collection, IEqualityComparer<Language>? comparer) : base(collection, comparer) { }
        public LocalizableString(int capacity, IEqualityComparer<Language>? comparer) : base(capacity, comparer) { }
    }
}
