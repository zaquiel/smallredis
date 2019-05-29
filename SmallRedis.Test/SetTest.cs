using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SmallRedis.Test
{
    public class SetTest : BaseTest
    {
        [SetUp]
        public void Setup()
        {
            SetupBase(LoadOptionsEnum.LoadDatabase);
        }

        [Test(Description = "Should update the key value.")]
        public void ShouldUpdateValueKey()
        {
            var commandGet = $"get {PREFIX_KEY}0";
            var result = ConsoleService.CommandExecute(commandGet);

            result.Should().Be("0");

            var commandSet = $"set {PREFIX_KEY}0 1";
            var result2 = ConsoleService.CommandExecute(commandSet);

            result2.Should().Be("OK");

            commandGet = $"get {PREFIX_KEY}0";
            result = ConsoleService.CommandExecute(commandGet);

            result.Should().Be("1");
        }

        [Test(Description = "Should return 'Invalid Command', for invalid commands")]
        public void ShouldReturnInvalidCommandWhenCommandIncorrect()
        {
            var command = $"set";
            var result = ConsoleService.CommandExecute(command);

            result.Should().Be(INVALID_COMMAND);

            command = $"set test 1 1 1 1 1";
            result = ConsoleService.CommandExecute(command);

            result.Should().Be(INVALID_COMMAND);
        }

        [Test(Description = "Should set the key expiration date correctly")]
        public void ShouldSetExpirationDateForKey()
        {
            var command = $"set {PREFIX_KEY}_Exp 1 1";
            var result = ConsoleService.CommandExecute(command);

            result.Should().Be("OK");

            Thread.Sleep(1000);
            command = $"get {PREFIX_KEY}_EXP";
            result = ConsoleService.CommandExecute(command);

            result.Should().Be("nil");
        }

        [Test(Description = "Should ignore values less than 1 for the expiration parameter")]
        public void ShouldIgnoreExpirationValueForKeyLessThanOne()
        {
            var command = $"set {PREFIX_KEY}_Exp 1 0";
            var result = ConsoleService.CommandExecute(command);

            result.Should().Be("OK");

            Thread.Sleep(1000);
            command = $"get {PREFIX_KEY}_EXP";
            result = ConsoleService.CommandExecute(command);

            result.Should().Be("1");
        }
    }
}
