using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmallRedis.Test
{
    public class IncrTest : BaseTest
    {
        [SetUp]
        public void Setup()
        {
            SetupBase(LoadOptionsEnum.LoadDatabase);
        }

        [Test(Description = "Should increment the value correctly.")]
        public void ShouldIncrTheValue()
        {
            var commandGet = $"get {PREFIX_KEY}1";
            var resultGet = ConsoleService.CommandExecute(commandGet);

            resultGet.Should().Be("1");


            var commandIncr = $"incr {PREFIX_KEY}1";
            var resultIncr = ConsoleService.CommandExecute(commandIncr);

            resultIncr.Should().Be("2");
        }

        [Test(Description = "Should return 'Invalid Command!' For incorrect command")]
        public void ShouldReturnInvalidCommand()
        {
            var command = $"incr";
            var result = ConsoleService.CommandExecute(command);

            result.Should().Be(INVALID_COMMAND);
        }

        [Test(Description = "Should create the non-existent key and set the value correctly")]
        public void ShouldIncrTheValueOfNewKey()
        {
            var commandIncr = $"incr {PREFIX_KEY}_new";
            var resultIncr = ConsoleService.CommandExecute(commandIncr);

            resultIncr.Should().Be("1");

            var commandGet = $"get {PREFIX_KEY}_new";
            var resultGet = ConsoleService.CommandExecute(commandGet);

            resultGet.Should().Be("1");
        }

        [Test(Description = "It should return an error if the value is not an integer")]
        public void ShouldReturnExceptionOfType()
        {
            var commandSet = $"set {PREFIX_KEY}_letter A";
            var resultSet = ConsoleService.CommandExecute(commandSet);

            resultSet.Should().Be("OK");

            var commandGet = $"get {PREFIX_KEY}_letter";
            var resultGet = ConsoleService.CommandExecute(commandGet);

            resultGet.Should().Be("A");

            var commandIncr = $"incr {PREFIX_KEY}_letter";
            var resultIncr = ConsoleService.CommandExecute(commandIncr);

            resultIncr.Should().Be(VALUE_OF_RECORD_NOT_INTEGER);

        }
    }
}
