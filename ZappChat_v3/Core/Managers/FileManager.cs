using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using NUnit.Framework;

namespace ZappChat_v3.Core.Managers
{
    internal static class FileManager
    {
        private static readonly string RootDirectory;
        private static readonly string SettingPath;
        private static readonly string UpdateFolderPath;
        private const string ZappChatDirectoryName = "ZappChat_v3";
        private const string SettingFileName = "Setting";
        private const string UpdateDirectoryName = "Update";

        public static string FullPathToSettingFile
        {
            get
            {
                CheckExistsFiles(SettingPath);
                return SettingPath;
            }
        }

        public static string FullPathToUpdateFolder
        {
            get
            {
                CheckExistsFiles();
                return UpdateFolderPath;
            }
        }

        static FileManager()
        {
            var appDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            if (appDataDirectory == null) throw new Exception("Не возможно определить расположение ApplicationsData");

            RootDirectory = Path.Combine(appDataDirectory, ZappChatDirectoryName);
            UpdateFolderPath = Path.Combine(RootDirectory, UpdateDirectoryName);
            SettingPath = Path.Combine(RootDirectory, SettingFileName);
        }

        public static void CheckExistsFiles(string path = null)
        {
            if (!Directory.Exists(RootDirectory)) Directory.CreateDirectory(RootDirectory);
            if (!Directory.Exists(UpdateFolderPath)) Directory.CreateDirectory(UpdateFolderPath);
            if(path == null) return;
            if (!File.Exists(path)) File.WriteAllText(path, "");
        }

        public static string FindFieldInfoLineInFile(string path, string field)
        {
            if (!File.Exists(path)) File.Create(path);
            return GetAllLineInFile(path).FirstOrDefault(str => str.StartsWith(field));
        }

        public static bool SaveSettings(string field, string setting)
        {
            return SaveInformationToFile(FullPathToSettingFile, field, setting);
        }

        /// <summary>
        /// Возвращает значение поля настроек в строковом формате или возращает null.
        /// </summary>
        /// <param name="field">Поле настроек.</param>
        public static string GetSetting(string field)
        {
            return GetFieldInfoInFile(FullPathToSettingFile, field);
        }

        public static void DeleteSetting(string field)
        {

            try
            {
                var allSettings = GetAllLineInFile(FullPathToSettingFile);
                var currentLine = FindFieldInfoLineInFile(FullPathToSettingFile, field);
                if (currentLine != null && allSettings.Contains(currentLine))
                {
                    allSettings.Remove(currentLine);
                    RewriteFile(FullPathToSettingFile, allSettings);
                }
                else
                    throw new Exception();
            }
            catch
            {
                // ignored
            }
        }

        private static string GetFieldInfoInFile(string path, string field)
        {
            var lineInfo = GetAllLineInFile(path)
                .FirstOrDefault(str => str.StartsWith(field));
            return lineInfo == null
                ? null
                : lineInfo.Split(':')[1];
        }

        private static bool SaveInformationToFile(string path, string field, string info)
        {
            try
            {
                var allLineFile = GetAllLineInFile(FullPathToSettingFile);
                var currentLine = allLineFile.FirstOrDefault(str => str.StartsWith(field));
                if (currentLine != null) allLineFile.Remove(currentLine);
                allLineFile.Add(string.Concat(field, ":", info));
                RewriteFile(path, allLineFile);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка при сохранении настроек!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private static List<string> GetAllLineInFile(string path)
        {
            if (!File.Exists(path)) File.Create(path);
            Thread.Sleep(1000);
            return File.ReadAllText(path).Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        private static void RewriteFile(string path, IEnumerable<string> allLine)
        {
            if (File.Exists(path)) File.Delete(path);
            File.WriteAllLines(path, allLine);
        }
    }
}
