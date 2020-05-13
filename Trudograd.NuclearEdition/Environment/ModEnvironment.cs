using System;
using System.IO;
using System.Reflection;

namespace Trudograd.NuclearEdition
{
    internal static class ModEnvironment
    {
        public static String DirectoryAbsolutePath { get; }

        static ModEnvironment()
        {
            DirectoryAbsolutePath = GetModDirectory();
        }

        private static String GetModDirectory()
        {
            String codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            String path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }
    }
}