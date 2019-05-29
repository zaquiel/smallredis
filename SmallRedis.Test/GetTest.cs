using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SmallRedis.Test
{
    public class GetTest: BaseTest
    {
        [SetUp]
        public void Setup()
        {
            SetupBase(LoadOptionsEnum.LoadDatabase);
        }

        [Test(Description = "It should return the correct value of the specified key.")]
        public void ShouldReturnCorrectValueByKey()
        {
            for (int i = 0; i < 5; i++)
            {
                var command = $"get {PREFIX_KEY}{i}";
                var result = ConsoleService.CommandExecute(command);

                result.Should().Be(i.ToString());
            }
        }

        [Test(Description = "Should return nil, for non-existent keys.")]
        public void ShouldReturnNilWhenKeyNotFound()
        {
            var command = $"get {PREFIX_KEY}0";
            var result = ConsoleService.CommandExecute(command);

            result.Should().Be("0");

            command = $"get {PREFIX_KEY}AAAAA";
            result = ConsoleService.CommandExecute(command);

            result.Should().Be("nil");
        }

        [Test(Description = "Should return nil, for expired keys.")]
        public void ShouldReturnNilWhenKeyExpired()
        {
            var command = $"get {PREFIX_KEY}0";
            var result = ConsoleService.CommandExecute(command);

            result.Should().Be("0");

            Thread.Sleep(1000);
            command = $"get {PREFIX_KEY}30";
            result = ConsoleService.CommandExecute(command);

            result.Should().Be("nil");
        }

        [Test(Description = "Should return 'Invalid Command', for invalid commands.")]
        public void ShouldReturnInvalidCommandWhenCommandIncorrect()
        {
            var command = $"get";
            var result = ConsoleService.CommandExecute(command);

            result.Should().Be(INVALID_COMMAND);

            command = $"get test 1 1";
            result = ConsoleService.CommandExecute(command);

            result.Should().Be(INVALID_COMMAND);
        }
    }
}
