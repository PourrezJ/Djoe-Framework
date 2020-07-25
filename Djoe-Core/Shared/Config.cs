using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Shared.Utils
{
    public static class Config
    {
        private static Dictionary<string, object> _entries { get; set; }

        public static void LoadConfig(string path)
        {
            Debug.WriteLine("Loading configuration");

            string content = null;

            try
            {
                content = API.LoadResourceFile(API.GetCurrentResourceName(), path);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"An error occurred while loading the config file, error description: {e.Message}.");
            }

            _entries = new Dictionary<string, object>();

            if (content == null || content.Length == 0)
            {
                Debug.WriteLine("Configuration is EMPTY!");
                return;
            }

            _entries = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(content);

            Debug.WriteLine("Configuration loaded!");
        }

        public static T Get<T>(string key)
        {
            if (_entries == null)
                throw new Exception("ERROR: Config not loaded!");

            if (_entries.ContainsKey(key))
            {
                var val = _entries[key];

                T output;

                if (val != null)
                {
                    if (val is T)
                    {
                        _entries[key] = val;
                        return (T)val;
                    }

                    try
                    {
                        output = (T)Convert.ChangeType(val, typeof(T), CultureInfo.InvariantCulture);
                    }
                    catch (InvalidCastException)
                    {
                        output = (T)Convert.ChangeType(val, typeof(T), CultureInfo.InvariantCulture);
                    }
                    catch (FormatException)
                    {
                        output = (T)Convert.ChangeType(val, typeof(T), CultureInfo.InvariantCulture);
                    }
                    _entries[key] = val;
                }
                else
                {
                    output = (T)val;
                }

                return output;
            }

            return default(T);
        }
    }
}
