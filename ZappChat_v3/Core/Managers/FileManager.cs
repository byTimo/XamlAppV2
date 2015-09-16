using System;
using System.IO;

namespace ZappChat_v3.Core.Managers
{
    /// <summary>
    /// Класс, управляющий файлами приложения
    /// </summary>
    internal static class FileManager
    {
        private static string _rootDirectory;
        private static string _updateFolderPath;
        private static string _profileCache;
        private static string _commontSettings;

        private const string ZappChatDirectoryName = "ZappChat_v3";
        private const string UpdateDirectoryName = "UpdateCache";
        private const string ProfileCacheDirectoryName = "ProfileCache";
        private const string CommonSettingsFile = "settings";

        
        private static string _profileFolder;

        /// <summary>
        /// Получить или назначить пакпку текущего профиля
        /// </summary>
        public static string ProfileFolder
        {
            get
            {
                if(_profileFolder == null) throw new NullReferenceException("Не указано имя папки профиля!");
                var fullPathToProfileFolder = Path.Combine(ProfileCache, _profileFolder);
                CheckFolder(fullPathToProfileFolder);
                return fullPathToProfileFolder;
            }
            set { _profileFolder = value; }
        }
        /// <summary>
        /// Получить путь к корневой папке приложения
        /// </summary>
        public static string RootDirectory
        {
            get
            {
                CheckFolder(_rootDirectory);
                return _rootDirectory;
            }
            private set { _rootDirectory = value; }
        }
        /// <summary>
        /// Получить путь к папке с обновлениями
        /// </summary>
        public static string UpdateFolderPath
        {
            get
            {
                CheckFolder(_updateFolderPath);
                return _updateFolderPath;
            }
            private set { _updateFolderPath = value; }
        }
        /// <summary>
        /// Получить путь к общей папке всех профилей
        /// </summary>
        public static string ProfileCache
        {
            get
            {
                CheckFolder(_profileCache);
                return _profileCache;
            }
            private set { _profileCache = value; }
        }
        /// <summary>
        /// Получить путь к файлу общих настроек
        /// </summary>
        public static string CommoSettingsFile
        {
            get
            {
                CheckFile(_commontSettings);
                return Path.Combine(_rootDirectory);
            } 
            private set { _commontSettings = value; }
        }
        static FileManager()
        {
            try
            {
                var appDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                RootDirectory = Path.Combine(appDataDirectory, ZappChatDirectoryName);
                UpdateFolderPath = Path.Combine(RootDirectory, UpdateDirectoryName);
                ProfileCache = Path.Combine(RootDirectory, ProfileCacheDirectoryName);
                CommoSettingsFile = Path.Combine(RootDirectory, CommonSettingsFile);
            }
            catch (Exception e)
            {
                Support.Logger.Fatal(e, "FileManager fail on static designer");
                throw;
            }
        }
        /// <summary>
        /// Проверяет, существует ли папка. Если не существует - она создаётся
        /// </summary>
        /// <param name="path">Путь до папки</param>
        public static void CheckFolder(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    Support.Logger.Info("FileManager created folder {0}", path);
                }
            }
            catch (Exception e)
            {
                Support.Logger.Fatal(e, "FileManager could not create folder with path {0}", path);
                throw;
            }
        }

        /// <summary>
        /// Проверяет, существует ли файл. Если не существует - он создаётся
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        public static void CheckFile(string path)
        {
            try
            {
                var indexFile = path.LastIndexOf('\\');
                if (indexFile == 0) throw new ArgumentException("Невозможно отделить название файла от дериктории");
                var directory = path.Substring(0, indexFile);
                var fileName = path.Substring(indexFile + 1);
                CheckFile(fileName, directory);
            }
            catch (Exception e)
            {
                Support.Logger.Fatal(e, "FileManager could not create file with path {0}", path);
                throw;
            }
        }

        /// <summary>
        /// Проверяет, существует ли файл. Если не существует - он создаётся
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        /// <param name="directory">Директория файла</param>
        public static void CheckFile(string fileName, string directory)
        {
            try
            {
                var fullPath = Path.Combine(directory, fileName);
                CheckFolder(directory);
                if (!File.Exists(fullPath))
                {
                    File.WriteAllText(fullPath,null);
                    Support.Logger.Info("FileManager created file {0}", fullPath);
                }
            }
            catch (Exception e)
            {
                Support.Logger.Fatal(e, "FileManager could not create file {0}", fileName);
                throw;
            }
        }
    }
}
