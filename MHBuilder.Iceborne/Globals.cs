using MHBuilder.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MHBuilder.Iceborne
{
    public static class Globals
    {
        public static Downloader? Downloader { get; private set; }
        public static readonly MasterData MasterData;
        public static readonly string ConfigDirectory;

        static Globals()
        {
            LocalizationContext.DefaultContext = new LocalizationContext(Constants.DefaultLanguage);
            ConfigDirectory = EnsureConfigDirectory();
            MasterData = new MasterData();
        }

        private static string EnsureConfigDirectory()
        {
            string configDirectory = Path.Join(AppContext.BaseDirectory, "config");

            if (Directory.Exists(configDirectory) == false)
                Directory.CreateDirectory(configDirectory);

            return configDirectory;
        }

        public static void SetupDownloader(IHttpClientFactory httpClientFactory)
        {
            Downloader = new Downloader(
                httpClientFactory,
                Constants.MasterDataBaseUrl,
                TimeSpan.FromHours(24.0)
            );
        }
    }
}
