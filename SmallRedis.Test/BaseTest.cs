using SmallRedis.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmallRedis.Test
{
    public class BaseTest
    {
        protected ConsoleService ConsoleService { get; private set; }
        protected const string PREFIX_KEY_EX = "keyEx";
        protected const string PREFIX_VALUE_EX = "Ex";
        protected const string PREFIX_KEY = "key";
        protected const string INVALID_COMMAND = "Invalid Command!";
        protected const string VALUE_OF_RECORD_NOT_INTEGER = "The value of the record is not an integer!";
        protected const string TYPE_IS_NOT_SORTED_SET = "Does not hold a sorted set!";
        protected const string NOT_FOUND_MSG_ERRO = "Command not found!";
        protected const int COUNT_DB_OK = 50;
        protected const int COUNT_DB_EXPIRED = 50;
        protected const int COUNT_DB = 100;

        protected enum LoadOptionsEnum
        {
            NotLoadDatabase = 0,
            LoadDatabase = 1,
            LoadOnlyKeysOk = 2,
            LoadOnlyKeysExpired = 3
        }

        protected void SetupBase(LoadOptionsEnum loadOptionsEnum)
        {
            ConsoleService = new ConsoleService();

            switch (loadOptionsEnum)
            {
                case LoadOptionsEnum.LoadDatabase:
                    //Ok
                    for (int i = 0; i < 25; i++)
                    {
                        ConsoleService.CommandExecute($"set key{i} {i}");
                        ConsoleService.CommandExecute($"set keyEx{i} Ex{i}");
                    }

                    //Expiradas
                    for (int i = 25; i < 50; i++)
                    {
                        ConsoleService.CommandExecute($"set key{i} {i} 1");
                        ConsoleService.CommandExecute($"set keyEx{i} Ex{i} 1");
                    }

                    break;
                case LoadOptionsEnum.LoadOnlyKeysOk:
                    //Ok
                    for (int i = 0; i < 25; i++)
                    {
                        ConsoleService.CommandExecute($"set key{i} {i}");
                        ConsoleService.CommandExecute($"set keyEx{i} Ex{i}");
                    }

                    break;
                case LoadOptionsEnum.LoadOnlyKeysExpired:
                    //Expiradas
                    for (int i = 25; i < 50; i++)
                    {
                        ConsoleService.CommandExecute($"set key{i} {i} 1");
                        ConsoleService.CommandExecute($"set keyEx{i} Ex{i} 1");
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
