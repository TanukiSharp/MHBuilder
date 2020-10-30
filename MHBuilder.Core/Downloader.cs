using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MHBuilder.Core
{
    public class Downloader
    {
        private readonly HttpClient httpClient;
        private readonly string cachePath;
        private readonly TimeSpan cacheValidityDuration;

        public Downloader(IHttpClientFactory httpClientFactory, string baseUrl, TimeSpan cacheValidityDuration)
        {
            this.cacheValidityDuration = cacheValidityDuration;

            httpClient = httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(baseUrl);

            cachePath = Path.Join(AppContext.BaseDirectory, "cache");

            if (Directory.Exists(cachePath) == false)
                Directory.CreateDirectory(cachePath);
        }

        private bool ShouldDownload(string cacheFile)
        {
            if (File.Exists(cacheFile) == false)
                return true;

            if (File.GetLastWriteTimeUtc(cacheFile) < (DateTime.UtcNow - cacheValidityDuration))
                return true;

            return false;
        }

        public async Task<string> GetFileContent(string relativeFilename, bool force = false)
        {
            string cacheFile = Path.Join(cachePath, relativeFilename);

            string content;

            if (force || ShouldDownload(cacheFile))
            {
                content = await httpClient.GetStringAsync(relativeFilename);
                File.WriteAllText(cacheFile, content);
            }
            else
                content = File.ReadAllText(cacheFile);

            return content;
        }
    }
}
