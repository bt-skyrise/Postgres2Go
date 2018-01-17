﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Postgres2Go.Helper.FileSystem
{
    internal static class FolderSearch
    {
        internal const int MaxLevelOfRecursion = 6;

        internal static string CurrentExecutingDirectory()
        {
            string filePath = new Uri(typeof(FolderSearch).GetTypeInfo().Assembly.CodeBase).LocalPath;
            return Path.GetDirectoryName(filePath);
        }

        internal static string FindFolder(this string startPath, string searchPattern)
        {
            string currentPath = startPath;
            string subPathToLookup = searchPattern.StartsWith(Path.DirectorySeparatorChar.ToString())
                ? searchPattern.Substring(1,searchPattern.Length - 1) 
                : searchPattern
                ;

            foreach (var part in subPathToLookup.Split(new[] { Path.DirectorySeparatorChar }, StringSplitOptions.None))
            {
                string[] matchesDirectory = Directory.GetDirectories(currentPath, part);
                if (!matchesDirectory.Any())
                {
                    return null;
                }
                currentPath = matchesDirectory.OrderBy(x => x).Last();
            }

            return currentPath;
        }

        internal static string FindFolderUpwards(this string startPath, string searchPattern)
        {
            return FindFolderUpwards(startPath, searchPattern, 0);
        }
        
        private static string FindFolderUpwards(this string startPath, string searchPattern, int currentLevel)
        {
            if (startPath == null)
            {
                return null;
            }

            if (currentLevel >= MaxLevelOfRecursion)
            {
                return null;
            }

            string matchingFolder = startPath.FindFolder(searchPattern);
            return matchingFolder ?? startPath.RemoveLastPart().FindFolderUpwards(searchPattern, currentLevel + 1);
        }

        internal static string RemoveLastPart(this string path)
        {
            if (!path.Contains(Path.DirectorySeparatorChar))
            {
                return null;
            }

            List<string> parts = path.Split(new[] { Path.DirectorySeparatorChar }, StringSplitOptions.None).ToList();
            parts.RemoveAt(parts.Count() - 1);
            return string.Join(Path.DirectorySeparatorChar.ToString(), parts.ToArray());
        }

        /// <summary>
        /// Absolute path stays unchanged, relative path will be relative to current executing directory (usually the /bin folder)
        /// </summary>
        internal static string FinalizePath(string fileName)
        {
            string finalPath;

            if (Path.IsPathRooted(fileName))
            {
                finalPath = fileName;
            }
            else
            {
                finalPath = Path.Combine (CurrentExecutingDirectory (), fileName);
                finalPath = Path.GetFullPath(finalPath);
            }

            return finalPath;
        }
    }
}
