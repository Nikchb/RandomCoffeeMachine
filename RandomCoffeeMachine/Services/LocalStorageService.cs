using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RandomCoffeeMachine.Services
{
    public static class LocalStorageService
    {
        private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            Formatting = Formatting.Indented
        };

        public static void Save<T>(T data, string fileName) where T : new()
        {
            var filePath = $"storage/{fileName}";
            File.WriteAllText(filePath, JsonConvert.SerializeObject(data, _settings));
        }
        public static T Load<T>(string fileName) where T : new()
        {
            var filePath = $"storage/{fileName}";
            if (File.Exists(filePath))
            {
                return JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath), _settings);
            }
            return new T();
        }
        public static object Load(string fileName)
        {
            var filePath = $"storage/{fileName}";
            if (File.Exists(filePath))
            {
                return JsonConvert.DeserializeObject(File.ReadAllText(filePath), _settings);
            }
            return null;
        }

        public static async Task SaveAsync<T>(T data, string fileName) where T : new()
        {
            var filePath = $"storage/{fileName}";
            await File.WriteAllTextAsync(filePath, JsonConvert.SerializeObject(data, _settings));
        }
        public static async Task<T> LoadAsync<T>(string fileName) where T : new()
        {
            var filePath = $"storage/{fileName}";
            if (File.Exists(filePath))
            {
                return JsonConvert.DeserializeObject<T>(await File.ReadAllTextAsync(filePath), _settings);
            }
            return new T();
        }
        public static async Task<object> LoadAsync(string fileName)
        {
            var filePath = $"storage/{fileName}";
            if (File.Exists(filePath))
            {
                return JsonConvert.DeserializeObject(await File.ReadAllTextAsync(filePath), _settings);
            }
            return null;
        }
    }
}
