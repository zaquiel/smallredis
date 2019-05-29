using SmallRedis.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmallRedis.Core.Services
{
    public class ConsoleService
    {
        private RedisinhoBLL _redisinhoBLL;

        public ConsoleService()
        {
            _redisinhoBLL = new RedisinhoBLL();
        }

        public string CommandExecute(string command)
        {
            object result = null;

            if (!string.IsNullOrEmpty(command))
            {
                var (response, commandFormatted, error) = _redisinhoBLL.FormatCommand(command);

                result = response ? _redisinhoBLL.ExecuteCommand(commandFormatted) : error;
            }

            return result?.ToString();
        }

        public void ClearDatabase()
        {
            _redisinhoBLL.ClearDatabase();
        }
    }
}
