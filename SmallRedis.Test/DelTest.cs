using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmallRedis.Test
{
    public class DelTest : BaseTest
    {
        [SetUp]
        public void Setup()
        {
            SetupBase(LoadOptionsEnum.NotLoadDatabase);
        }

        [Test(Description = "Should return 'Invalid Command!' For incorrect command")]
        public void ShouldReturnInvalidCommand()
        {
            var command = $"del";
            var result = ConsoleService.CommandExecute(command);

            result.Should().Be(INVALID_COMMAND);
        }

        [Test(Description = "You must delete the keys.")]
        public void ShouldRemoveKeys()
        {
            // Mounts the values
            for (int i = 0; i < 5; i++)
            {
                var commandSet = $"set {PREFIX_KEY}{i} {i}";
                var resultSet = ConsoleService.CommandExecute(commandSet);

                resultSet.Should().Be("OK");
            }

            // Shows that the values exist
            for (int i = 0; i < 5; i++)
            {
                var commandGet = $"get {PREFIX_KEY}{i}";
                var resultGet = ConsoleService.CommandExecute(commandGet);

                resultGet.Should().Be(i.ToString());
            }

            // Delete the values
            var commandDel = "del";
            var builder = new StringBuilder();
            builder.Append(commandDel);
            for (int i = 0; i < 5; i++)
            {
                builder.Append($" {PREFIX_KEY}{i}");
            }
            commandDel = builder.ToString();
            var resultDel = ConsoleService.CommandExecute(commandDel);

            resultDel.Should().Be("5");

            // Shows that the values no longer exist
            for (int i = 0; i < 5; i++)
            {
                var commandGet = $"get {PREFIX_KEY}{i}";
                var resultGet = ConsoleService.CommandExecute(commandGet);

                resultGet.Should().Be("nil");
            }

        }

        [Test(Description = "Must ignore if key does not exist.")]
        public void ShouldIgnoreKeyNotFound()
        {
            ConsoleService.ClearDatabase();

            for (int i = 0; i < 5; i++)
            {
                var commandSet = $"set {PREFIX_KEY}{i} {i}";
                var resultSet = ConsoleService.CommandExecute(commandSet);

                resultSet.Should().Be("OK");
            }

            // Shows that the value exists
            var commandGet = $"get {PREFIX_KEY}1";
            var resultGet = ConsoleService.CommandExecute(commandGet);

            resultGet.Should().Be("1");

            var commandDel = $"del {PREFIX_KEY}1 {PREFIX_KEY}2 {PREFIX_KEY}3 {PREFIX_KEY}10";
            var resultDel = ConsoleService.CommandExecute(commandDel);

            resultDel.Should().Be("3");

            // Shows that the value no longer exists
            commandGet = $"get {PREFIX_KEY}1";
            resultGet = ConsoleService.CommandExecute(commandGet);

            resultGet.Should().Be("nil");
        }
    }
}
