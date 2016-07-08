using System;
using System.IO;
using System.Threading;
using HelpersLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MainLib.AppSettings
{
    public class ApplicationConfig
    {
        private static ApplicationConfig _applicationConfig;

        public GeneralSettings GeneralSettings;
        public ImageSettings ImageSettings;
        public HotKeysConfig HotKeysConfig;

        public static ApplicationConfig GetDefaultSettings()
        {
            LazyInitializer.EnsureInitialized(ref _applicationConfig,
                () => new ApplicationConfig
                {
                    GeneralSettings = new GeneralSettings { PlaySoundAfterPrintScreen = true, Language = SupportedLanguage.Automatic },
                    ImageSettings = new ImageSettings { PictureFormat = "PNG", PictureSaveFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) },
                    HotKeysConfig = new HotKeysConfig { IgnoreHotkeys = false, Hotkeys = HotkeyManager.GetDefaultHotkeyList() }
                });

            return _applicationConfig;
        }

        public static ApplicationConfig Load(string filePath)
        {

            if (!string.IsNullOrEmpty(filePath))
            {
                if (File.Exists(filePath))
                {
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        if (fileStream.Length > 0)
                        {
                            ApplicationConfig settings;

                            using (StreamReader streamReader = new StreamReader(fileStream))
                            using (JsonTextReader jsonReader = new JsonTextReader(streamReader))
                            {
                                JsonSerializer serializer = new JsonSerializer();
                                serializer.Converters.Add(new StringEnumConverter());
                                serializer.ObjectCreationHandling = ObjectCreationHandling.Replace;
                                serializer.Error += (sender, e) => e.ErrorContext.Handled = true;
                                settings = serializer.Deserialize<ApplicationConfig>(jsonReader);
                            }

                            if (settings == null)
                            {
                                throw new Exception("Application Config object is null.");
                            }

                            return settings;
                        }
                    }
                }
            }

            return GetDefaultSettings();
        }

        public void Save(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                string tempFilePath = filePath + ".temp";

                using (FileStream fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write, FileShare.Read))
                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                using (JsonTextWriter jsonWriter = new JsonTextWriter(streamWriter))
                {
                    jsonWriter.Formatting = Formatting.Indented;
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.ContractResolver = new WritablePropertiesOnlyResolver();
                    serializer.Converters.Add(new StringEnumConverter());
                    serializer.Serialize(jsonWriter, this);
                    jsonWriter.Flush();
                }

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                File.Move(tempFilePath, filePath);
            }
        }
    }
}
