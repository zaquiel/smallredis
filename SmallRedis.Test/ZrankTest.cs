using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmallRedis.Test
{
    public class ZrankTest : BaseTest
    {
        [SetUp]
        public void Setup()
        {
            SetupBase(LoadOptionsEnum.NotLoadDatabase);
        }

        [Test(Description = "Should return 'Invalid Command!' For incorrect command")]
        public void ShouldReturnInvalidCommand()
        {
            var command = $"zrank";
            var result = ConsoleService.CommandExecute(command);

            result.Should().Be(INVALID_COMMAND);

            command = $"zrank 1";
            result = ConsoleService.CommandExecute(command);

            result.Should().Be(INVALID_COMMAND);
        }

        [Test(Description = "Should return nil, when the key does not exist")]
        public void ShouldReturnNilWhenKeyNotFound()
        {
            var command = $"zrank test test";
            var result = ConsoleService.CommandExecute(command);

            result.Should().Be("nil");
        }

        [Test(Description = "Should return nil, when member does not exist")]
        public void ShouldReturnNilWhenMemberNotFound()
        {
            ConsoleService.ClearDatabase();
            var commandAdd = $"zadd {PREFIX_KEY} 1 \"one\"";
            var resultAdd = ConsoleService.CommandExecute(commandAdd);

            resultAdd.Should().Be("1");

            var command = $"zrank {PREFIX_KEY} test";
            var result = ConsoleService.CommandExecute(command);

            result.Should().Be("nil");
        }

        [Test(Description = "Should return the correct index")]
        public void ShouldReturnCorrectIndex()
        {
            ConsoleService.ClearDatabase();
            var commandAdd = $"zadd {PREFIX_KEY} 1 \"one\"";
            var resultAdd = ConsoleService.CommandExecute(commandAdd);

            resultAdd.Should().Be("1");

            var command = $"zrank {PREFIX_KEY} \"one\"";
            var result = ConsoleService.CommandExecute(command);

            result.Should().Be("0");
        }
    }
}
