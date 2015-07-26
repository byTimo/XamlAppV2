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
            SettingContent readingSetting;
            var serializer = new XmlSerializer(typeof(SettingContent));
            using (var stream = File.Open(settingFilePath, FileMode.Open))
            {
                readingSetting = (SettingContent)serializer.Deserialize(stream);
                Support.Logger.Trace("Successful reading of settings.");
            }
            if (readingSetting == null)
            {
                Support.Logger.Trace("The settings file is empty.");
                readingSetting = InitializeContent();
                Support.Logger.Trace("Exhibited default settings.");
            }
            return readingSetting;
        }

        private static SettingContent InitializeContent()
        {
            var newSettingContent = new SettingContent();
            SaveSettings(newSettingContent);
            return new SettingContent();
        }
    }

    [TestFixture]
    public class SettingTests
    {
        private Settings.SettingContent realSettingContent;

        [SetUp]
        public void SetUp()
        {
            realSettingContent = GetSettingNativeWay();
        }

        [TearDown]
        public void TestDown()
        {
            SetSettingNativeWay(realSettingContent);
        }

        [Test]
        public void GetterSettingTest()
        {
            Support.Logger.Trace("Start GetterSettingTest method");
            var currentSetting = Settings.Current;
            Assert.NotNull(currentSetting);
            Support.Logger.Trace("GetterSettingTest method successful");

        }

        [Test]
        public void SavingSettingTest()
        {
            Support.Logger.Trace("Start GetterSettingTest method");
            var currrentSetting = Settings.Current;

            currrentSetting.InDeviceNumber = 5;
            Settings.SaveSettings();
            var nativeContent = GetSettingNativeWay();

            Assert.AreEqual(nativeContent.InDeviceNumber, 5);
            Support.Logger.Trace("GetterSettingTest method successful");
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