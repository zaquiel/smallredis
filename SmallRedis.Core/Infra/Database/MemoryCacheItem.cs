using System;
using System.Collections.Generic;
using System.Text;

namespace SmallRedis.Core.Infra.Database
{
    public class MemoryCacheItem
    {
        public MemoryCacheItem(object data, DateTime expirationDate)
        {
            Data = data;
            ExpirationDate = expirationDate;
        }
        public object Data { get; }
        public DateTime ExpirationDate { get; }
    }
}
