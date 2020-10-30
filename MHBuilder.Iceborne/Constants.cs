using MHBuilder.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHBuilder.Iceborne
{
    public static class Constants
    {
        public const string MasterDataBaseUrl = "https://raw.githubusercontent.com/TanukiSharp/MHWMasterDataUtils/master/MHWMasterDataUtils.Exporter/data/";

        public static readonly Language DefaultLanguage = new Language("English", "eng");

        public static readonly Language[] Languages = new Language[]
        {
            new Language("日本語", "jpn"),
            DefaultLanguage,
            new Language("Français", "fre"),
            new Language("Español", "spa"),
            new Language("Deutsch", "ger"),
            new Language("Italiano", "ita"),
            new Language("한국어", "kor"),
            new Language("中文繁體T", "cnt"),
            new Language("中文繁體S", "cns"),
            new Language("Русский", "rus"),
            new Language("Polska", "pol"),
            new Language("Portugues", "ptb"),
            new Language("Arabic", "ara"),
        };
    }
}
