using Newtonsoft.Json;
using System.IO;

namespace TestAliexpressPage.Configuration
{
    public class LocalFileConfiguration<T> where T : class, new()
    {
        private readonly string _configurationFilePath;

        public LocalFileConfiguration(string configurationFileName = "local_setting.json", string fileDirectory = "")
        {
            var _fileDirectory = string.IsNullOrEmpty(fileDirectory) ? Path.GetTempPath() : fileDirectory;
            _configurationFilePath = Path.Combine(_fileDirectory, configurationFileName);
        }

        public T Get()
        {
            if (!File.Exists(_configurationFilePath))
            {
                return new T();
            }

            var fileContent = File.ReadAllText(_configurationFilePath);
            var objectResult = JsonConvert.DeserializeObject<T>(fileContent);

            return objectResult;
        }

        public void Save(T value)
        {
            var _value = JsonConvert.SerializeObject(value);
            File.WriteAllText(_configurationFilePath, _value);
        }
    }
}
