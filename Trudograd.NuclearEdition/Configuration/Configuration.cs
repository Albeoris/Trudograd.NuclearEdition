using System;
using System.IO;
using UnityEngine;

namespace Trudograd.NuclearEdition
{
    public static class Configuration
    {
        private static RootConfiguration _instance = new RootConfiguration();

        public static String ConfigurationFilePath { get; }

        public static BombaganConfiguration Bombagan => _instance.Bombagan;
        public static DialogConfiguration Dialog => _instance.Dialog;
        public static ScannerConfiguration Scanner => _instance.Scanner;

        static Configuration()
        {
            try
            {
                ConfigurationFilePath = Path.GetFullPath(Path.Combine(ModEnvironment.DirectoryAbsolutePath, "Config.txt")).Replace('\\', '/');

                The<ModDirectoryWatcher>.Instance.FileChanged += OnFileChanged;
                The<ModDirectoryWatcher>.Instance.FileDeleted += OnFileDeleted;

                SyncConfiguration();
            }
            catch (Exception ex)
            {
                Debug.LogError($"[{nameof(NuclearEdition)}] Failed to initialize configuration.");
                Debug.LogException(ex);
            }
        }

        private static void OnFileDeleted(String filename)
        {
            if (filename != ConfigurationFilePath)
                return;
            
            Debug.Log($"[{nameof(NuclearEdition)}] The configuration file has been deleted. Reset configuration.");
            _instance = new RootConfiguration();
            WriteConfiguration();
        }

        private static void OnFileChanged(String filename, Stream stream)
        {
            if (filename != ConfigurationFilePath)
                return;

            Debug.Log($"[{nameof(NuclearEdition)}] The configuration file has been changed. Loading: {ConfigurationFilePath}");
            using (var input = new StreamReader(stream))
                ConfigurationSerializer.Read(_instance, input);
        }

        private static void SyncConfiguration()
        {
            if (File.Exists(ConfigurationFilePath))
            {
                Debug.Log($"[{nameof(NuclearEdition)}] Load configuration: {ConfigurationFilePath}");
                using (var input = File.OpenText(ConfigurationFilePath))
                    ConfigurationSerializer.Read(_instance, input);
            }

            WriteConfiguration();
        }

        private static void WriteConfiguration()
        {
            Debug.Log($"[{nameof(NuclearEdition)}] Write configuration: {ConfigurationFilePath}");
            using (var output = File.CreateText(ConfigurationFilePath))
                ConfigurationSerializer.Write(_instance, output);
        }
    }
}