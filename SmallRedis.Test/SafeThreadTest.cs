using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmallRedis.Test
{
    public class SafeThreadTest : BaseTest
    {
        [SetUp]
        public void Setup()
        {
            SetupBase(LoadOptionsEnum.NotLoadDatabase);
        }

        [Test]
        public void ShouldEnsureSafeThread()
        {
            ConsoleService.ClearDatabase();
            var workers = CreateThreadsWithCommandSet(500);

            Task.WaitAll(workers);

            var command = $"get {PREFIX_KEY}";
            var result = ConsoleService.CommandExecute(command);
        }

        [Test]
        public void ShouldEnsureSafeThreadWithCommandIncr()
        {
            ConsoleService.ClearDatabase();
            var workers = CreateThreadsWithCommandIncr(500);

            Task.WaitAll(workers);

            var command = $"get {PREFIX_KEY}_incr";
            var result = ConsoleService.CommandExecute(command);
        }


        private Task[] CreateThreadsWithCommandSet(int maxThreadCount)
        {
            var workers = new Task[maxThreadCount];

            for (int i = 0; i < maxThreadCount; i++)
            {
                var command = $"set {PREFIX_KEY} {i}";
                var name = "Thread " + (i + 1);
                workers[i] = Task.Factory.StartNew(() => ConsoleService.CommandExecute(command));//new Thread(() => ConsoleService.CommandExecute(command)) { Name = name };
            }

            return workers;
        }

        private Task[] CreateThreadsWithCommandIncr(int maxThreadCount)
        {
            var workers = new Task[maxThreadCount];

            for (int i = 0; i < maxThreadCount; i++)
            {
                var command = $"incr {PREFIX_KEY}_incr";
                var name = "Thread " + (i + 1);

                workers[i] = Task.Factory.StartNew(() => ConsoleService.CommandExecute(command));//new Thread(() => ConsoleService.CommandExecute(command)) { Name = name };
            }

            return workers;
        }
    }
}
