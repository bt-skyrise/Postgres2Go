using System;
using Xunit;

namespace Postgres2Go.Tests
{
    public class WinTest
    {
        [Fact]
        public void Run()
        {
            var runner = PostgresRunner.Start(searchPatternOverride: "\\tools\\pgsql-10.1-windows64-binaries\\bin");
            System.Diagnostics.Debugger.Break();

            if (runner is IDisposable)
                (runner as IDisposable).Dispose();
        }
    }
}
