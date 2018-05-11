using System;
using Emuses.Services;
using FluentAssertions;
using Xunit;

namespace Emuses.Tests
{
    public class LoggerServiceTests
    {
        [Fact]
        public void write_log()
        {
            ILoggerService loggerService = new LoggerService("ComponentName");

            loggerService.PrintLog("Test...").ComponentName.Should().Be("ComponentName");
            loggerService.PrintLog("Test...").Text.Should().Be("Test...");
            loggerService.PrintLog("Test...").Created.Should().BeCloseTo(DateTime.Now);
            loggerService.PrintLog("Test...").Head.Should().Be("=== ComponentName ====================================================");
            loggerService.PrintLog("Test...").Value.Should().Contain("Test..").And.Contain(":");
        }
    }
}
