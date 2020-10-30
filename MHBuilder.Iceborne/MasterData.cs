using MHBuilder.Iceborne.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MHBuilder.Iceborne
{
    public class MasterData
    {
        private Skill[]? skills;

        public async Task<Skill[]> GetSkills()
        {
            if (skills != null)
                return skills;

            const string filename = "skills.json";

            if (Globals.Downloader == null)
                throw new InvalidOperationException("Downloader instance not yet ready.");

            string content = await Globals.Downloader.GetFileContent(filename);

            skills = JsonSerializer.Deserialize<Skill[]>(content, new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                PropertyNameCaseInsensitive = false,
                ReadCommentHandling = JsonCommentHandling.Skip
            });

            if (skills == null)
                throw new InvalidDataException($"File '{filename}' contains invalid data.");

            return skills;
        }
    }
}
