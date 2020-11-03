using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MHBuilder.Core
{
    public struct SearchInfo
    {
        public readonly bool IsExact;
        public readonly string Text;

        public SearchInfo(bool isExact, string text)
        {
            IsExact = isExact;
            Text = text;
        }

        public bool IsMatching(string textToLower)
        {
            if (IsExact)
            {
                if (textToLower == Text)
                    return true;
            }
            else if (textToLower.Contains(Text))
                return true;

            return false;
        }

        public override bool Equals(object? obj)
        {
            if (obj is SearchInfo si)
                return si.IsExact == IsExact && si.Text == Text;
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IsExact, Text);
        }

        public static bool operator ==(SearchInfo left, SearchInfo right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SearchInfo left, SearchInfo right)
        {
            return !(left == right);
        }
    }

    public class SearchStatement
    {
        public string? OriginalSearchText { get; }
        public IDictionary<string, string>? OriginalAliases { get; }

        public bool IsEmpty { get; }
        public readonly ReadOnlyCollection<SearchInfo>? SearchInfo;

        public static readonly SearchStatement Empty = new SearchStatement(null, null);

        private static readonly char[] searchInfoSeparators = new[] { ',', ';', '/', ':' };

        public static SearchStatement Create(string? searchText, IDictionary<string, string>? aliases)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return Empty;

            return new SearchStatement(searchText, aliases);
        }

        public SearchStatement(string? searchText, IDictionary<string, string>? aliases)
        {
            OriginalSearchText = searchText;
            OriginalAliases = aliases;

            if (string.IsNullOrWhiteSpace(searchText))
            {
                IsEmpty = true;
                return;
            }

            var searchInfo = new List<SearchInfo>();

            foreach (string sub in searchText.Split(searchInfoSeparators))
            {
                string subText = sub.Trim();

                if (subText.Length == 0)
                    continue;

                subText = subText.ToLower();

                bool isExact = subText.StartsWith("=");

                if (isExact)
                    subText = subText[1..].TrimStart();

                if (aliases != null)
                {
                    foreach (KeyValuePair<string, string> kv in aliases)
                    {
                        if (subText.Contains(kv.Key))
                            subText = Regex.Replace(subText, $"\\b{Regex.Escape(kv.Key)}\\b", kv.Value);
                    }
                }

                searchInfo.Add(new SearchInfo(isExact, subText));
            }

            IsEmpty = searchInfo.Count == 0;

            if (IsEmpty == false)
                SearchInfo = new ReadOnlyCollection<SearchInfo>(searchInfo);
        }

        public bool IsMatching(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return true;

            string textToLower = text.Trim().ToLower();

            if (SearchInfo != null)
            {
                foreach (SearchInfo si in SearchInfo)
                {
                    if (si.IsMatching(textToLower))
                        return true;
                }
            }

            return false;
        }
    }
}
