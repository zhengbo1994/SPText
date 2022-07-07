using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using System.Linq.Expressions;
using System.Web;
using MongoDB;
using System.Configuration;
using System.Threading.Tasks;

namespace SPText.Common.MongoDB
{
    public partial class MongoDBHelper
    {
        private string databaseName = string.Empty;
        private IMongoClient client = null;
        private IMongoDatabase database = null;

        public MongoDBHelper(string connectionString)
        {
            client = new MongoClient(connectionString);
        }

        public MongoDBHelper(string connectionString, string databaseName)
        {
            client = new MongoClient(connectionString);
            database = client.GetDatabase(databaseName);
        }

        public string DatabaseName
        {
            get { return databaseName; }
            set
            {
                databaseName = value;
                database = client.GetDatabase(databaseName);
            }
        }

        /// <summary>
        /// 执行命令，命令请参考MongoCommand,命令太多，不一一展示，传入的就是里面的字符串，有些命令执行需要连接到admin表
        /// </summary>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        public BsonDocument RunCommand(string cmdText)
        {
            return database.RunCommand<BsonDocument>(cmdText);
        }

        public IList<BsonDocument> GetDatabase()
        {
            return client.ListDatabases().ToList();
        }

        #region SELECT
        /// <summary>
        /// 判断文档存在状态
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="filterexist"></param>
        /// <returns></returns>
        public bool IsExistDocument<T>(string documentname, FilterDefinition<T> filter)
        {
            return database.GetCollection<T>(documentname).Count(filter) > 0;
        }

        /// <summary>
        /// 通过条件得到查询的结果个数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public long GetCount<T>(string documentname, FilterDefinition<T> filter)
        {
            return database.GetCollection<T>(documentname).Count(filter);
        }

        /// <summary>
        /// 通过系统id(ObjectId)获取一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetDocumentById<T>(string documentname, string id)
        {
            ObjectId oid = ObjectId.Parse(id);
            var filter = Builders<T>.Filter.Eq("_id", oid);
            var result = database.GetCollection<T>(documentname).Find(filter);
            return result.FirstOrDefault();
        }

        /// <summary>
        /// 通过系统id(ObjectId)获取一个对象同时过滤字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="id"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public T GetDocumentById<T>(string documentname, string id, ProjectionDefinition<T> fields)
        {
            ObjectId oid = ObjectId.Parse(id);
            var filter = Builders<T>.Filter.Eq("_id", oid);
            return database.GetCollection<T>(documentname).Find(filter).Project<T>(fields).FirstOrDefault();
        }

        /// <summary>
        /// 通过指定的条件获取一个对象，如果有多条，只取第一条，同时过滤字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="filter"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public T GetDocumentByUserFilter<T>(string documentname, FilterDefinition<T> filter, ProjectionDefinition<T> fields)
        {
            return database.GetCollection<T>(documentname).Find(filter).Project<T>(fields).FirstOrDefault();
        }

        /// <summary>
        /// 获取全部文档
        /// </summary>
        /// <typeparam name="T"></typeparam>       
        /// <param name="documentname"></param>
        /// <returns></returns>
        public IList<T> GetAllDocuments<T>(string documentname)
        {
            var filter = Builders<T>.Filter.Empty;
            return database.GetCollection<T>(documentname).Find(filter).ToList();
        }

        /// <summary>
        /// 获取全部文档同时过滤字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="fields">要获取的字段</param>
        /// <returns></returns>
        public IList<T> GetAllDocuments<T>(string documentname, ProjectionDefinition<T> fields)
        {
            var filter = Builders<T>.Filter.Empty;
            return database.GetCollection<T>(documentname).Find(filter).Project<T>(fields).ToList();
        }

        /// <summary>
        /// 通过一个条件获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="property">字段名</param>
        /// <param name="value">字段值</param>
        /// <returns></returns>
        public IList<T> GetDocumentsByFilter<T>(string documentname, string property, string value)
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Eq(property, value);
            return database.GetCollection<T>(documentname).Find(filter).ToList();
        }

        /// <summary>
        /// 通过条件获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IList<T> GetDocumentsByFilter<T>(string documentname, FilterDefinition<T> filter)
        {
            return database.GetCollection<T>(documentname).Find(filter).ToList();
        }

        /// <summary>
        /// 通过条件获取对象,同时过滤字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="property">字段名</param>
        /// <param name="value">字段值</param>
        /// <param name="fields">要获取的字段</param>
        /// <returns></returns>
        public IList<T> GetDocumentsByFilter<T>(string documentname, string property, string value, ProjectionDefinition<T> fields)
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Eq(property, value);
            return database.GetCollection<T>(documentname).Find(filter).Project<T>(fields).ToList();
        }

        /// <summary>
        /// 通过条件获取对象,同时过滤数据和字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="filter">过滤器</param>
        /// <param name="fields">要获取的字段</param>
        /// <returns></returns>
        public IList<T> GetDocumentsByFilter<T>(string documentname, FilterDefinition<T> filter, ProjectionDefinition<T> fields)
        {
            return database.GetCollection<T>(documentname).Find(filter).Project<T>(fields).ToList();
        }

        /// <summary>
        /// 通过条件获取分页的文档并排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IList<T> GetPagedDocumentsByFilter<T>(string documentname, FilterDefinition<T> filter, ProjectionDefinition<T> fields, SortDefinition<T> sort, int pageIndex, int pageSize)
        {
            IList<T> result = new List<T>();
            if (pageIndex != 0 && pageSize != 0)
            {
                result = database.GetCollection<T>(documentname).Find(filter).Project<T>(fields).Sort(sort).Skip(pageSize * (pageIndex - 1)).Limit(pageSize).ToList();
            }
            else
            {
                result = database.GetCollection<T>(documentname).Find(filter).Project<T>(fields).Sort(sort).ToList();
            }
            return result;
        }

        /// <summary>
        /// 通过条件获取分页的文档并排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IList<T> GetPagedDocumentsByFilter<T>(string documentname, FilterDefinition<T> filter, SortDefinition<T> sort, int pageIndex, int pageSize)
        {
            IList<T> result = new List<T>();
            if (pageIndex != 0 && pageSize != 0)
            {
                result = database.GetCollection<T>(documentname).Find(filter).Sort(sort).Skip(pageSize * (pageIndex - 1)).Limit(pageSize).ToList();
            }
            else
            {
                result = database.GetCollection<T>(documentname).Find(filter).Sort(sort).ToList();
            }
            return result;
        }

        /// <summary>
        /// 通过条件获取分页的文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="filter"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IList<T> GetPagedDocumentsByFilter<T>(string documentname, FilterDefinition<T> filter, int pageIndex, int pageSize)
        {
            IList<T> result = new List<T>();
            if (pageIndex != 0 && pageSize != 0)
            {
                result = database.GetCollection<T>(documentname).Find(filter).Skip(pageSize * (pageIndex - 1)).Limit(pageSize).ToList();
            }
            else
            {
                result = database.GetCollection<T>(documentname).Find(filter).ToList();
            }
            return result;
        }

        /// <summary>
        /// 获取分页的文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IList<T> GetPagedDocumentsByFilter<T>(string documentname, SortDefinition<T> sort, int pageIndex, int pageSize)
        {
            IList<T> result = new List<T>();
            var filter = Builders<T>.Filter.Empty;
            if (pageIndex != 0 && pageSize != 0)
            {
                result = database.GetCollection<T>(documentname).Find(filter).Sort(sort).Skip(pageSize * (pageIndex - 1)).Limit(pageSize).ToList();
            }
            else
            {
                result = database.GetCollection<T>(documentname).Find(filter).Sort(sort).ToList();
            }
            return result;
        }

        /// <summary>
        /// 获取分页的文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IList<T> GetPagedDocumentsByFilter<T>(string documentname, int pageIndex, int pageSize)
        {
            IList<T> result = new List<T>();
            var filter = Builders<T>.Filter.Empty;
            if (pageIndex != 0 && pageSize != 0)
            {
                result = database.GetCollection<T>(documentname).Find(filter).Skip(pageSize * (pageIndex - 1)).Limit(pageSize).ToList();
            }
            else
            {
                result = database.GetCollection<T>(documentname).Find(filter).ToList();
            }
            return result;
        }
        #endregion

        #region INSERT

        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="document"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public void Insert<T>(string documentName, T document)
        {
            try
            {
                database.GetCollection<T>(documentName).InsertOne(document);
            }
            catch (MongoWriteException me)
            {
                MongoBulkWriteException mbe = me.InnerException as MongoBulkWriteException;
                if (mbe != null && mbe.HResult == -2146233088)
                    throw new Exception("插入重复的键！");
                throw new Exception(mbe.Message);
            }
            catch (Exception ep)
            {
                throw ep;
            }
        }

        /// <summary>
        /// 新增多个文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="documents"></param>
        /// <returns></returns>
        public void InsertMany<T>(string documentname, IList<T> documents)
        {
            try
            {
                database.GetCollection<T>(documentname).InsertMany(documents);
            }
            catch (MongoWriteException me)
            {
                MongoBulkWriteException mbe = me.InnerException as MongoBulkWriteException;
                if (mbe != null && mbe.HResult == -2146233088)
                    throw new Exception("插入重复的键！");
                throw new Exception(mbe.Message);
            }
            catch (Exception ep)
            {
                throw ep;
            }
        }
        #endregion

        #region UPDATE
        /// <summary>
        /// 修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="filterexist"></param>
        /// <param name="id"></param>
        /// <param name="oldinfo"></param>
        /// <returns></returns>
        public void UpdateReplaceOne<T>(string documentname, string id, T oldinfo)
        {
            ObjectId oid = ObjectId.Parse(id);
            var filter = Builders<T>.Filter.Eq("_id", oid);
            database.GetCollection<T>(documentname).ReplaceOne(filter, oldinfo);
        }

        /// <summary>
        /// 只能替换一条，如果有多条的话
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="filter"></param>
        /// <param name="oldinfo"></param>
        public void UpdateReplaceOne<T>(string documentname, FilterDefinition<T> filter, T oldinfo)
        {
            database.GetCollection<T>(documentname).ReplaceOne(filter, oldinfo);
        }

        /// <summary>
        /// 更新指定属性值，按ID就只有一条，替换一条
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="id"></param>
        /// <param name="setvalue"></param>
        /// <returns></returns>
        public void Update<T>(string documentname, string id, string property, string value)
        {
            ObjectId oid = ObjectId.Parse(id);
            var filter = Builders<T>.Filter.Eq("_id", oid);
            var update = Builders<T>.Update.Set(property, value);
            database.GetCollection<T>(documentname).UpdateOne(filter, update);
        }

        public void Update<T>(string documentname, FilterDefinition<T> filter, UpdateDefinition<T> update)
        {
            database.GetCollection<T>(documentname).UpdateOne(filter, update);
        }

        public void UpdateMany<T>(string documentname, FilterDefinition<T> filter, UpdateDefinition<T> update)
        {
            database.GetCollection<T>(documentname).UpdateMany(filter, update);
        }

        #endregion

        #region DELETE
        /// <summary>
        /// 删除一个文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public void Delete<T>(string documentname, string id)
        {
            ObjectId oid = ObjectId.Parse(id);
            var filterid = Builders<T>.Filter.Eq("_id", oid);
            database.GetCollection<T>(documentname).DeleteOne(filterid);
        }

        public void Delete<T>(string documentname, string property, string value)
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Eq(property, value);
            database.GetCollection<T>(documentname).DeleteOne(filter);
        }

        /// <summary>
        /// 通过一个属性名和属性值删除多个文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public void DeleteMany<T>(string documentname, string property, string value)
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Eq(property, value);
            database.GetCollection<T>(documentname).DeleteMany(filter);
        }

        /// <summary>
        /// 通过一个属性名和属性值删除多个文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public void DeleteMany<T>(string documentname, FilterDefinition<T> filter)
        {
            database.GetCollection<T>(documentname).DeleteMany(filter);
        }
        #endregion

        /// <summary>
        /// 有些命令要求你连到系统库上才能执行
        /// You need to link to the admin table if you want to run system command;eg:listDatabases ,the following url show you the details
        /// https://docs.mongodb.com/manual/reference/command/listCommands/
        /// </summary>
        public sealed class MongoCommand
        {
            public const string ListDatabases = "{listDatabases:1}";
            public const string ListCommands = "{ listCommands: 1 }";
        }
    }


    public partial class MongoDbHelper
    {
        #region 构造函数
        /// <summary>
        /// 集合
        /// </summary>
        public string _collName { get; set; }
        /// <summary>
        /// 数据库对象
        /// </summary>
        public MongoDbHelper(string collName)
        {
            this._collName = collName;
        }

        #endregion


        #region 连接配置
        /// <summary>
        /// 链接
        /// </summary>
        private static readonly string conneStr = ConfigurationManager.AppSettings["MongoDbSource"];
        /// <summary>
        /// 数据库
        /// </summary>
        private static readonly string dbName = ConfigurationManager.AppSettings["MongoDbName"];
        #endregion

        #region 单例创建链接
        private static IMongoClient _mongoclient { get; set; }
        private static IMongoClient CreateClient()
        {
            if (_mongoclient == null)
            {
                _mongoclient = new MongoClient(conneStr);
            }
            return _mongoclient;
        }
        #endregion

        #region 获取链接和数据库

        private IMongoClient client = CreateClient();
        public IMongoDatabase _database { get { return _mongoclient.GetDatabase(dbName); } }

        public IMongoDatabase GetDatabase()
        {
            return _database;
        }
        public IMongoCollection<T> GetClient<T>() where T : class, new()
        {
            return _database.GetCollection<T>(_collName);
        }
        #endregion


        #region +Add 添加一条数据
        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <param name="t">添加的实体</param>
        /// <param name="host">mongodb连接信息</param>
        /// <returns></returns>
        public int Add<T>(T t) where T : class, new()
        {
            try
            {
                var client = _database.GetCollection<T>(_collName);
                client.InsertOne(t);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        #endregion

        #region +AddAsync 异步添加一条数据
        /// <summary>
        /// 异步添加一条数据
        /// </summary>
        /// <param name="t">添加的实体</param>
        /// <param name="host">mongodb连接信息</param>
        /// <returns></returns>
        public async Task<int> AddAsync<T>(T t) where T : class, new()
        {
            try
            {
                var client = _database.GetCollection<T>(_collName);
                await client.InsertOneAsync(t);
                return 1;
            }
            catch
            {
                return 0;
            }
        }
        #endregion

        #region +InsertMany 批量插入
        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="host">mongodb连接信息</param>
        /// <param name="t">实体集合</param>
        /// <returns></returns>
        public int InsertMany<T>(List<T> t) where T : class, new()
        {
            try
            {
                var client = _database.GetCollection<T>(_collName);
                client.InsertMany(t);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        #endregion

        #region +InsertManyAsync 异步批量插入
        /// <summary>
        /// 异步批量插入
        /// </summary>
        /// <param name="host">mongodb连接信息</param>
        /// <param name="t">实体集合</param>
        /// <returns></returns>
        public async Task<int> InsertManyAsync<T>(List<T> t) where T : class, new()
        {
            try
            {
                var client = _database.GetCollection<T>(_collName);
                await client.InsertManyAsync(t);
                return 1;
            }
            catch
            {
                return 0;
            }
        }
        #endregion

        #region +Update 修改一条数据
        /// <summary>
        /// 修改一条数据
        /// </summary>
        /// <param name="t">添加的实体</param>
        /// <param name="host">mongodb连接信息</param>
        /// <returns></returns>
        public UpdateResult Update<T>(T t, string id, bool isObjectId = true) where T : class, new()
        {
            try
            {
                var client = _database.GetCollection<T>(_collName);
                //修改条件
                FilterDefinition<T> filter;
                if (isObjectId)
                {
                    filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
                }
                else
                {
                    filter = Builders<T>.Filter.Eq("_id", id);
                }
                //要修改的字段
                var list = new List<UpdateDefinition<T>>();
                foreach (var item in t.GetType().GetProperties())
                {
                    if (item.Name.ToLower() == "id") continue;
                    list.Add(Builders<T>.Update.Set(item.Name, item.GetValue(t)));
                }
                var updatefilter = Builders<T>.Update.Combine(list);
                return client.UpdateOne(filter, updatefilter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region +UpdateAsync 异步修改一条数据
        /// <summary>
        /// 异步修改一条数据
        /// </summary>
        /// <param name="t">添加的实体</param>
        /// <param name="host">mongodb连接信息</param>
        /// <returns></returns>
        public async Task<UpdateResult> UpdateAsync<T>(T t, string id, bool isObjectId) where T : class, new()
        {
            try
            {
                var client = _database.GetCollection<T>(_collName);
                //修改条件
                FilterDefinition<T> filter;
                if (isObjectId)
                {
                    filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
                }
                else
                {
                    filter = Builders<T>.Filter.Eq("_id", id);
                }
                //要修改的字段
                var list = new List<UpdateDefinition<T>>();
                foreach (var item in t.GetType().GetProperties())
                {
                    if (item.Name.ToLower() == "id") continue;
                    list.Add(Builders<T>.Update.Set(item.Name, item.GetValue(t)));
                }
                var updatefilter = Builders<T>.Update.Combine(list);
                return await client.UpdateOneAsync(filter, updatefilter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region +UpdateManay 批量修改数据
        /// <summary>
        /// 批量修改数据
        /// </summary>
        /// <param name="dic">要修改的字段</param>
        /// <param name="host">mongodb连接信息</param>
        /// <param name="filter">修改条件</param>
        /// <returns></returns>
        public UpdateResult UpdateManay<T>(Dictionary<string, string> dic, FilterDefinition<T> filter) where T : class, new()
        {
            try
            {
                var client = _database.GetCollection<T>(_collName);
                T t = new T();
                //要修改的字段
                var list = new List<UpdateDefinition<T>>();
                foreach (var item in t.GetType().GetProperties())
                {
                    if (!dic.ContainsKey(item.Name)) continue;
                    var value = dic[item.Name];
                    list.Add(Builders<T>.Update.Set(item.Name, value));
                }
                var updatefilter = Builders<T>.Update.Combine(list);
                return client.UpdateMany(filter, updatefilter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region +UpdateManayAsync 异步批量修改数据
        /// <summary>
        /// 异步批量修改数据
        /// </summary>
        /// <param name="dic">要修改的字段</param>
        /// <param name="host">mongodb连接信息</param>
        /// <param name="filter">修改条件</param>
        /// <returns></returns>
        public async Task<UpdateResult> UpdateManayAsync<T>(Dictionary<string, string> dic, FilterDefinition<T> filter) where T : class, new()
        {
            try
            {
                var client = _database.GetCollection<T>(_collName);
                T t = new T();
                //要修改的字段
                var list = new List<UpdateDefinition<T>>();
                foreach (var item in t.GetType().GetProperties())
                {
                    if (!dic.ContainsKey(item.Name)) continue;
                    var value = dic[item.Name];
                    list.Add(Builders<T>.Update.Set(item.Name, value));
                }
                var updatefilter = Builders<T>.Update.Combine(list);
                return await client.UpdateManyAsync(filter, updatefilter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region Delete 删除一条数据
        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="host">mongodb连接信息</param>
        /// <param name="id">objectId</param>
        /// <returns></returns>
        public DeleteResult Delete<T>(string id, bool isObjectId = true) where T : class, new()
        {
            try
            {
                var client = _database.GetCollection<T>(_collName);
                FilterDefinition<T> filter;
                if (isObjectId)
                {
                    filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
                }
                else
                {
                    filter = Builders<T>.Filter.Eq("_id", id);
                }
                return client.DeleteOne(filter);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region DeleteAsync 异步删除一条数据
        /// <summary>
        /// 异步删除一条数据
        /// </summary>
        /// <param name="host">mongodb连接信息</param>
        /// <param name="id">objectId</param>
        /// <returns></returns>
        public async Task<DeleteResult> DeleteAsync<T>(string id, bool isObjectId = true) where T : class, new()
        {
            try
            {
                var client = _database.GetCollection<T>(_collName);
                //修改条件
                FilterDefinition<T> filter;
                if (isObjectId)
                {
                    filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
                }
                else
                {
                    filter = Builders<T>.Filter.Eq("_id", id);
                }
                return await client.DeleteOneAsync(filter);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region DeleteMany 删除多条数据
        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="host">mongodb连接信息</param>
        /// <param name="filter">删除的条件</param>
        /// <returns></returns>
        public DeleteResult DeleteMany<T>(FilterDefinition<T> filter) where T : class, new()
        {
            try
            {
                var client = _database.GetCollection<T>(_collName);
                return client.DeleteMany(filter);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region DeleteManyAsync 异步删除多条数据
        /// <summary>
        /// 异步删除多条数据
        /// </summary>
        /// <param name="host">mongodb连接信息</param>
        /// <param name="filter">删除的条件</param>
        /// <returns></returns>
        public async Task<DeleteResult> DeleteManyAsync<T>(FilterDefinition<T> filter) where T : class, new()
        {
            try
            {
                var client = _database.GetCollection<T>(_collName);
                return await client.DeleteManyAsync(filter);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion


        #region FindOne 根据id查询一条数据
        /// <summary>
        /// 根据id查询一条数据
        /// </summary>
        /// <param name="host">mongodb连接信息</param>
        /// <param name="id">objectid</param>
        /// <param name="field">要查询的字段，不写时查询全部</param>
        /// <returns></returns>
        public T FindOne<T>(string id, bool isObjectId = true, string[] field = null) where T : class, new()
        {
            try
            {
                var client = _database.GetCollection<T>(_collName);
                FilterDefinition<T> filter;
                if (isObjectId)
                {
                    filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
                }
                else
                {
                    filter = Builders<T>.Filter.Eq("_id", id);
                }
                //不指定查询字段
                if (field == null || field.Length == 0)
                {
                    return client.Find(filter).FirstOrDefault<T>();
                }

                //制定查询字段
                var fieldList = new List<ProjectionDefinition<T>>();
                for (int i = 0; i < field.Length; i++)
                {
                    fieldList.Add(Builders<T>.Projection.Include(field[i].ToString()));
                }
                var projection = Builders<T>.Projection.Combine(fieldList);
                fieldList?.Clear();
                return client.Find(filter).Project<T>(projection).FirstOrDefault<T>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region FindOneAsync 异步根据id查询一条数据
        /// <summary>
        /// 异步根据id查询一条数据
        /// </summary>
        /// <param name="host">mongodb连接信息</param>
        /// <param name="id">objectid</param>
        /// <returns></returns>
        public async Task<T> FindOneAsync<T>(string id, bool isObjectId = true, string[] field = null) where T : class, new()
        {
            try
            {
                var client = _database.GetCollection<T>(_collName);
                FilterDefinition<T> filter;
                if (isObjectId)
                {
                    filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
                }
                else
                {
                    filter = Builders<T>.Filter.Eq("_id", id);
                }

                //不指定查询字段
                if (field == null || field.Length == 0)
                {
                    return await client.Find(filter).FirstOrDefaultAsync();
                }

                //制定查询字段
                var fieldList = new List<ProjectionDefinition<T>>();
                for (int i = 0; i < field.Length; i++)
                {
                    fieldList.Add(Builders<T>.Projection.Include(field[i].ToString()));
                }
                var projection = Builders<T>.Projection.Combine(fieldList);
                fieldList?.Clear();
                return await client.Find(filter).Project<T>(projection).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region FindList 查询集合
        /// <summary>
        /// 查询集合
        /// </summary>
        /// <param name="host">mongodb连接信息</param>
        /// <param name="filter">查询条件</param>
        /// <param name="field">要查询的字段,不写时查询全部</param>
        /// <param name="sort">要排序的字段</param>
        /// <returns></returns>
        public List<T> FindList<T>(FilterDefinition<T> filter, string[] field = null, SortDefinition<T> sort = null) where T : class, new()
        {
            try
            {
                var client = _database.GetCollection<T>(_collName);
                //不指定查询字段
                if (field == null || field.Length == 0)
                {
                    if (sort == null) return client.Find(filter).ToList();
                    //进行排序
                    return client.Find(filter).Sort(sort).ToList();
                }

                //制定查询字段
                var fieldList = new List<ProjectionDefinition<T>>();
                for (int i = 0; i < field.Length; i++)
                {
                    fieldList.Add(Builders<T>.Projection.Include(field[i].ToString()));
                }
                var projection = Builders<T>.Projection.Combine(fieldList);
                fieldList?.Clear();
                if (sort == null) return client.Find(filter).Project<T>(projection).ToList();
                //排序查询
                return client.Find(filter).Sort(sort).Project<T>(projection).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region FindListAsync 异步查询集合
        /// <summary>
        /// 异步查询集合
        /// </summary>
        /// <param name="host">mongodb连接信息</param>
        /// <param name="filter">查询条件</param>
        /// <param name="field">要查询的字段,不写时查询全部</param>
        /// <param name="sort">要排序的字段</param>
        /// <returns></returns>
        public async Task<List<T>> FindListAsync<T>(FilterDefinition<T> filter, string[] field = null, SortDefinition<T> sort = null) where T : class, new()
        {
            try
            {
                var client = _database.GetCollection<T>(_collName);
                //不指定查询字段
                if (field == null || field.Length == 0)
                {
                    if (sort == null) return await client.Find(filter).ToListAsync();
                    return await client.Find(filter).Sort(sort).ToListAsync();
                }

                //制定查询字段
                var fieldList = new List<ProjectionDefinition<T>>();
                for (int i = 0; i < field.Length; i++)
                {
                    fieldList.Add(Builders<T>.Projection.Include(field[i].ToString()));
                }
                var projection = Builders<T>.Projection.Combine(fieldList);
                fieldList?.Clear();
                if (sort == null) return await client.Find(filter).Project<T>(projection).ToListAsync();
                //排序查询
                return await client.Find(filter).Sort(sort).Project<T>(projection).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region FindListByPage 分页查询集合
        /// <summary>
        /// 分页查询集合
        /// </summary>
        /// <param name="host">mongodb连接信息</param>
        /// <param name="filter">查询条件</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="count">总条数</param>
        /// <param name="field">要查询的字段,不写时查询全部</param>
        /// <param name="sort">要排序的字段</param>
        /// <returns></returns>
        public List<T> FindListByPage<T>(FilterDefinition<T> filter, int pageIndex, int pageSize, out long count, string[] field = null, SortDefinition<T> sort = null) where T : class, new()
        {
            try
            {
                var client = _database.GetCollection<T>(_collName);
                count = client.CountDocuments(filter);
                //不指定查询字段
                if (field == null || field.Length == 0)
                {
                    if (sort == null) return client.Find(filter).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToList();
                    //进行排序
                    return client.Find(filter).Sort(sort).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToList();
                }

                //制定查询字段
                var fieldList = new List<ProjectionDefinition<T>>();
                for (int i = 0; i < field.Length; i++)
                {
                    fieldList.Add(Builders<T>.Projection.Include(field[i].ToString()));
                }
                var projection = Builders<T>.Projection.Combine(fieldList);
                fieldList?.Clear();

                //不排序
                if (sort == null) return client.Find(filter).Project<T>(projection).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToList();

                //排序查询
                return client.Find(filter).Sort(sort).Project<T>(projection).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region FindListByPageAsync 异步分页查询集合
        /// <summary>
        /// 异步分页查询集合
        /// </summary>
        /// <param name="host">mongodb连接信息</param>
        /// <param name="filter">查询条件</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="field">要查询的字段,不写时查询全部</param>
        /// <param name="sort">要排序的字段</param>
        /// <returns></returns>
        public async Task<List<T>> FindListByPageAsync<T>(FilterDefinition<T> filter, int pageIndex, int pageSize, string[] field = null, SortDefinition<T> sort = null) where T : class, new()
        {
            try
            {
                var client = _database.GetCollection<T>(_collName);
                //不指定查询字段
                if (field == null || field.Length == 0)
                {
                    if (sort == null) return await client.Find(filter).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToListAsync();
                    //进行排序
                    return await client.Find(filter).Sort(sort).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToListAsync();
                }

                //制定查询字段
                var fieldList = new List<ProjectionDefinition<T>>();
                for (int i = 0; i < field.Length; i++)
                {
                    fieldList.Add(Builders<T>.Projection.Include(field[i].ToString()));
                }
                var projection = Builders<T>.Projection.Combine(fieldList);
                fieldList?.Clear();

                //不排序
                if (sort == null) return await client.Find(filter).Project<T>(projection).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToListAsync();

                //排序查询
                return await client.Find(filter).Sort(sort).Project<T>(projection).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToListAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Count 根据条件获取总数
        /// <summary>
        /// 根据条件获取总数
        /// </summary>
        /// <param name="host">mongodb连接信息</param>
        /// <param name="filter">条件</param>
        /// <returns></returns>
        public long Count<T>(FilterDefinition<T> filter) where T : class, new()
        {
            try
            {
                var client = _database.GetCollection<T>(_collName);
                return client.CountDocuments(filter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region CountAsync 异步根据条件获取总数
        /// <summary>
        /// 异步根据条件获取总数
        /// </summary>
        /// <param name="host">mongodb连接信息</param>
        /// <param name="filter">条件</param>
        /// <returns></returns>
        public async Task<long> CountAsync<T>(FilterDefinition<T> filter) where T : class, new()
        {
            try
            {
                var client = _database.GetCollection<T>(_collName);
                return await client.CountDocumentsAsync(filter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
