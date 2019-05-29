using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmallRedis.Test
{
    public class ZaddTest : BaseTest
    {
        [SetUp]
        public void Setup()
        {
            SetupBase(LoadOptionsEnum.NotLoadDatabase);
        }

        [Test(Description = "Should return 'Invalid Command!' For incorrect command")]
        public void ShouldReturnInvalidCommand()
        {
            var command = $"zadd";
            var result = ConsoleService.CommandExecute(command);

            result.Should().Be(INVALID_COMMAND);

            command = $"zadd 1";
            result = ConsoleService.CommandExecute(command);

            result.Should().Be(INVALID_COMMAND);

            command = $"zadd 1 2";
            result = ConsoleService.CommandExecute(command);

            result.Should().Be(INVALID_COMMAND);
        }

        [Test(Description = "Should add the records and, if a member already exists, update the value.")]
        public void ShouldAddOrUpdateCorrectsValues()
        {
            // 1) "one" 2) "two"
            var command = $"zadd {PREFIX_KEY} 1 \"one\" 2 \"two\"";
            var result = ConsoleService.CommandExecute(command);

            result.Should().Be("2");

            var testResult = Zrange(PREFIX_KEY);

            testResult.Should().Be("1) \"one\"\r\n2) \"two\"\r\n");

            // 1) "one" 2) "two" 3) "four"
            command = $"zadd {PREFIX_KEY} 3 \"two\" 4 \"four\"";
            result = ConsoleService.CommandExecute(command);

            result.Should().Be("1");

            testResult = Zrange(PREFIX_KEY);

            testResult.Should().Be("1) \"one\"\r\n2) \"two\"\r\n3) \"four\"\r\n");

            // 1) "one" 2) "two" 3) "four"
            command = $"zadd {PREFIX_KEY} 2 \"two\"";
            result = ConsoleService.CommandExecute(command);

            result.Should().Be("0");

            testResult = Zrange(PREFIX_KEY);

            testResult.Should().Be("1) \"one\"\r\n2) \"two\"\r\n3) \"four\"\r\n");
        }

        private string Zrange(string key)
        {
            return ConsoleService.CommandExecute($"zrange {key} 0 -1");
        }

        [Test(Description = "It should return a type error because the collection is not ordered.")]
        public void ShouldCreateKeyAndAddRecords()
        {
            var commandSet = $"set {PREFIX_KEY} 1";
            var resultSet = ConsoleService.CommandExecute(commandSet);

            resultSet.Should().Be("OK");

            var command = $"zadd {PREFIX_KEY} 3 \"two\" 4 \"four\"";
            var result = ConsoleService.CommandExecute(command);

            result.Should().Be(TYPE_IS_NOT_SORTED_SET);
        }

    }
}
