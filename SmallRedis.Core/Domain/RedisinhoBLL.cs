using SmallRedis.Core.Entities;
using SmallRedis.Core.Enum;
using SmallRedis.Core.Infra;
using SmallRedis.Core.Infra.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallRedis.Core.Domain
{
    public class RedisinhoBLL
    {
        private MemoryCacheDB _memoryCacheDB;
        private const string INVALID_MSG_ERRO = "Invalid Command!";
        private const string NOT_FOUND_MSG_ERRO = "Command not found!";
        private const string VALUE_OF_RECORD_NOT_INTEGER = "The value of the record is not an integer!";
        private const string TYPE_IS_NOT_SORTED_SET = "Does not hold a sorted set!";

        public RedisinhoBLL()
        {
            _memoryCacheDB = MemoryCacheDbInstance.MemoryCacheDB;
        }

        public object ExecuteCommand(Command command)
        {
            object response = null;

            if (command.Operation == OperationEnum.GET)
            {
                response = _memoryCacheDB.Get(command.Key.First());
            }
            else if (command.Operation == OperationEnum.SET)
            {
                _memoryCacheDB.Set(command.Key.First(), command.Value, command.ExSeconds);
                response = "OK";
            }
            else if (command.Operation == OperationEnum.DEL)
            {
                response = _memoryCacheDB.Del(command.Key.ToArray());
            }
            else if (command.Operation == OperationEnum.DBSIZE)
            {
                response = _memoryCacheDB.Count;
            }
            else if (command.Operation == OperationEnum.INCR)
            {
                response = _memoryCacheDB.Incr(command.Key.First());
                response = (response.ToString() == "-1") ? VALUE_OF_RECORD_NOT_INTEGER : response;
            }
            else if (command.Operation == OperationEnum.ZADD)
            {
                response = _memoryCacheDB.Zadd(command.Key.First(), command.MemberScore);

                if (response.ToString().Equals("ERROR_TYPE"))
                {
                    response = TYPE_IS_NOT_SORTED_SET;
                }
            }
            else if (command.Operation == OperationEnum.ZCARD)
            {
                response = _memoryCacheDB.Zcard(command.Key.First());
            }
            else if (command.Operation == OperationEnum.ZRANK)
            {
                response = _memoryCacheDB.Zrank(command.Key.First(), command.Member);
            }
            else if (command.Operation == OperationEnum.ZRANGE)
            {
                response = _memoryCacheDB.Zrange(command.Key.First(), command.Start, command.Stop);
            }

            return response ?? "nil";
        }

        public (bool, Command, string) FormatCommand(string command)
        {
            var response = (false, default(Command), INVALID_MSG_ERRO);

            if (string.IsNullOrEmpty(command))
            {
                return response;
            }

            var options = command.Trim().Split(" ");
            var commandObj = new Command();

            try
            {
                commandObj.Operation = (OperationEnum)System.Enum.Parse(typeof(OperationEnum), options[0].ToUpper());

                switch (commandObj.Operation)
                {
                    case OperationEnum.GET:
                    case OperationEnum.INCR:
                    case OperationEnum.ZCARD:
                        {
                            if (options.Length == 2)
                            {
                                commandObj.Key.Add(options[1]);
                                response = (true, commandObj, string.Empty);
                            }
                            break;
                        }
                    case OperationEnum.SET:
                        {
                            if ((options.Length >= 3) && (options.Length < 5))
                            {
                                commandObj.Key.Add(options[1]);
                                commandObj.Value = options[2];
                                if (options.Length == 4)
                                    commandObj.ExSeconds = int.Parse(options[3]);
                                response = (true, commandObj, string.Empty);
                            }
                            break;
                        }
                    case OperationEnum.DEL:
                        {
                            if (options.Length >= 2)
                            {
                                commandObj.Key = options.Skip(1).ToList();
                                response = (true, commandObj, string.Empty);
                            }
                            break;
                        }
                    case OperationEnum.DBSIZE:
                        {
                            if (options.Length == 1)
                            {
                                response = (true, commandObj, string.Empty);
                            }
                            break;
                        }
                    case OperationEnum.ZADD:
                        {
                            if (options.Length >= 4)
                            {
                                commandObj.Key.Add(options[1]);
                                var listOpt = options.Skip(2).ToList();
                                for (int i = 0; i < listOpt.Count(); i++)
                                {
                                    commandObj.MemberScore.Add(listOpt[i + 1], int.Parse(listOpt[i]));
                                    i++;
                                }
                                response = (true, commandObj, string.Empty);
                            }
                            break;
                        }
                    case OperationEnum.ZRANK:
                        {
                            if (options.Length == 3)
                            {
                                commandObj.Key.Add(options[1]);
                                commandObj.Member = options[2];
                                response = (true, commandObj, string.Empty);
                            }
                            break;
                        }
                    case OperationEnum.ZRANGE:
                        {
                            if (options.Length == 4)
                            {
                                commandObj.Key.Add(options[1]);
                                commandObj.Start = int.Parse(options[2]);
                                commandObj.Stop = int.Parse(options[3]);
                                response = (true, commandObj, string.Empty);
                            }
                            break;
                        }
                    default:
                        throw new ArgumentException();
                }
            }
            catch (ArgumentException)
            {
                response = (false, null, NOT_FOUND_MSG_ERRO);
            }
            catch (Exception)
            {
                response = (false, null, INVALID_MSG_ERRO);
            }


            return response;
        }

        public void ClearDatabase()
        {
            _memoryCacheDB.Clear();
        }
    }
}
