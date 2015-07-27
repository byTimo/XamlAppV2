using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
            try
            {
                var appDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                RootDirectory = Path.Combine(appDataDirectory, ZappChatDirectoryName);
                UpdateFolderPath = Path.Combine(RootDirectory, UpdateDirectoryName);
                SettingPath = Path.Combine(RootDirectory, SettingFileName);
            }
            catch (Exception e)
            {
                Support.Logger.Fatal(e, "FileManager fail on static designer");
            }
        }

        public static void CheckExistsFiles(string path = null)
        {
            try
            {
                if (!Directory.Exists(RootDirectory))
                {
                    Directory.CreateDirectory(RootDirectory);
                    Support.Logger.Info("{0} is created", RootDirectory);
                }
                if (!Directory.Exists(UpdateFolderPath))
                {
                    Directory.CreateDirectory(UpdateFolderPath);
                    Support.Logger.Info("{0} is created", UpdateFolderPath);
                }
                if(path == null) return;
                if (!File.Exists(path))
                {
                    File.WriteAllText(path,null);
                }
            }
            catch (Exception e)
            {
                Support.Logger.Fatal(e, "FileManager fail on CheckExitstFiles method");
                throw;
            }
        }
    }
}
