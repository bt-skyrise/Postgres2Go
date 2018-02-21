using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Postgres2Go.Tests.Helpers.ProcesOutput
{
    public class should_tostring_return_whole_output
    {
        [Theory]
        [InlineData("only stand output",null)]
        [InlineData(null,"only err output")]
        [InlineData("stand output","err output")]
        public void run(string standardOutput, string errorOutput)
        {
            // PREPARE
            var collectionOfstandardOutputs = new List<string>{"STD OUTPUT:", standardOutput};
            var collectionOfErrorOutputs = new List<string>{"ERR OUTPUT:", errorOutput};

            var sut = new Helper.Process.ProcessOutput(collectionOfstandardOutputs, collectionOfErrorOutputs, 0);

            // RUN
            string wholeFormatedOutput = sut.ToString();

            // ASSERT
            wholeFormatedOutput
                .Should()
                .Be(String.Format(@"
===
STD OUTPUT:
{0}

ERR OUTPUT:
{1}
"
                , standardOutput, errorOutput));


        }
    }
}
