using System;
using System.IO;
using Xunit;
using Postgres2Go.Helper.FileSystem;
using Postgres2Go.Helper.Postgres;

namespace Postgres2Go.Tests.Helpers.FolderSearch
{
    public class when_invoke_FindFolderUpwards : IDisposable
    {
        private readonly Fixture _fixture;

        public when_invoke_FindFolderUpwards()
        {
            _fixture = new Fixture();
        }

        public void Dispose()
        {
            _fixture?.Dispose();
        }

        [Theory]
        [InlineData("a")]
        [InlineData(@"a\b")]
        [InlineData(@"a\b\c")]
        [InlineData(@"a\b\c\d")]
        [InlineData(@"a\b\c\d\e")]
        public void should_locate_directory_with_default_search_pattern(string executionPath)
        {
            // PREPARE
            _fixture.CreateSubDirectoryIfNotExists(
                Path.Combine(@"packages\Postgres2Go-0.3.0", @"pg-dist\pgsql-10.1-windows64-binaries\bin")
            );

            string executionAbsPath = _fixture
                .CreateSubDirectoryIfNotExists(executionPath);

            string searchLocation = Path.Combine(
                PostgresBinaryLocator.NugetPackagesDirectoryPrefix,
                PostgresBinaryLocator.DefaultWindowsSearchPattern
            );

            // RUN
            string foundLocation = executionAbsPath
                .FindFolderUpwards(searchLocation);

            // ASSERT
            Assert.NotNull(foundLocation);
        }

        [Theory]
        [InlineData("a", @"packa*\Postgres2*\pg-dist\pgsql-*\bin")]
        [InlineData(@"a\b", @"packa*\Postgres2*\pg-dist\pgsql-*\bin")]
        [InlineData(@"a\b\c\d\e", @"packa*\Postgres2*\pg-dist\pgsql-*\bin")]
        public void should_locate_directory_using_custom_search_pattern(string executionPath, string searchPattern)
        {
            // PREPARE
            _fixture.CreateSubDirectoryIfNotExists(
                Path.Combine(@"packages\Postgres2Go-0.3.0", @"pg-dist\pgsql-10.1-windows64-binaries\bin")
            );

            string executionAbsPath = _fixture
                .CreateSubDirectoryIfNotExists(executionPath);
            
            // RUN
            string foundLocation = executionAbsPath
                .FindFolderUpwards(searchPattern);

            // ASSERT
            Assert.NotNull(foundLocation);
        }

        [Theory]
        [InlineData(@"a\b\c\d\e\f")]
        [InlineData(@"a\b\c\d\e\f\g")]
        public void cannot_locate_directory_when_nest_level_greater_than_5(string executionPath)
        {
            // PREPARE
            _fixture.CreateSubDirectoryIfNotExists(
                Path.Combine(@"packages\Postgres2Go-0.3.0", @"pg-dist\pgsql-10.1-windows64-binaries\bin")
            );

            string executionAbsPath = _fixture
                .CreateSubDirectoryIfNotExists(executionPath);

            string searchLocation = Path.Combine(
                PostgresBinaryLocator.NugetPackagesDirectoryPrefix,
                PostgresBinaryLocator.DefaultWindowsSearchPattern
            );

            // RUN
            string foundLocation = executionAbsPath
                .FindFolderUpwards(searchLocation);

            // ASSERT
            Assert.Null(foundLocation);

        }

        class Fixture : IDisposable
        {
            internal string Root { get; private set; }

            public Fixture()
            {
                Root = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

                if (!Directory.Exists(Root))
                    Directory.CreateDirectory(Root);
            }

            public string CreateSubDirectoryIfNotExists(string path)
            {
                var subDirPath = Path.Combine(Root, path);

                if (!Directory.Exists(subDirPath))
                    Directory.CreateDirectory(subDirPath);

                return subDirPath;
            }

            public void Dispose()
            {
                if (!Directory.Exists(Root))
                    Directory.Delete(Root);
            }
        }
    }
}
