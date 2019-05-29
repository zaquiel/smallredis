using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmallRedis.Test
{
    public class ZrangeTest : BaseTest
    {
        [SetUp]
        public void Setup()
        {
            SetupBase(LoadOptionsEnum.NotLoadDatabase);
        }

        [Test(Description = "Should return 'Invalid Command!' For incorrect command")]
        public void ShouldReturnInvalidCommand()
        {
            var command = $"zrange";
            var result = ConsoleService.CommandExecute(command);

            result.Should().Be(INVALID_COMMAND);

            command = $"zrange 1";
            result = ConsoleService.CommandExecute(command);

            result.Should().Be(INVALID_COMMAND);

            command = $"zrange 1 2";
            result = ConsoleService.CommandExecute(command);

            result.Should().Be(INVALID_COMMAND);
        }

        [Test(Description = "Should return 'Invalid Command!' to incorrect command (Letters in index)")]
        public void ShouldReturnInvalidCommandWhenIndexNotTypeInt()
        {
            var command = $"zrange test a b";
            var result = ConsoleService.CommandExecute(command);

            result.Should().Be(INVALID_COMMAND);
        }

        [Test(Description = "It should return the correct range of values.")]
        public void ShouldReturnCorrectRangeValues()
        {
            ConsoleService.ClearDatabase();
            // 1) "one" 2) "two" 3) "three" 4) "four"
            var command = $"zadd {PREFIX_KEY} 1 \"one\" 2 \"two\" 3 \"three\" 4 \"four\"";
            var result = ConsoleService.CommandExecute(command);

            result.Should().Be("4");

            // 1) "one" 2) "two"
            var commandRange = $"zrange {PREFIX_KEY} 0 1";
            var resultRange = ConsoleService.CommandExecute(commandRange);

            resultRange.Should().Be("1) \"one\"\r\n2) \"two\"\r\n");

            // 1) "one" 2) "two" 3) "three" 4) "four"
            commandRange = $"zrange {PREFIX_KEY} 0 -1";
            resultRange = ConsoleService.CommandExecute(commandRange);

            resultRange.Should().Be("1) \"one\"\r\n2) \"two\"\r\n3) \"three\"\r\n4) \"four\"\r\n");

            // 1) "three" 2) "four"
            commandRange = $"zrange {PREFIX_KEY} -2 -1";
            resultRange = ConsoleService.CommandExecute(commandRange);

            resultRange.Should().Be("1) \"three\"\r\n2) \"four\"\r\n");
        }

        [Test(Description = "Should return the correct range of values, even if the STOP is greater than the number of members.")]
        public void ShouldReturnCorrectRangeValuesWithStopParameterGreaterThanLastIndex()
        {
            ConsoleService.ClearDatabase();
            // 1) "one" 2) "two" 3) "three" 4) "four"
            var command = $"zadd {PREFIX_KEY} 1 \"one\" 2 \"two\" 3 \"three\" 4 \"four\"";
            var result = ConsoleService.CommandExecute(command);

            result.Should().Be("4");

            // 1) "one" 2) "two" 3) "three" 4) "four"
            var commandRange = $"zrange {PREFIX_KEY} 0 8";
            var resultRange = ConsoleService.CommandExecute(commandRange);

            resultRange.Should().Be("1) \"one\"\r\n2) \"two\"\r\n3) \"three\"\r\n4) \"four\"\r\n");
        }

        [Test(Description = "Should return an empty result, when Start> Stop or Start more than Count")]
        public void ShouldReturnEmptyWhenStartGreaterThanStopOrCount()
        {
            ConsoleService.ClearDatabase();
            // 1) "one" 2) "two" 3) "three" 4) "four"
            var command = $"zadd {PREFIX_KEY} 1 \"one\" 2 \"two\" 3 \"three\" 4 \"four\"";
            var result = ConsoleService.CommandExecute(command);

            result.Should().Be("4");

            // 1) "one" 2) "two" 3) "three" 4) "four"
            var commandRange = $"zrange {PREFIX_KEY} 8 0";
            var resultRange = ConsoleService.CommandExecute(commandRange);

            resultRange.Should().Be("");

            // 1) "one" 2) "two" 3) "three" 4) "four"
            commandRange = $"zrange {PREFIX_KEY} 8 9";
            resultRange = ConsoleService.CommandExecute(commandRange);

            resultRange.Should().Be("");
        }

    }
}
