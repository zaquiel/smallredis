using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmallRedis.Test
{
    public class GeneralTest : BaseTest
    {
        [SetUp]
        public void Setup()
        {
            SetupBase(LoadOptionsEnum.NotLoadDatabase);
        }

        [Test(Description = "Command not found.")]
        public void ShouldCommandNotFound()
        {
            var command = $"blablabla";
            var result = ConsoleService.CommandExecute(command);

            result.Should().Be(NOT_FOUND_MSG_ERRO);
        }
    }
}
