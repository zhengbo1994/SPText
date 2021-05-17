using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SPText.Common.Redis
{
    /// <summary>
    /// Redis的客户端封装
    /// </summary>
    public class RedisRepository : ICache
    {
        #region 连接对象

        private static ConnectionMultiplexer _multiplexer;
        private static ConfigurationOptions _options;
        private static readonly object Locker = new object();

        public RedisRepository(string url)
        {
            _options = ConfigurationOptions.Parse(url);
            _options.KeepAlive = 120;
            _options.CommandMap = CommandMap.Create(new HashSet<string>
                            {
                                "INFO",
                                "CONFIG",
                                "PING",
                                "ECHO",
                                "CLIENT"
                            }, false);
        }

        private static ConnectionMultiplexer Conn
        {
            get
            {
                if (_multiplexer != null && _multiplexer.IsConnected)
                    return _multiplexer;

                lock (Locker)
                {
                    if (_multiplexer != null && _multiplexer.IsConnected)
                        return _multiplexer;

                    _multiplexer = ConnectionMultiplexer.Connect(_options);
                }

                return _multiplexer;
            }
        }

        private static IDatabase Db => Conn.GetDatabase();

        #endregion

        #region Key处理

        public bool Remove(string key)
        {
            return Db.KeyDelete(key);
        }

        public Task<bool> RemoveAsync(string key)
        {
            return Db.KeyDeleteAsync(key);
        }

        public long Remove(string[] keys)
        {
            var tasks = new Task<bool>[keys.Length];
            var removed = 0L;

            for (var i = 0; i < keys.Length; i++)
                tasks[i] = RemoveAsync(keys[i]);

            for (var i = 0; i < keys.Length; i++)
                if (Db.Wait(tasks[i]))
                    removed++;

            return removed;
        }

        public async Task<long> RemoveAsync(string[] keys)
        {
            var removed = 0L;

            foreach (var t in keys)
            {
                if (await Db.KeyDeleteAsync(t))
                    removed++;
            }
            return removed;
        }

        public bool Contains(string key)
        {
            return Db.KeyExists(key);
        }

        public Task<bool> ContainsAsync(string key)
        {
            return Db.KeyExistsAsync(key);
        }

        public bool KeyExpire(string key, DateTime? expiry)
        {
            return Db.KeyExpire(key, expiry);
        }

        public Task<bool> KeyExpireAsync(string key, DateTime? expiry)
        {
            return Db.KeyExpireAsync(key, expiry);
        }

        public TimeSpan? KeyTimeToLive(string key)
        {
            return Db.KeyTimeToLive(key);
        }

        public Task<TimeSpan?> KeyTimeToLiveAsync(string key)
        {
            return Db.KeyTimeToLiveAsync(key);
        }

        public RedisItemType KeyType(string key)
        {
            return Db.KeyType(key).ToType();
        }

        public async Task<RedisItemType> KeyTypeAsync(string key)
        {
            return (await Db.KeyTypeAsync(key)).ToType();
        }

        public IEnumerable<RedisKey> FindKeys(string pattern)
        {
            return Conn.GetServer(Conn.GetEndPoints()[0]).Keys(Db.Database, pattern);
        }

        #endregion

        #region 字符串

        #region Set

        public bool Set(string key, string value, int seconds = 0)
        {
            return Db.StringSet(key, value, seconds.ToRedisTimeSpan());
        }

        public bool Set<T>(string key, T t, int seconds = 0) where T : class, new()
        {
            return Set(key, t.ToJson(), seconds);
        }

        public Task<bool> SetAsync(string key, string value, int seconds = 0)
        {
            return Db.StringSetAsync(key, value, seconds.ToRedisTimeSpan());
        }

        public Task<bool> SetAsync<T>(string key, T t, int seconds = 0) where T : class, new()
        {
            return SetAsync(key, t.ToJson(), seconds);
        }

        public bool Set(IDictionary<string, string> kvs)
        {
            return Db.StringSet(kvs.ToKeyValuePairArray());
        }

        public Task<bool> SetAsync(IDictionary<string, string> kvs)
        {
            return Db.StringSetAsync(kvs.ToKeyValuePairArray());
        }

        /// <summary>
        /// 锁
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="seconds">秒</param>
        /// <param name="nolockAction">无锁操作</param>
        /// <param name="lockAction">已锁操作</param>
        public void Lock(string key, Action nolockAction, int seconds = 3, Action lockAction = null)
        {
            var lockValue = Guid.NewGuid().ToString("N");//避免A线程锁超时的误释放B线程
            var isLock = Db.StringSet(key, lockValue, seconds.ToRedisTimeSpan(), When.NotExists);
            try
            {
                if (isLock)
                {
                    nolockAction?.Invoke();
                }
                else
                {
                    lockAction?.Invoke();
                }
            }
            finally
            {
                RemoveLock(key, lockValue);
            }
        }

        /// <summary>
        /// 删除锁
        /// </summary>
        /// <param name="key"></param>
        /// <param name="lockValue"></param>
        private void RemoveLock(string key, string lockValue)
        {
            Db.ScriptEvaluate(LuaScript.Prepare(@"
            if redis.call(""get"",@key) == @lockValue then
            return redis.call(""del"", @key)
            else
            return 0
            end"), new { key, lockValue });
        }

        #endregion

        #region Add

        public bool Add(string key, string value, int seconds = 0)
        {
            return Db.StringSet(key, value, seconds.ToRedisTimeSpan(), When.NotExists);
        }

        public bool Add<T>(string key, T t, int seconds = 0) where T : class, new()
        {
            return Db.StringSet(key, t.ToJson(), seconds.ToRedisTimeSpan(), When.NotExists);
        }

        public Task<bool> AddAsync(string key, string value, int seconds = 0)
        {
            return Db.StringSetAsync(key, value, seconds.ToRedisTimeSpan(), When.NotExists);
        }

        public bool Add(IDictionary<string, string> kvs)
        {
            return Db.StringSet(kvs.ToKeyValuePairArray(), When.NotExists);
        }

        public Task<bool> AddAsync(IDictionary<string, string> kvs)
        {
            return Db.StringSetAsync(kvs.ToKeyValuePairArray(), When.NotExists);
        }

        #endregion

        #region Get

        public string Get(string key)
        {
            return Db.StringGet(key);
        }

        public T Get<T>(string key) where T : class
        {
            return Get(key).FromJson<T>();
        }

        public string[] Get(string[] keys)
        {
            return Db.StringGet(keys.ToRedisKeyArray()).ToStringArray();
        }

        public async Task<string> GetAsync(string key)
        {
            return await Db.StringGetAsync(key);
        }

        public async Task<string[]> GetAsync(string[] keys)
        {
            return (await Db.StringGetAsync(keys.ToRedisKeyArray())).ToStringArray();
        }

        #endregion

        #region Increment

        public long Increment(string key, long value = 1)
        {
            return Db.StringIncrement(key, value);
        }

        public Task<long> IncrementAsync(string key, long value = 1)
        {
            return Db.StringIncrementAsync(key, value);
        }

        #endregion

        #region Decrement

        public long Decrement(string key, long value = 1)
        {
            return Db.StringDecrement(key, value);
        }

        public Task<long> DecrementAsync(string key, long value = 1)
        {
            return Db.StringDecrementAsync(key, value);
        }

        #endregion

        #region GetSet

        public string GetSet(string key, string value)
        {
            return Db.StringGetSet(key, value);
        }

        public async Task<string> GetSetAsync(string key, string value)
        {
            return await Db.StringGetSetAsync(key, value);
        }

        #endregion

        #region GetOrAdd

        public string GetOrAdd(string key, Func<string> aquire, int seconds = 0)
        {
            var data = Get(key);
            if (data.IsNullOrEmpty())
            {
                data = aquire();
                if (!data.IsNullOrEmpty())
                    Add(key, data, seconds);
            }
            return data;
        }

        public T GetOrAdd<T>(string key, Func<T> aquire, int seconds = 0) where T : class, new()
        {
            var data = Get<T>(key);
            if (data.IsNull())
            {
                data = aquire();
                if (data.IsNotNull())
                    Add(key, data, seconds);
            }
            return data;
        }

        public async Task<string> GetOrAddAsync(string key, Func<string> aquire, int seconds = 0)
        {
            var data = await Db.StringGetAsync(key);
            var value = data.HasValue ? (string)data : null;
            if (value.IsNullOrEmpty())
            {
                value = aquire();
                if (!value.IsNullOrEmpty())
                    await Db.StringSetAsync(key, value, seconds.ToRedisTimeSpan(), When.NotExists, CommandFlags.FireAndForget);
            }
            return value;
        }

        #endregion

        #region GetOrSet

        public string GetOrSet(string key, Func<string> aquire, int seconds = 0)
        {
            var data = Get(key);
            if (data.IsNullOrEmpty())
            {
                data = aquire();
                if (!data.IsNullOrEmpty())
                    Set(key, data, seconds);
            }
            return data;
        }

        public T GetOrSet<T>(string key, Func<T> aquire, int seconds = 0) where T : class, new()
        {
            var data = Get<T>(key);
            if (data.IsNull())
            {
                data = aquire();
                if (data.IsNotNull())
                    Set(key, data, seconds);
            }
            return data;
        }

        public async Task<string> GetOrSetAsync(string key, Func<string> aquire, int seconds = 0)
        {
            var data = await Db.StringGetAsync(key);
            var value = data.HasValue ? (string)data : null;
            if (value.IsNullOrEmpty())
            {
                value = aquire();
                if (!value.IsNullOrEmpty())
                    await Db.StringSetAsync(key, value, seconds.ToRedisTimeSpan());
            }
            return value;
        }

        #endregion

        #endregion

        #region 哈希

        #region HashSet

        public bool HashSet(string key, string field, string value)
        {
            return Db.HashSet(key, field, value);
        }

        public Task<bool> HashSetAsync(string key, string field, string value)
        {
            return Db.HashSetAsync(key, field, value);
        }

        public void HashSet<T>(string key, T entity) where T : class, new()
        {
            var hashEntry = entity.GetHashEntry(typeof(T).GetEntityProperties());
            Db.HashSet(key, hashEntry);
        }

        public void HashSet<T>(string key, Expression<Func<T>> expression) where T : class, new()
        {
            //var hashEntry = entity.GetHashEntry(typeof(T).GetEntityProperties());
            //Db.HashSet(key, hashEntry);
        }

        #endregion

        #region HashIncrement

        public long HashIncrement(string key, string field, long value = 1)
        {
            return Db.HashIncrement(key, field, value);
        }

        public Task<long> HashIncrementAsync(string key, string field, long value = 1)
        {
            return Db.HashIncrementAsync(key, field, value);
        }

        #endregion

        #region HashDecrement

        public long HashDecrement(string key, string field, long value = 1)
        {
            return Db.HashDecrement(key, field, value);
        }

        public Task<long> HashDecrementAsync(string key, string field, long value = 1)
        {
            return Db.HashDecrementAsync(key, field, value);
        }

        #endregion

        #region HashExists

        public bool HashExists(string key, string field)
        {
            return Db.HashExists(key, field);
        }

        public Task<bool> HashExistsAsync(string key, string field)
        {
            return Db.HashExistsAsync(key, field);
        }

        #endregion

        #region HashDelete

        public bool HashDelete(string key, string field)
        {
            return Db.HashDelete(key, field);
        }

        public Task<bool> HashDeleteAsync(string key, string field)
        {
            return Db.HashDeleteAsync(key, field);
        }

        public long HashDelete(string key, string[] fields)
        {
            return Db.HashDelete(key, RedisExtensions.ToRedisValueArray(fields));
        }

        public Task<long> HashDeleteAsync(string key, string[] fields)
        {
            return Db.HashDeleteAsync(key, RedisExtensions.ToRedisValueArray(fields));
        }

        #endregion

        #region HashGet

        public string HashGet(string key, string field)
        {
            return Db.HashGet(key, field);
        }

        public Task<RedisValue> HashGetAsync(string key, string field)
        {
            return Db.HashGetAsync(key, field);
        }

        #endregion

        #region HashGetAll

        public KeyValuePair<string, string>[] HashGetAll(string key)
        {
            return Db.HashGetAll(key).ToHashPairs();
        }

        public async Task<KeyValuePair<string, string>[]> HashGetAllAsync(string key)
        {
            return (await Db.HashGetAllAsync(key)).ToHashPairs();
        }

        #endregion

        #region HashKeys

        public string[] HashKeys(string key)
        {
            return Db.HashKeys(key).ToStringArray();
        }
        public async Task<string[]> HashKeysAsync(string key)
        {
            return (await Db.HashKeysAsync(key)).ToStringArray();
        }

        #endregion

        #region HashValues

        public string[] HashValues(string key)
        {
            return Db.HashValues(key).ToStringArray();
        }

        public async Task<string[]> HashValuesAsync(string key)
        {
            return (await Db.HashValuesAsync(key)).ToStringArray();
        }

        #endregion

        #region HashLength

        public long HashLength(string key)
        {
            return Db.HashLength(key);
        }

        public Task<long> HashLengthAsync(string key)
        {
            return Db.HashLengthAsync(key);
        }

        #endregion

        #region HashScan

        public KeyValuePair<string, string>[] HashScan(string key, string pattern = null, int pageSize = 10, long cursor = 0, int pageOffset = 0)
        {
            return Db.HashScan(key, pattern, pageSize, cursor, pageOffset).ToHashPairs();
        }

        #endregion

        #endregion

        #region 列表

        #region ListLeftPush

        public long ListLeftPush(string key, string value)
        {
            return Db.ListLeftPush(key, value);
        }

        public long ListLeftPush(string key, string[] values)
        {
            return Db.ListLeftPush(key, RedisExtensions.ToRedisValueArray(values));
        }

        public async Task<long> ListLeftPushAsync(string key, string value)
        {
            return await Db.ListLeftPushAsync(key, value);
        }

        public async Task<long> ListLeftPushAsync(string key, string[] values)
        {
            return await Db.ListLeftPushAsync(key, RedisExtensions.ToRedisValueArray(values));
        }

        #endregion

        #region ListRightPush

        public long ListRightPush(string key, string value)
        {
            return Db.ListRightPush(key, value);
        }
        public long ListRightPush(string key, string[] values)
        {
            return Db.ListRightPush(key, RedisExtensions.ToRedisValueArray(values));
        }

        public async Task<long> ListRightPushAsync(string key, string value)
        {
            return await Db.ListRightPushAsync(key, value);
        }

        public async Task<long> ListRightPushAsync(string key, string[] values)
        {
            return await Db.ListRightPushAsync(key, RedisExtensions.ToRedisValueArray(values));
        }

        #endregion

        #region ListLeftPop

        public string ListLeftPop(string key)
        {
            return Db.ListLeftPop(key);
        }

        public async Task<string> ListLeftPopAsync(string key)
        {
            return await Db.ListLeftPopAsync(key);
        }

        #endregion

        #region ListRightPop

        public string ListRightPop(string key)
        {
            return Db.ListRightPop(key);
        }

        public async Task<string> ListRightPopAsync(string key)
        {
            return await Db.ListRightPopAsync(key);
        }

        #endregion

        #region ListLength

        public long ListLength(string key)
        {
            return Db.ListLength(key);
        }

        public Task<long> ListLengthAsync(string key)
        {
            return Db.ListLengthAsync(key);
        }

        #endregion

        #region ListRange

        public string[] ListRange(string key, long start = 0, long stop = -1)
        {
            return Db.ListRange(key, start, stop).ToStringArray();
        }

        public async Task<string[]> ListRangeAsync(string key, long start = 0, long stop = -1)
        {
            return (await Db.ListRangeAsync(key, start, stop)).ToStringArray();
        }

        #endregion

        #region ListRemove

        public long ListRemove(string key, string value, long count = 0)
        {
            return Db.ListRemove(key, value, count);
        }

        public Task<long> ListRemoveAsync(string key, string value, long count = 0)
        {
            return Db.ListRemoveAsync(key, value, count);
        }

        #endregion

        #region ListTrim

        public void ListTrim(string key, long start, long stop)
        {
            Db.ListTrim(key, start, stop);
        }

        public Task ListTrimAsync(string key, long start, long stop)
        {
            return Db.ListTrimAsync(key, start, stop);
        }

        #endregion

        #region ListInsertAfter

        public long ListInsertAfter(string key, string pivot, string value)
        {
            return Db.ListInsertAfter(key, pivot, value);
        }

        public Task<long> ListInsertAfterAsync(string key, string pivot, string value)
        {
            return Db.ListInsertAfterAsync(key, pivot, value);
        }

        #endregion

        #region ListInsertBefore

        public long ListInsertBefore(string key, string pivot, string value)
        {
            return Db.ListInsertBefore(key, pivot, value);
        }

        public Task<long> ListInsertBeforeAsync(string key, string pivot, string value)
        {
            return Db.ListInsertBeforeAsync(key, pivot, value);
        }

        #endregion

        #region ListGetByIndex

        public string ListGetByIndex(string key, long index)
        {
            return Db.ListGetByIndex(key, index);
        }

        public async Task<string> ListGetByIndexAsync(string key, long index)
        {
            return await Db.ListGetByIndexAsync(key, index);
        }

        #endregion

        #region ListSetByIndex

        public void ListSetByIndex(string key, long index, string value)
        {
            Db.ListSetByIndex(key, index, value);
        }

        public Task ListSetByIndexAsync(string key, long index, string value)
        {
            return Db.ListSetByIndexAsync(key, index, value);
        }

        #endregion

        #endregion

        #region 集合

        #region SetAdd

        public bool SetAdd(string key, string value)
        {
            if (value.IsNullOrWhiteSpace())
                return false;

            return Db.SetAdd(key, value);
        }

        public long SetAdd(string key, string[] values)
        {
            if (!values.Any())
                return 0;

            return Db.SetAdd(key, RedisExtensions.ToRedisValueArray(values));
        }

        public Task<bool> SetAddAsync(string key, string value)
        {
            if (value.IsNullOrWhiteSpace())
                return Task.FromResult(false);

            return Db.SetAddAsync(key, value);
        }

        public Task<long> SetAddAsync(string key, string[] values)
        {
            if (!values.Any())
                return Task.FromResult((long)0);

            return Db.SetAddAsync(key, RedisExtensions.ToRedisValueArray(values));
        }

        #endregion

        #region SetCombine

        public string[] SetCombine(RedisSetOperation operation, string first, string second)
        {
            return Db.SetCombine(operation.ToSetOperation(), first, second).ToStringArray();
        }
        public async Task<string[]> SetCombineAsync(RedisSetOperation operation, string first, string second)
        {
            return (await Db.SetCombineAsync(operation.ToSetOperation(), first, second)).ToStringArray();
        }

        public string[] SetCombine(RedisSetOperation operation, string[] keys)
        {
            return Db.SetCombine(operation.ToSetOperation(), keys.ToRedisKeyArray()).ToStringArray();
        }

        public async Task<string[]> SetCombineAsync(RedisSetOperation operation, string[] keys)
        {
            return (await Db.SetCombineAsync(operation.ToSetOperation(), keys.ToRedisKeyArray())).ToStringArray();
        }

        #endregion

        #region SetCombineAndStore

        public long SetCombineAndStore(RedisSetOperation operation, string desctination, string first, string second)
        {
            return Db.SetCombineAndStore(operation.ToSetOperation(), desctination, first, second);
        }

        public Task<long> SetCombineAndStoreAsync(RedisSetOperation operation, string desctination, string first, string second)
        {
            return Db.SetCombineAndStoreAsync(operation.ToSetOperation(), desctination, first, second);
        }

        public long SetCombineAndStore(RedisSetOperation operation, string desctination, string[] keys)
        {
            return Db.SetCombineAndStore(operation.ToSetOperation(), desctination, keys.ToRedisKeyArray());
        }

        public Task<long> SetCombineAndStoreAsync(RedisSetOperation operation, string desctination, string[] keys)
        {
            return Db.SetCombineAndStoreAsync(operation.ToSetOperation(), desctination, keys.ToRedisKeyArray());
        }

        #endregion

        #region SetContains

        public bool SetContains(string key, string value)
        {
            return Db.SetContains(key, value);
        }

        public Task<bool> SetContainsAsync(string key, string value)
        {
            return Db.SetContainsAsync(key, value);
        }

        #endregion

        #region SetLength

        public long SetLength(string key)
        {
            return Db.SetLength(key);
        }

        public Task<long> SetLengthAsync(string key)
        {
            return Db.SetLengthAsync(key);
        }

        #endregion

        #region SetMembers

        public string[] SetMembers(string key)
        {
            return Db.SetMembers(key).ToStringArray();
        }

        public async Task<string[]> SetMembersAsync(string key)
        {
            return (await Db.SetMembersAsync(key)).ToStringArray();
        }

        #endregion

        #region SetMove

        public bool SetMove(string source, string desctination, string value)
        {
            return Db.SetMove(source, desctination, value);
        }

        public Task<bool> SetMoveAsync(string source, string desctination, string value)
        {
            return Db.SetMoveAsync(source, desctination, value);
        }

        #endregion

        #region SetPop

        public string SetPop(string key)
        {
            return Db.SetPop(key);
        }

        public async Task<string> SetPopAsync(string key)
        {
            return await Db.SetPopAsync(key);
        }

        #endregion

        #region SetRandomMember

        public string SetRandomMember(string key)
        {
            return Db.SetRandomMember(key);
        }

        public async Task<string> SetRandomMemberAsync(string key)
        {
            return await Db.SetRandomMemberAsync(key);
        }

        #endregion

        #region SetRandomMembers

        public string[] SetRandomMembers(string key, long count)
        {
            return Db.SetRandomMembers(key, count).ToStringArray();
        }

        public async Task<string[]> SetRandomMembersAsync(string key, long count)
        {
            return (await Db.SetRandomMembersAsync(key, count)).ToStringArray();
        }

        #endregion

        #region SetRemove

        public bool SetRemove(string key, string value)
        {
            return Db.SetRemove(key, value);
        }

        public Task<bool> SetRemoveAsync(string key, string value)
        {
            return Db.SetRemoveAsync(key, value);
        }

        public long SetRemove(string key, string[] values)
        {
            return Db.SetRemove(key, RedisExtensions.ToRedisValueArray(values));
        }

        public Task<long> SetRemoveAsync(string key, string[] values)
        {
            return Db.SetRemoveAsync(key, RedisExtensions.ToRedisValueArray(values));
        }

        #endregion

        #region SetScan

        public string[] SetScan(string key, string pattern = null, int pageSize = 0, long cursor = 0, int pageOffset = 0)
        {
            return Db.SetScan(key, pattern, pageSize, cursor, pageOffset).ToArray().ToStringArray();
        }

        #endregion

        #endregion

        #region 有序集合

        #region SortedSetAdd

        public bool SortedSetAdd(string key, string member, double score)
        {
            return Db.SortedSetAdd(key, member, score);
        }

        public Task<bool> SortedSetAddAsync(string key, string member, double score)
        {
            return Db.SortedSetAddAsync(key, member, score);
        }

        public long SortedSetAdd(string key, IDictionary<string, double> members)
        {
            return Db.SortedSetAdd(key, members.ToSortedSetEntry());
        }

        public Task<long> SortedSetAddAsync(string key, IDictionary<string, double> members)
        {
            return Db.SortedSetAddAsync(key, members.ToSortedSetEntry());
        }

        #endregion

        #region SortedSetCombineAndStore

        public long SortedSetCombineAndStore(RedisSetOperation operation, string desctination, string first, string second, RedisAggregate aggregate = RedisAggregate.Sum)
        {
            return Db.SortedSetCombineAndStore(operation.ToSetOperation(), desctination, first, second, aggregate.ToAggregate());
        }

        public Task<long> SortedSetCombineAndStoreAsync(RedisSetOperation operation, string desctination, string first, string second, RedisAggregate aggregate = RedisAggregate.Sum)
        {
            return Db.SortedSetCombineAndStoreAsync(operation.ToSetOperation(), desctination, first, second, aggregate.ToAggregate());
        }

        public long SortedSetCombineAndStore(RedisSetOperation operation, string desctination, string[] keys, double[] weights = null, RedisAggregate aggregate = RedisAggregate.Sum)
        {
            return Db.SortedSetCombineAndStore(operation.ToSetOperation(), desctination, keys.ToRedisKeyArray(), weights, aggregate.ToAggregate());
        }

        public Task<long> SortedSetCombineAndStoreAsync(RedisSetOperation operation, string desctination, string[] keys, double[] weights = null, RedisAggregate aggregate = RedisAggregate.Sum)
        {
            return Db.SortedSetCombineAndStoreAsync(operation.ToSetOperation(), desctination, keys.ToRedisKeyArray(), weights, aggregate.ToAggregate());
        }

        #endregion

        #region SortedSetDecrement

        public double SortedSetDecrement(string key, string member, double value)
        {
            return Db.SortedSetDecrement(key, member, value);
        }

        public Task<double> SortedSetDecrementAsync(string key, string member, double value)
        {
            return Db.SortedSetDecrementAsync(key, member, value);
        }

        #endregion

        #region SortedSetIncrement

        public double SortedSetIncrement(string key, string member, double value)
        {
            return Db.SortedSetIncrement(key, member, value);
        }

        public Task<double> SortedSetIncrementAsync(string key, string member, double value)
        {
            return Db.SortedSetIncrementAsync(key, member, value);
        }

        #endregion

        #region SortedSetLength

        public long SortedSetLength(string key, double min = double.NegativeInfinity, double max = double.PositiveInfinity, RedisExclude exclude = RedisExclude.None)
        {
            return Db.SortedSetLength(key, min, max, exclude.ToExclude());
        }

        public Task<long> SortedSetLengthAsync(string key, double min = double.NegativeInfinity, double max = double.PositiveInfinity, RedisExclude exclude = RedisExclude.None)
        {
            return Db.SortedSetLengthAsync(key, min, max, exclude.ToExclude());
        }

        #endregion

        #region SortedSetLengthByValue

        public long SortedSetLengthByValue(string key, string min, string max, RedisExclude exclude = RedisExclude.None)
        {
            return Db.SortedSetLengthByValue(key, min, max, exclude.ToExclude());
        }

        public Task<long> SortedSetLengthByValueAsync(string key, string min, string max, RedisExclude exclude = RedisExclude.None)
        {
            return Db.SortedSetLengthByValueAsync(key, min, max, exclude.ToExclude());
        }

        #endregion

        #region SortedSetRangeByRank

        public string[] SortedSetRangeByRank(string key, long start = 0, long stop = -1, RedisOrder order = RedisOrder.Ascending)
        {
            return Db.SortedSetRangeByRank(key, start, stop, order.ToOrder()).ToStringArray();
        }

        public async Task<string[]> SortedSetRangeByRankAsync(string key, long start = 0, long stop = -1, RedisOrder order = RedisOrder.Ascending)
        {
            return (await Db.SortedSetRangeByRankAsync(key, start, stop, order.ToOrder())).ToStringArray();
        }

        #endregion

        #region SortedSetRangeByRankWithScores

        public KeyValuePair<string, double>[] SortedSetRangeByRankWithScores(string key, long start = 0, long stop = -1, RedisOrder order = RedisOrder.Ascending)
        {
            return Db.SortedSetRangeByRankWithScores(key, start, stop, order.ToOrder()).ToSortedPairs();
        }

        public async Task<KeyValuePair<string, double>[]> SortedSetRangeByRankWithScoresAsync(string key, long start = 0, long stop = -1, RedisOrder order = RedisOrder.Ascending)
        {
            return (await Db.SortedSetRangeByRankWithScoresAsync(key, start, stop, order.ToOrder())).ToSortedPairs();
        }

        #endregion

        #region SortedSetRangeByScore

        public string[] SortedSetRangeByScore(string key, double start = double.NegativeInfinity, double stop = double.PositiveInfinity, RedisExclude exclude = RedisExclude.None, RedisOrder order = RedisOrder.Ascending, long skip = 0, long take = -1)
        {
            return Db.SortedSetRangeByScore(key, start, stop, exclude.ToExclude(), order.ToOrder(), skip, take).ToStringArray();
        }

        public async Task<string[]> SortedSetRangeByScoreAsync(string key, double start = double.NegativeInfinity, double stop = double.PositiveInfinity, RedisExclude exclude = RedisExclude.None, RedisOrder order = RedisOrder.Ascending, long skip = 0, long take = -1)
        {
            return (await Db.SortedSetRangeByScoreAsync(key, start, stop, exclude.ToExclude(), order.ToOrder(), skip, take)).ToStringArray();
        }

        #endregion

        #region SortedSetRangeByScoreWithScores

        public KeyValuePair<string, double>[] SortedSetRangeByScoreWithScores(string key, double start = double.NegativeInfinity, double stop = double.PositiveInfinity, RedisExclude exclude = RedisExclude.None, RedisOrder order = RedisOrder.Ascending, long skip = 0, long take = -1)
        {
            return Db.SortedSetRangeByScoreWithScores(key, start, stop, exclude.ToExclude(), order.ToOrder(), skip, take).ToSortedPairs();
        }

        public async Task<KeyValuePair<string, double>[]> SortedSetRangeByScoreWithScoresAsync(string key, double start = double.NegativeInfinity, double stop = double.PositiveInfinity, RedisExclude exclude = RedisExclude.None, RedisOrder order = RedisOrder.Ascending, long skip = 0, long take = -1)
        {
            return (await Db.SortedSetRangeByScoreWithScoresAsync(key, start, stop, exclude.ToExclude(), order.ToOrder(), skip, take)).ToSortedPairs();
        }

        #endregion

        #region SortedSetRangeByValue

        public string[] SortedSetRangeByValue(string key, string min = null, string max = null, RedisExclude exclude = RedisExclude.None, long skip = 0, long take = -1)
        {
            return Db.SortedSetRangeByValue(key, min, max, exclude.ToExclude(), skip, take).ToStringArray();
        }

        public async Task<string[]> SortedSetRangeByValueAsync(string key, string min = null, string max = null, RedisExclude exclude = RedisExclude.None, long skip = 0, long take = -1)
        {
            return (await Db.SortedSetRangeByValueAsync(key, min, max, exclude.ToExclude(), skip, take)).ToStringArray();
        }

        #endregion

        #region SortedSetRank

        public long? SortedSetRank(string key, string member, RedisOrder order = RedisOrder.Ascending)
        {
            return Db.SortedSetRank(key, member, order.ToOrder());
        }

        public Task<long?> SortedSetRankAsync(string key, string member, RedisOrder order = RedisOrder.Ascending)
        {
            return Db.SortedSetRankAsync(key, member, order.ToOrder());
        }

        #endregion

        #region SortedSetRemove

        public bool SortedSetRemove(string key, string member)
        {
            return Db.SortedSetRemove(key, member);
        }

        public long SortedSetRemove(string key, string[] members)
        {
            return Db.SortedSetRemove(key, RedisExtensions.ToRedisValueArray(members));
        }

        public Task<bool> SortedSetRemoveAsync(string key, string member)
        {
            return Db.SortedSetRemoveAsync(key, member);
        }

        public Task<long> SortedSetRemoveAsync(string key, string[] members)
        {
            return Db.SortedSetRemoveAsync(key, RedisExtensions.ToRedisValueArray(members));
        }

        #endregion

        #region SortedSetRemoveRangeByRank

        public long SortedSetRemoveRangeByRank(string key, long start, long stop)
        {
            return Db.SortedSetRemoveRangeByRank(key, start, stop);
        }

        public Task<long> SortedSetRemoveRangeByRankAsync(string key, long start, long stop)
        {
            return Db.SortedSetRemoveRangeByRankAsync(key, start, stop);
        }

        #endregion

        #region SortedSetRemoveRangeByScore

        public long SortedSetRemoveRangeByScore(string key, double start, double stop, RedisExclude exclude = RedisExclude.None)
        {
            return Db.SortedSetRemoveRangeByScore(key, start, stop, exclude.ToExclude());
        }

        public Task<long> SortedSetRemoveRangeByScoreAsync(string key, double start, double stop, RedisExclude exclude = RedisExclude.None)
        {
            return Db.SortedSetRemoveRangeByScoreAsync(key, start, stop, exclude.ToExclude());
        }

        #endregion

        #region SortedSetRemoveRangeByValue

        public long SortedSetRemoveRangeByValue(string key, string min, string max, RedisExclude exclude = RedisExclude.None)
        {
            return Db.SortedSetRemoveRangeByValue(key, min, max, exclude.ToExclude());
        }

        public Task<long> SortedSetRemoveRangeByValueAsync(string key, string min, string max, RedisExclude exclude = RedisExclude.None)
        {
            return Db.SortedSetRemoveRangeByValueAsync(key, min, max, exclude.ToExclude());
        }

        #endregion

        #region SortedSetScan

        public KeyValuePair<string, double>[] SortedSetScan(string key, string pattern = null, int pageSize = 10, int cursor = 0, int pageOffset = 0)
        {
            return Db.SortedSetScan(key, pattern, pageSize, cursor, pageOffset).ToSortedPairs();
        }

        #endregion

        #region SortedSetScore

        public double? SortedSetScore(string key, string member)
        {
            if (key.IsNullOrWhiteSpace() || member.IsNullOrWhiteSpace())
                return null;

            return Db.SortedSetScore(key, member);
        }

        public Task<double?> SortedSetScoreAsync(string key, string member)
        {
            if (key.IsNullOrWhiteSpace() || member.IsNullOrWhiteSpace())
                return null;

            return Db.SortedSetScoreAsync(key, member);
        }

        #endregion

        #endregion
    }

    public interface ICache
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="seconds">缓存时间（秒）</param>
        /// <returns></returns>
        bool Add(string key, string value, int seconds = 0);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="seconds">缓存时间（秒）</param>
        /// <returns></returns>
        bool Add<T>(string key, T value, int seconds = 0) where T : class, new();

        /// <summary>
        /// 存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="seconds">缓存时间（秒）</param>
        /// <returns></returns>
        bool Set(string key, string value, int seconds = 0);

        /// <summary>
        /// 存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="seconds">缓存时间（秒）</param>
        /// <returns></returns>
        bool Set<T>(string key, T value, int seconds = 0) where T : class, new();

        /// <summary>
        /// 取
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        string Get(string key);

        /// <summary>
        /// 取
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        T Get<T>(string key) where T : class;

        /// <summary>
        /// 取
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="aquire">委托</param>
        /// <param name="seconds">缓存时间（秒）</param>
        /// <returns></returns>
        string GetOrAdd(string key, Func<string> aquire, int seconds = 0);

        /// <summary>
        /// 取
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="aquire">委托</param>
        /// <param name="seconds">缓存时间（秒）</param>
        /// <returns></returns>
        T GetOrAdd<T>(string key, Func<T> aquire, int seconds = 0) where T : class, new();

        /// <summary>
        /// 取
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="aquire">委托</param>
        /// <param name="seconds">缓存时间（秒）</param>
        /// <returns></returns>
        string GetOrSet(string key, Func<string> aquire, int seconds = 0);

        /// <summary>
        /// 取
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="aquire">委托</param>
        /// <param name="seconds">缓存时间（秒）</param>
        /// <returns></returns>
        T GetOrSet<T>(string key, Func<T> aquire, int seconds = 0) where T : class, new();

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        bool Contains(string key);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        bool Remove(string key);
    }
    internal static class RedisExtensions
    {
        internal static HashEntry[] GetHashEntry<T>(this T entity, PropertyInfo[] propertyInfos)
        {
            return (from propertyInfo in propertyInfos
                    let value = propertyInfo.GetValue(entity).ToString()
                    let filedName = propertyInfo.Name
                    select new HashEntry(filedName, value)).ToArray();
        }

        internal static RedisKey[] ToRedisKeyArray(this string[] keys)
        {
            var arrayKey = new RedisKey[keys.Length];

            for (int i = 0, j = keys.Length; i < j; i++)
            {
                arrayKey[i] = keys[i];
            }

            return arrayKey;
        }

        internal static RedisValue[] ToRedisValueArray(this string[] values)
        {
            var arrayValue = new RedisValue[values.Length];

            for (int i = 0, j = values.Length; i < j; i++)
            {
                arrayValue[i] = values[i];
            }

            return arrayValue;
        }

        internal static KeyValuePair<RedisKey, RedisValue>[] ToKeyValuePairArray(this IDictionary<string, string> kvs)
        {
            return kvs.Select(a => new KeyValuePair<RedisKey, RedisValue>(a.Key, a.Value)).ToArray();
        }

        internal static KeyValuePair<string, string>[] ToHashPairs(this IEnumerable<HashEntry> entries)
        {
            return entries == null ? null : ToHashPairs(entries.ToArray());
        }

        internal static KeyValuePair<string, string>[] ToHashPairs(this HashEntry[] entries)
        {
            if (entries == null)
                return null;

            var result = new KeyValuePair<string, string>[entries.Length];
            for (int i = 0, j = result.Length; i < j; i++)
            {
                result[i] = new KeyValuePair<string, string>(entries[i].Name, entries[i].Value.HasValue ? (string)entries[i].Value : null);
            }

            return result;
        }

        internal static KeyValuePair<string, double>[] ToSortedPairs(this IEnumerable<SortedSetEntry> entries)
        {
            return entries == null ? null : ToSortedPairs(entries.ToArray());
        }

        internal static KeyValuePair<string, double>[] ToSortedPairs(this SortedSetEntry[] entries)
        {
            if (entries == null)
                return null;

            var result = new KeyValuePair<string, double>[entries.Length];
            for (int i = 0, j = result.Length; i < j; i++)
            {
                result[i] = new KeyValuePair<string, double>(entries[i].Element, entries[i].Score);
            }

            return result;
        }

        internal static SortedSetEntry[] ToSortedSetEntry(this IDictionary<string, double> members)
        {
            return members?.Select(a => new SortedSetEntry(a.Key, a.Value)).ToArray();
        }

        internal static TimeSpan? ToRedisTimeSpan(this int seconds)
        {
            return seconds <= 0 ? (TimeSpan?)null : TimeSpan.FromSeconds(seconds);
        }

        #region 枚举转换

        internal static SetOperation ToSetOperation(this RedisSetOperation operation)
        {
            switch (operation)
            {
                case RedisSetOperation.Union:
                    return SetOperation.Union;

                case RedisSetOperation.Intersect:
                    return SetOperation.Intersect;

                case RedisSetOperation.Difference:
                    return SetOperation.Difference;

                default:
                    throw new Exception("RedisProxy 非法的集合运算");
            }
        }

        internal static Aggregate ToAggregate(this RedisAggregate aggregate)
        {
            switch (aggregate)
            {
                case RedisAggregate.Sum:
                    return Aggregate.Sum;

                case RedisAggregate.Max:
                    return Aggregate.Max;

                case RedisAggregate.Min:
                    return Aggregate.Min;

                default:
                    throw new Exception("RedisProxy 非法的聚合运算");
            }
        }

        internal static Order ToOrder(this RedisOrder order)
        {
            switch (order)
            {
                case RedisOrder.Ascending:
                    return Order.Ascending;

                case RedisOrder.Descending:
                    return Order.Descending;

                default:
                    throw new Exception("RedisProxy 非法顺序");
            }
        }

        internal static Exclude ToExclude(this RedisExclude exclude)
        {
            return (Exclude)(int)exclude;
        }

        internal static RedisItemType ToType(this RedisType type)
        {
            return (RedisItemType)(int)type;
        }

        #endregion
    }

    #region  枚举
    public enum RedisItemType
    {
        None = 0,
        String = 1,
        List = 2,
        Set = 3,
        SortedSet = 4,
        Hash = 5,
        Unknown = 6,
    }

    public enum RedisSetOperation
    {
        Union,
        Intersect,
        Difference
    }

    public enum RedisAggregate
    {
        Sum,
        Min,
        Max
    }

    [Flags]
    public enum RedisExclude
    {
        None = 0,
        Start = 1,
        Stop = 2,
        Both = Start | Stop
    }

    public enum RedisOrder
    {
        Ascending,
        Descending
    }
    #endregion

    #region 序列化
    public static class SerializeExtension
    {
        #region JSON序列化
        /// <summary>
        /// 实体对象转字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="ignoreNull"></param>
        /// <returns></returns>
        public static string ToJson(this object obj, bool ignoreNull = false)
        {
            if (obj.IsNull())
                return null;

            return JsonConvert.SerializeObject(obj, Formatting.None, new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-dd HH:mm:ss",
                NullValueHandling = ignoreNull ? NullValueHandling.Ignore : NullValueHandling.Include
            });
        }

        /// <summary>
        /// JSON字符串转实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static T FromJson<T>(this string jsonStr)
        {
            return jsonStr.IsNullOrEmpty() ? default(T) : JsonConvert.DeserializeObject<T>(jsonStr);
        }
        #endregion

        #region 字节序列
        /// <summary>
        /// 字符串序列化成字节序列
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] SerializeUtf8(this string str)
        {
            return str == null ? null : Encoding.UTF8.GetBytes(str);
        }

        /// <summary>
        /// 字节序列序列化成字符串
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string DeserializeUtf8(this byte[] stream)
        {
            return stream == null ? null : Encoding.UTF8.GetString(stream);
        }
        #endregion

        private static readonly ConcurrentDictionary<Type, PropertyInfo[]> PropertiesDic = new ConcurrentDictionary<Type, PropertyInfo[]>();
        /// <summary>
        /// 获取实体字段
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static PropertyInfo[] GetEntityProperties(this Type type)
        {
            return PropertiesDic.GetOrAdd(type, item => type.GetProperties(BindingFlags.Instance | BindingFlags.Public));
        }
    }

    public static class ObjectExtension
    {
        /// <summary>
        /// 对象是空
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNull(this object obj)
        {
            return obj == null;
        }

        /// <summary>
        /// 对象不为空
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNotNull(this object obj)
        {
            return obj != null;
        }

        public static string ToStr(this object input)
        {
            return input.IsNull() ? null : input.ToString();
        }

        public static void ThrowIfNull(this object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
        }
    }
    #endregion
}
