using SmallRedis.Core.Infra.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallRedis.Core.Infra
{
    public class MemoryCacheDB
    {
        private static Hashtable _items;
        private static Hashtable Items
        {
            get
            {
                if (_items == null)
                {
                    _items = new Hashtable(StringComparer.InvariantCultureIgnoreCase);
                }

                return _items;
            }
        }

        private object _sync = new object();

        public int Count => Items.Values.Cast<MemoryCacheItem>()
            .Count(x => x.ExpirationDate > DateTime.Now || x.ExpirationDate == DateTime.MinValue);

        public void Set(string key, object value, int expire = -1)
        {
            lock (_sync)
            {
                var expirationDate = DateTime.MinValue;
                if (expire > 0)
                    expirationDate = DateTime.Now.AddSeconds(expire);
                Items[key] = new MemoryCacheItem(value, expirationDate);
            }
        }

        public object Get(string key)
        {
            object value = null;
            lock (_sync)
            {
                if (ContainsKey(key))
                {
                    var record = ((MemoryCacheItem)Items[key]);

                    if ((record.ExpirationDate == DateTime.MinValue) || (record.ExpirationDate > DateTime.Now))
                    {
                        value = ((MemoryCacheItem)Items[key]).Data;
                    }
                }
            }
            return value;
        }

        public int Del(string[] keys)
        {
            var countDel = 0;
            lock (_sync)
            {
                foreach (var key in keys)
                {
                    if (ContainsKey(key))
                    {
                        Items.Remove(key);
                        countDel++;
                    }
                }
            }

            return countDel;
        }

        public int Incr(string key)
        {
            var returnValue = 0;
            lock (_sync)
            {
                var value = Get(key);
                if (value == null)
                {
                    returnValue = 1;
                    Set(key, returnValue);
                }
                else if (int.TryParse(value.ToString(), out var valueInt))
                {
                    returnValue = valueInt + 1;
                    Set(key, returnValue);
                }
                else
                {
                    returnValue = -1;
                }


            }
            return returnValue;
        }

        public void Clear()
        {
            Items.Clear();
        }

        #region [Methods Z]

        public object Zadd(string key, Dictionary<string, int> values)
        {
            object result = null;

            lock (_sync)
            {
                if (ContainsKey(key))
                {
                    Dictionary<string, int> records = null;

                    try
                    {
                        records = (Dictionary<string, int>)Get(key);
                        var resultCount = 0;

                        foreach (var item in values)
                        {
                            var refer = records.FirstOrDefault(x => x.Key.ToLower().Equals(item.Key.ToLower()));

                            if (refer.Key == null)
                            {
                                records.Add(item.Key, item.Value);
                                resultCount++;
                            }
                            else
                            {
                                records[item.Key] = item.Value;
                            }
                        }
                        Set(key, records);
                        result = resultCount;
                    }
                    catch (Exception)
                    {
                        result = "ERROR_TYPE";
                    }

                }
                else
                {
                    Set(key, values);
                    result = values.Count;
                }

            }

            return result;
        }

        public int Zcard(string key)
        {
            var returnCount = 0;
            lock (_sync)
            {
                if (ContainsKey(key))
                {
                    var records = (Dictionary<string, int>)Get(key);

                    returnCount = records.Count();
                }
            }

            return returnCount;

        }

        public object Zrank(string key, string member)
        {
            var returnCount = -1;
            object result = null;
            lock (_sync)
            {
                if (ContainsKey(key))
                {
                    var records = (Dictionary<string, int>)Get(key);

                    var registrosList = records.ToList();
                    registrosList.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));

                    for (int i = 0; i < registrosList.Count; i++)
                    {
                        if (registrosList[i].Key.ToLower().Equals(member.ToLower()))
                            returnCount = i;
                    }

                    if (returnCount >= 0)
                        result = returnCount;
                }
            }

            return result;
        }

        public string Zrange(string key, int start, int stop)
        {
            var result = string.Empty;
            lock (_sync)
            {
                if (ContainsKey(key))
                {
                    var records = (Dictionary<string, int>)Get(key);

                    var indexStart = (start < 0) ? (records.Count() + start) : start;
                    var indexStop = (stop < 0) ? (records.Count() + stop) : stop;

                    var intervalMembers = records.Skip(indexStart).Take(indexStop - indexStart + 1);

                    if (intervalMembers.Any())
                    {
                        var collection = intervalMembers.OrderBy(x => x.Value).Select((x, i) => $"{i + 1}) {x.Key}{Environment.NewLine}").ToArray();

                        result = collection.Aggregate((x, prox) => x + prox);
                    }
                }
            }

            return result;
        }

        #endregion

        private bool ContainsKey(string key)
        {
            lock (_sync)
            {
                return Items.ContainsKey(key);
            }
        }
    }
}
