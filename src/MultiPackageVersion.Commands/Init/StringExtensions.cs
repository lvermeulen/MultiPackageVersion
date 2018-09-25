using System;
using System.IO;

namespace MultiPackageVersion.Commands.Init
{
    public static class StringExtensions
    {
        private static string ConvertForwardSlashesToBackSlashes(string s)
        {
            return string.IsNullOrEmpty(s)
                ? s
                : s.Replace('/', '\\');
        }

        private static string GetDrive(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            s = ConvertForwardSlashesToBackSlashes(s);

            int colonPos = s.IndexOf(':');
            int slashPos = s.IndexOf('\\');

            if (colonPos <= 0)
            {
                return string.Empty;
            }

            if (slashPos < 0 || slashPos > colonPos)
            {
                return s.Substring(0, colonPos + 1);
            }

            return string.Empty;
        }

        private static string GetShare(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            string str = s;

            // look for \\ (could be "\\server\share" or "http:\\www.xyz.com\")
            const string DOUBLE_BACKSLASH = @"\\";
            int n = str.IndexOf(DOUBLE_BACKSLASH, StringComparison.InvariantCultureIgnoreCase);
            if (n < 0)
            {
                return string.Empty;
            }

            string ret = str.Substring(0, n + DOUBLE_BACKSLASH.Length);
            str = str.Remove(0, n + DOUBLE_BACKSLASH.Length);

            const char SINGLE_BACKSLASH = '\\';
            n = str.IndexOf(SINGLE_BACKSLASH);
            if (n <= 0)
            {
                return string.Empty;
            }

            ret += str.Substring(0, n + 1);
            str = str.Remove(0, n + 1);

            n = str.IndexOf(SINGLE_BACKSLASH);
            if (n < 0)
            {
                n = str.Length;
            }
            else if (n == 0)
            {
                return string.Empty;
            }

            ret += str.Substring(0, n);

            string result = ret[ret.Length - 1] == SINGLE_BACKSLASH
                ? string.Empty
                : ret;
            return result;
        }

        private static string GetDriveOrShare(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            if (!string.IsNullOrEmpty(GetDrive(s)))
            {
                return GetDrive(s);
            }

            bool local = !string.IsNullOrEmpty(GetShare(s));
            return local
                ? GetShare(s)
                : string.Empty;
        }

        private static string GetFileNameWithoutDriveOrNetworkShare(this string s) =>
            s.Substring(GetDriveOrShare(s).Length);

        public static string GetDirectoryNameWithoutDriveOrNetworkShare(this string s) =>
            Path.GetDirectoryName(s.GetFileNameWithoutDriveOrNetworkShare());

        public static string ToGlob(this string fileName, string rootFolderName = null)
        {
            string folderName = fileName.GetDirectoryNameWithoutDriveOrNetworkShare();
            if (folderName == null)
            {
                return null;
            }

            if (rootFolderName != null)
            {
                rootFolderName = rootFolderName.GetFileNameWithoutDriveOrNetworkShare();
                int rootFolderNameIndex = folderName.IndexOf(rootFolderName, StringComparison.InvariantCultureIgnoreCase);
                folderName = folderName.Substring(rootFolderNameIndex + rootFolderName.Length);
            }

            if (!folderName.StartsWith(new string(new[] { Path.DirectorySeparatorChar }), StringComparison.InvariantCultureIgnoreCase))
            {
                folderName = $@"{Path.DirectorySeparatorChar}{folderName}";
            }
            if (!folderName.EndsWith(new string(new[] { Path.DirectorySeparatorChar }), StringComparison.InvariantCultureIgnoreCase))
            {
                folderName = $@"{folderName}{Path.DirectorySeparatorChar}";
            }

            return $@"**{folderName}*";
        }
    }
}
