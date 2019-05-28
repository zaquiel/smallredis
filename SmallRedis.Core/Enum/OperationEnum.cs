using System;
using System.Collections.Generic;
using System.Text;

namespace SmallRedis.Core.Enum
{
    public enum OperationEnum
    {
        GET = 0,
        SET = 1,
        DEL = 2,
        DBSIZE = 3,
        INCR = 4,
        ZADD = 5,
        ZCARD = 6,
        ZRANK = 7,
        ZRANGE = 8
    }
}
