using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SPText.Common.MongoDB.Framework002
{

	public class DBbase<T> where T : class, new()
	{
		MongoClient client;
		IMongoDatabase database;
		public IMongoCollection<T> collection;
		public DBbase()
		{
			//var client = new MongoClient("mongodb://host:27017,host2:27017/?replicaSet=rs0");
			client = new MongoClient("mongodb://39.96.34.52:27017");
			database = client.GetDatabase("test");
			Type type = typeof(T);
			collection = database.GetCollection<T>(type.Name.ToLower());
		}

		public void DropDatabase()
		{
			client.DropDatabase("test");
		}
		public void InsertOne(T model)
		{
			collection.InsertOne(model);
		}
		public void InsertMany(params T[] modes)
		{
			collection.InsertMany(modes);
		}
		public IMongoQueryable<T> Select()
		{
			return collection.AsQueryable<T>();
		}
		public IMongoQueryable<T> Select(int pageIndex, int pageSize)
		{
			return collection.AsQueryable<T>().Skip(pageSize * (pageIndex - 1)).Take(pageSize);
		}
		public IMongoQueryable<T> Select(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> keySelector, int pageIndex, int pageSize)
		{
			return collection.AsQueryable<T>().Where(predicate).OrderBy(keySelector).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
		}
		public void UpdateMany(Expression<Func<T, bool>> filter, UpdateDefinition<T> update)
		{
			collection.UpdateMany(filter, update);
		}

		public void UpdateOne(Expression<Func<T, bool>> filter, T update)
		{
			collection.ReplaceOne(filter, update);
		}

		public void DeleteMany(Expression<Func<T, bool>> filter)
		{
			collection.DeleteMany(filter);
		}

	}




	#region  其它
	public class JsonOperation
	{
		public static void Show()
		{
			//var client = new MongoClient("mongodb://192.168.2.202:27017");
			//var database = client.GetDatabase("test");
			//var document = BsonDocument.Parse("{ a: 1, b: [{ c: 1 }],c: 'ff'}");
			//var document1 = BsonDocument.Parse("{ a: 6666}");
			//database.GetCollection<BsonDocument>("userinfo").InsertOne(document1);

			//database.GetCollection<BsonDocument>("userinfo").InsertOne(document);

			var client = new MongoClient("mongodb://192.168.3.202:27017");
			var database = client.GetDatabase("mongodbDemo");
			var document = BsonDocument.Parse("{ a: 1, b: [{ c: 1 }],c: 'ff'}");
			database.GetCollection<BsonDocument>("order").InsertOne(document);
			Console.WriteLine("ok");

		}
	}
	public class LinqOperation
	{
		static DBbase<Userinfo> dBbase = new DBbase<Userinfo>();

		public static void DropDatabase()
		{
			dBbase.DropDatabase();
		}
		public static void InserOne()
		{


			dBbase.InsertOne(new Userinfo()
			{
				Id = Guid.NewGuid().ToString(),
				Name = "诸葛亮",
				Address = "蜀国",
				Age = 27,
				Sex = "男",
				DetpInfo = new DetpInfo()
				{
					DeptId = 1,
					DeptName = "蜀国集团"
				}
			});
			dBbase.InsertOne(new Userinfo()
			{
				Id = Guid.NewGuid().ToString(),
				Name = "荀彧",
				Address = "魏国",
				Age = 47,
				Sex = "男",
				DetpInfo = new DetpInfo()
				{
					DeptId = 2,
					DeptName = "魏国集团"
				}
			});
			Console.WriteLine("写入完成");
		}

		public static void InsertMany()
		{

			var zhouyu = new Userinfo()
			{
				Id = Guid.NewGuid().ToString(),
				Name = "周瑜",
				Address = "吴国",
				Age = 32,
				Sex = "男",
				DetpInfo = new DetpInfo() { DeptId = 3, DeptName = "吴国集团" }
			};
			var daqiao = new Userinfo()
			{
				Id = Guid.NewGuid().ToString(),
				Name = "大乔",
				Address = "吴国",
				Age = 14,
				Sex = "女",
				DetpInfo = new DetpInfo() { DeptId = 3, DeptName = "吴国集团" }
			};
			var caocao = new Userinfo()
			{
				Id = Guid.NewGuid().ToString(),
				Name = "曹操",
				Address = "魏国",
				Age = 32,
				Sex = "男",
				DetpInfo = new DetpInfo() { DeptId = 3, DeptName = "魏国集团" }
			};
			dBbase.InsertMany(zhouyu, daqiao, caocao);
			Console.WriteLine("批量写入完成");
		}

		public static void GetPage()
		{

			var pagelist = dBbase.Select(m => m.Sex == "男", m => m.Age, 2, 2);
			foreach (var item in pagelist)
			{
				Console.WriteLine(item.Name + ":" + item.Age);
			}

			var query = from p in dBbase.collection.AsQueryable()
						where (p.Sex == "男") && p.Age > 18
						select p;
			foreach (var item in query)
			{
				Console.WriteLine(item.Name + ":" + item.Age);
			}
		}

		public static void Update()
		{
			var daqiaod = dBbase.Select().Where(m => m.Name == "大乔").FirstOrDefault();
			daqiaod.Age = 18;
			dBbase.UpdateOne(m => m.Id == daqiaod.Id, daqiaod);
			Console.WriteLine("修改完成");
		}
		public static void DeleteMany()
		{
			dBbase.DeleteMany(m => m.Name == "大乔");
			Console.WriteLine("删除完成");
			//全部删除
			dBbase.DeleteMany(m => 1 == 1);
			Console.WriteLine("全部删除完成");


		}
		public static void GroupBy()
		{

			//linq  
			var groups = dBbase.collection.AsQueryable().GroupBy(m => new { m.DetpInfo.DeptId, m.DetpInfo.DeptName }).Select(t => new
			{
				DeptId = t.Key.DeptId,
				DeptName = t.Key.DeptName,
				number = t.Count(),
				ages = t.Sum(s => s.Age)
			}).Take(0).Skip(10);
			foreach (var item in groups)
			{
				Console.WriteLine(item.DeptName + ":" + item.number + ":" + item.ages);
			}
		}
	}
	public class Transaction
	{
		static object o = new object();
		public static void Show()
		{



			//MongoClient client = new MongoClient("mongodb://localhost:30001,localhost:30002,localhost:30003");
			MongoClient client = new MongoClient("mongodb://39.96.34.52:27017,47.95.2.2:27017,39.96.82.51:27017");

			//MongoClient client = new MongoClient("mongodb://localhost:30002");
			//事务
			var session = client.StartSession();
			var database = session.Client.GetDatabase("test");
			session.StartTransaction(new TransactionOptions(
				readConcern: ReadConcern.Snapshot,
				writeConcern: WriteConcern.WMajority));
			try
			{
				IMongoCollection<Userinfo> collection = database.GetCollection<Userinfo>("userinfo");
				IMongoCollection<DetpInfo> weiguocollection = database.GetCollection<DetpInfo>("deptindo");
				Userinfo daqiao = new Userinfo()
				{
					Id = Guid.NewGuid().ToString(),
					Address = "吴国",
					Name = "大乔",
					Sex = "女",
					DetpInfo = new DetpInfo()
					{
						DeptId = 1,
						DeptName = "蜀国集团"
					}
				};

				collection.InsertOne(session, daqiao);
				//throw new Exception("取消事务");

				DetpInfo weiguo = new DetpInfo() { DeptId = 1, DeptName = "魏国" };
				weiguocollection.InsertOne(session, weiguo);
				session.CommitTransaction();
			}
			catch (Exception ex)
			{
				//回滚
				session.AbortTransaction();
				Console.WriteLine(ex.Message);
			}

			Console.WriteLine("ok");
		}
	}
	public class Userinfo
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
		public string Sex { get; set; }

		public string dsfsdfsd { get; set; }

		public int Age { get; set; }
		public DetpInfo DetpInfo { get; set; }
	}

	public class DetpInfo
	{
		public int DeptId { get; set; }
		public string DeptName { get; set; }
	}
	#endregion
}
