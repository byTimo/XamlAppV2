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
            try
            {
                if (!Directory.Exists(RootDirectory)) Directory.CreateDirectory(RootDirectory);
                if (!Directory.Exists(UpdateFolderPath)) Directory.CreateDirectory(UpdateFolderPath);
                if(path == null) return;
                if (!File.Exists(path)) File.WriteAllText(path, "");
            }
            catch (Exception e)
            {
                Support.Logger.Fatal(e, "FileManager fail on CheckExitstFiles method");
                throw;
            }
        }
    }
}
