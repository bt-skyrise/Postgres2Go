using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Postgres2Go.Tests.Helpers.ProcesOutput
{
    public class procesoutput_toString
    {
        [Theory]
        [InlineData("only stand output",null)]
        [InlineData(null,"only err output")]
        [InlineData("stand output","err output")]
        public void should_return_concatenation_of_outputs(string standardOutput, string errorOutput)
        {
            // PREPARE
            var collectionOfstandardOutputs = new List<string>{"STD OUTPUT:", standardOutput};
            var collectionOfErrorOutputs = new List<string>{"ERR OUTPUT:", errorOutput};

            var sut = new Helper.Process.ProcessOutput(collectionOfstandardOutputs, collectionOfErrorOutputs, 0);

            // RUN
            string wholeFormatedOutput = sut.ToString();

            // ASSERT
            
            string expectation = $"{Environment.NewLine}" +
                                 $"==={Environment.NewLine}" +
                                 $"STD OUTPUT:{Environment.NewLine}{standardOutput}{Environment.NewLine}" +
                                 $"{Environment.NewLine}" +
                                 $"ERR OUTPUT:{Environment.NewLine}{errorOutput}{Environment.NewLine}";

            wholeFormatedOutput.Should().Be(expectation);

        }
    }
}
