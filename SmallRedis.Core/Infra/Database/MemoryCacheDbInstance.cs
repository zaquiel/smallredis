using System;
using System.Collections.Generic;
using System.Text;

namespace SmallRedis.Core.Infra.Database
{
    public class MemoryCacheDbInstance
    {
        private static MemoryCacheDB _memoryCacheDB;
        public static MemoryCacheDB MemoryCacheDB
        {
            get
            {
                if (_memoryCacheDB == null)
                {
                    _memoryCacheDB = new MemoryCacheDB();
                }

                return _memoryCacheDB;
            }
        }
    }
}
