using SmallRedis.Core.Services;
using System;

namespace SmallRedis.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var consoleService = new ConsoleService();

            while (true)
            {
                Console.Write("smallredis> ");
                var command = Console.ReadLine();

                if (command.Trim().ToLower() == "exit")
                    break;

                var commandReturn = consoleService.CommandExecute(command);

                Console.WriteLine(commandReturn);
            }
        }
    }
}
