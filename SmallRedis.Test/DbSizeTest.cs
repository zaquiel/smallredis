using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SmallRedis.Test
{
    public class DbSizeTest : BaseTest
    {
        [SetUp]
        public void Setup()
        {
            SetupBase(LoadOptionsEnum.LoadOnlyKeysOk);
        }

        [Test(Description = "Should return 'Invalid Command!' For incorrect command")]
        public void ShouldReturnInvalidCommand()
        {
            var command = $"dbsize 1 2 3";
            var result = ConsoleService.CommandExecute(command);

            result.Should().Be(INVALID_COMMAND);
        }

        [Test(Description = "It should return the size of the bank disregarding expired records.")]
        public void ShouldReturnLengthDb()
        {
            // Count
            var command = $"dbsize";
            var result = ConsoleService.CommandExecute(command);

            result.Should().Be(COUNT_DB_OK.ToString());

            // Add Expired
            var commandSet = $"set exp 1 1";
            var resultSet = ConsoleService.CommandExecute(commandSet);

            resultSet.Should().Be("OK");

            //Expect to expire
            Thread.Sleep(1000);

            // Count
            command = $"dbsize";
            result = ConsoleService.CommandExecute(command);

            result.Should().Be(COUNT_DB_OK.ToString());
        }
    }    
}
