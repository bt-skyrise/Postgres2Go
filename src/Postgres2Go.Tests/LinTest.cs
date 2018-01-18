using System;
using Xunit;

namespace Postgres2Go.Tests
{
    public class LinTest
    {
        [Fact]
        public void Run()
        {
            var runner = PostgresRunner.Start(searchPatternOverride: "/tools/pgsql-10.1-linux-binaries/bin");
            System.Diagnostics.Debugger.Break();

            if (runner is IDisposable)
                (runner as IDisposable).Dispose();
        }
    }
}
