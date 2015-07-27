using System;
using System.IO;
using System.Xml.Serialization;
using NUnit.Framework;
using ZappChat_v3.Core.Managers;

namespace ZappChat_v3.Core
{
    /// <summary>
    /// Класс описывает общие настройки приложения.
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// Вспомогательный класс, отображающий внутренню структуру файла настроек
        /// </summary>
        public class SettingContent
        {
            public int InDeviceNumber { get; set; }
            public int OutDeviceNumber { get; set; }
        }

        private static SettingContent currentSetting;

        /// <summary>
        /// Получить текущие настройки
        /// </summary>
        public static SettingContent Current
        {
            get
            {
                if (currentSetting != null)
                    return currentSetting;
                currentSetting = ReadSettingFromFile(FileManager.FullPathToSettingFile);
                return currentSetting;
            }
        }
        /// <summary>
        /// Сохраняет текущие настройки в файл настроек.
        /// </summary>
        public static void SaveSettings()
        {
            if (currentSetting == null)
            {
                Support.Logger.Warn("Trying to save the setting with an empty object.");
                currentSetting = ReadSettingFromFile(FileManager.FullPathToSettingFile);
            }
            var serializer = new XmlSerializer(typeof(SettingContent));
            using (var stream = File.Open(FileManager.FullPathToSettingFile, FileMode.Create))
            {
               serializer.Serialize(stream, currentSetting); 
            }
            Support.Logger.Trace("Successful save setting");
        }
        /// <summary>
        /// Схораняет настройки, переданные в SettingContent
        /// </summary>
        /// <param name="settingContent">Схораняемые настройки</param>
        public static void SaveSettings(SettingContent settingContent)
        {
            currentSetting = settingContent;
            SaveSettings();
        }

        private static SettingContent ReadSettingFromFile(string settingFilePath)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(SettingContent));
                SettingContent readingSetting;
                using (var stream = new FileStream(settingFilePath, FileMode.Open))
                {
                    try
                    {
                        readingSetting = (SettingContent)serializer.Deserialize(stream);
                    }
                    catch (InvalidOperationException ioe)
                    {
                        Support.Logger.Trace(ioe, "The settings file is empty.");
                        try
                        {
                            Support.Logger.Info("Trying create new settings in file");
                            readingSetting = InitializeContent(stream);
                        }
                        catch (Exception e)
                        {
                            Support.Logger.Fatal(e, "Trying create new settings is failed\n");
                            throw;
                        }
                        Support.Logger.Trace("Exhibited default settings.");
                    }
                    Support.Logger.Trace("Successful reading of settings.");
                }
                return readingSetting;
            }
            catch (Exception e)
            {
                Support.Logger.Fatal(e,"PeripheryManager fail on ReadSettinFromFile method\n");
                throw;
            }
        }

        private static SettingContent InitializeContent(FileStream settingFileStream)
        {
            var newSettingContent = new SettingContent();
            new XmlSerializer(typeof(SettingContent)).Serialize(settingFileStream, newSettingContent);
            return new SettingContent();
        }
    }

    [TestFixture]
    public class SettingTests
    {
        [Test]
        public void GetterSettingTest()
        {
            Support.Logger.Trace("Start GetterSettingTest method");
            var currentSetting = Settings.Current;
            try
            {
                Assert.NotNull(currentSetting);
            }
            catch (Exception e)
            {
                Support.Logger.Warn(e, "GetterSetting unit test is failed");
                throw;
            }
            Support.Logger.Trace("GetterSettingTest method successful");

        }

        [Test]
        public void SavingSettingTest()
        {
            Support.Logger.Trace("Start SavingSettingTest method");
            var currrentSetting = Settings.Current;

            currrentSetting.InDeviceNumber = 5;
            Settings.SaveSettings();
            var nativeContent = GetSettingNativeWay();

            try
            {
                Assert.AreEqual(nativeContent.InDeviceNumber, 5);
            }
            catch (Exception e)
            {
                Support.Logger.Warn(e, "SavingSetting unit test is failed");
                throw;
            }
            Support.Logger.Trace("SavingSettingTest method successful");
        }

        private Settings.SettingContent GetSettingNativeWay()
        {
            Settings.SettingContent settingContent;
            var serialazer = new XmlSerializer(typeof (Settings.SettingContent));
            using (var stream = File.Open(FileManager.FullPathToSettingFile, FileMode.Open))
            {
                settingContent = (Settings.SettingContent) serialazer.Deserialize(stream);
            }
            return settingContent;
        }

        private void SetSettingNativeWay(Settings.SettingContent settingContent)
        {
            var serialazer = new XmlSerializer(typeof(Settings.SettingContent));
            using (var stream = File.Open(FileManager.FullPathToSettingFile, FileMode.Create))
            {
                serialazer.Serialize(stream, settingContent);
            }
        }
    }
}