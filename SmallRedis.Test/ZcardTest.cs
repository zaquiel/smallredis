using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmallRedis.Test
{
    public class ZcardTest : BaseTest
    {
        [SetUp]
        public void Setup()
        {
            SetupBase(LoadOptionsEnum.NotLoadDatabase);
        }

        [Test(Description = "Should return 'Invalid Command!' For incorrect command")]
        public void ShouldReturnInvalidCommand()
        {
            var command = $"zcard";
            var result = ConsoleService.CommandExecute(command);

            result.Should().Be(INVALID_COMMAND);
        }

        [Test(Description = "Should return 0, when the key does not exist")]
        public void ShouldReturnZeroWhenKeyNotFound()
        {
            var command = $"zcard blablabla";
            var result = ConsoleService.CommandExecute(command);

            result.Should().Be("0");
        }

        [Test(Description = "Must return total members of a key")]
        public void ShouldReturnCountOfMembersOfKey()
        {
            ConsoleService.ClearDatabase();
            var commandAdd = $"zadd {PREFIX_KEY} 1 \"one\" 2 \"two\"";
            var resultAdd = ConsoleService.CommandExecute(commandAdd);

            resultAdd.Should().Be("2");

            var command = $"zcard {PREFIX_KEY}";
            var result = ConsoleService.CommandExecute(command);

            result.Should().Be("2");
        }
    }
}
