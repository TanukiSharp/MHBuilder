using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MHBuilder.WPF
{
    public class SerializableWindowConfiguration
    {
        [JsonPropertyName("left")]
        public int? Left { get; set; }
        [JsonPropertyName("top")]
        public int? Top { get; set; }
        [JsonPropertyName("width")]
        public int? Width { get; set; }
        [JsonPropertyName("height")]
        public int? Height { get; set; }
        [JsonPropertyName("isMaximized")]
        public bool IsMaximized { get; set; }
    }

    public static class WindowManagerExtensions
    {
        public static Dictionary<string, SerializableWindowConfiguration> ConvertWindowConfigurationToSerializable(IWindowConfiguration[] windowConfigurations)
        {
            var result = new Dictionary<string, SerializableWindowConfiguration>();

            foreach (IWindowConfiguration windowConfiguration in windowConfigurations)
            {
                result[windowConfiguration.Type!] = new SerializableWindowConfiguration
                {
                    Left = windowConfiguration.Left,
                    Top = windowConfiguration.Top,
                    Width = windowConfiguration.Width,
                    Height = windowConfiguration.Height,
                    IsMaximized = windowConfiguration.IsMaximized
                };
            }

            return result;
        }

        public static IWindowConfiguration[] ConvertWindowConfigurationFromSerializable(Dictionary<string, SerializableWindowConfiguration> windowConfigurations)
        {
            var result = new List<IWindowConfiguration>(windowConfigurations.Count);

            foreach (KeyValuePair<string, SerializableWindowConfiguration> kv in windowConfigurations)
            {
                result.Add(new ReadonlyWindowConfiguration(
                    kv.Key,
                    kv.Value.Left,
                    kv.Value.Top,
                    kv.Value.Width,
                    kv.Value.Height,
                    kv.Value.IsMaximized
                ));
            }

            return result.ToArray();
        }

        public static void Save(IWindowConfiguration[] windowConfigurations, string absoluteFilename)
        {
            Dictionary<string, SerializableWindowConfiguration> serializable = ConvertWindowConfigurationToSerializable(windowConfigurations);

            string result = JsonSerializer.Serialize(serializable, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(absoluteFilename, result, Encoding.UTF8);
        }

        public static void Save(string absoluteFilename)
        {
            var result = WindowManager.GetWindowsConfiguration();
            Save(result, absoluteFilename);
        }

        public static async ValueTask<IWindowConfiguration[]?> Load(string absoluteFilename)
        {
            if (File.Exists(absoluteFilename) == false)
                return null;

            using Stream fs = File.OpenRead(absoluteFilename);

            Dictionary<string, SerializableWindowConfiguration>? deserialized;

            try
            {
                deserialized = await JsonSerializer.DeserializeAsync<Dictionary<string, SerializableWindowConfiguration>>(fs, new JsonSerializerOptions { WriteIndented = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }

            if (deserialized == null)
                return null;

            return ConvertWindowConfigurationFromSerializable(deserialized);
        }

        public static async ValueTask LoadAndSetup(string absoluteFilename)
        {
            var result = await Load(absoluteFilename);
            if (result != null)
                WindowManager.SetWindowsConfiguration(result);
        }
    }
}
