using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MultiPackageVersion.Core
{
    public static class StringExtensions
    {
        public static string DoubleQuoted(this string s) => $"\"{s}\"";

        private static string WithEnding(this string s, string ending)
        {
            if (s == null)
            {
                return ending;
            }

            string result = s;

            // Right() is 1-indexed, so include these cases
            // * Append no characters
            // * Append up to N characters, where N is ending length
            for (int i = 0; i <= ending.Length; i++)
            {
                string tmp = result + ending.Right(i);
                if (tmp.EndsWith(ending, StringComparison.Ordinal))
                {
                    return tmp;
                }
            }

            return result;
        }

        private static string Right(this string s, int length)
        {
            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }

            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), length, "Length is less than zero");
            }

            return length < s.Length
                ? s.Substring(s.Length - length)
                : s;
        }

        public static bool IsSubPathOf(this string path, string baseDirPath)
        {
            string normalizedPath = Path.GetFullPath(path.Replace('/', '\\')
                .WithEnding("\\"));

            string normalizedBaseDirPath = Path.GetFullPath(baseDirPath.Replace('/', '\\')
                .WithEnding("\\"));

            return normalizedPath.StartsWith(normalizedBaseDirPath, StringComparison.OrdinalIgnoreCase);
        }

        public static IEnumerable<string> WhereSubPathOfAnyFolderName(this IEnumerable<string> items, params string[] folderNames)
        {
            var list = items.ToList();

            foreach (string folderName in folderNames)
            {
                foreach (string item in list)
                {
                    if (Path.GetDirectoryName(item).IsSubPathOf(folderName))
                    {
                        yield return item;
                    }
                }
            }
        }
    }
}