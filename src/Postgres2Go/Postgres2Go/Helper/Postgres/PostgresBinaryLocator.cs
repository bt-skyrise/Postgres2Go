using System;
using System.IO;
using System.Runtime.InteropServices;
using Postgres2Go.Common;
using Postgres2Go.Helper.FileSystem;

namespace Postgres2Go.Helper.Postgres
{

    internal class PostgresBinaryLocator
    {
        internal const string DefaultWindowsSearchPattern = @"tools\pqsql-win64*\bin";
        internal const string DefaultLinuxSearchPattern = "tools/pqsql-linux*/bin";
        internal const string DefaultOsxSearchPattern = "tools/pqsql-osx*/bin";
        
        private readonly string _nugetPrefix = Path.Combine("packages", "Postgres2Go*");
        private string _binFolder = string.Empty;
        private readonly string _searchPattern;

        public PostgresBinaryLocator(string searchPatternOverride)
        {
            if (string.IsNullOrEmpty(searchPatternOverride))
            {
                switch (RecognizedOSPlatform.Determine())
                {
                    case RecognizedOSPlatformEnum.Linux:
                        _searchPattern = DefaultLinuxSearchPattern;
                        break;
                    case RecognizedOSPlatformEnum.OSX:
                        _searchPattern = DefaultOsxSearchPattern;
                        break;
                    case RecognizedOSPlatformEnum.Windows:
                        _searchPattern = DefaultWindowsSearchPattern;
                        break;
                    default:
                        throw new PostgresBinariesNotFoundException($"Unknow OS:{RuntimeInformation.OSDescription}");
                        
                }
            }
            else
            {
                _searchPattern = searchPatternOverride;
            }
        }

        internal string Directory 
            => String.IsNullOrEmpty(_binFolder) 
                ? _binFolder = ResolveBinariesDirectory () 
                : _binFolder
                ;

        private string ResolveBinariesDirectory()
        {
            var binariesFolder =
                FolderSearch.CurrentExecutingDirectory().FindFolderUpwards(Path.Combine(_nugetPrefix, _searchPattern)) 
                ??
                FolderSearch.CurrentExecutingDirectory().FindFolderUpwards(_searchPattern);

            if (binariesFolder == null)
                throw new PostgresBinariesNotFoundException ($"Could not find Postgres binaries using the search pattern of {_searchPattern}. The Searching began in {FolderSearch.CurrentExecutingDirectory()}");

            return binariesFolder;
        }
    }
}
