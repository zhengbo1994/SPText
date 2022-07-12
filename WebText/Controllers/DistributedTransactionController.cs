using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Threading.Tasks;
using WebText.Data;
using WebText.Data.Models;

namespace WebText.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class DistributedTransactionController : ControllerBase
    {
        #region UserService
        public class UserController : ControllerBase
        {
            private static string PublishName = "RabbitMQ.SQLServer.UserService";

            private readonly IConfiguration _iConfiguration;
            /// <summary>
            /// 构造函数注入---默认IOC容器完成---注册是在AddCAP
            /// </summary>
            private readonly ICapPublisher _iCapPublisher;
            private readonly CommonServiceDbContext _UserServiceDbContext;
            private readonly ILogger<UserController> _Logger;

            public UserController(ICapPublisher capPublisher, IConfiguration configuration, CommonServiceDbContext userServiceDbContext, ILogger<UserController> logger)
            {
                this._iCapPublisher = capPublisher;
                this._iConfiguration = configuration;
                this._UserServiceDbContext = userServiceDbContext;
                this._Logger = logger;
            }
            //{"Headers":{"cap-callback-name":null,"cap-msg-id":"1403333028973604864","cap-corr-id":"1403333028973604864","cap-corr-seq":"0","cap-msg-name":"RabbitMQ.SQLServer.UserService","cap-msg-type":"User","cap-senttime":"2021/6/11 20:47:21 \u002B08:00"},"Value":{"Id":1,"Name":"\u81EA\u95ED\u75C7\u60A3\u80052021","Account":"admin","Password":"e10adc3949ba59abbe56e057f20f883e","Email":"12","Mobile":"133","CompanyId":1,"CompanyName":"\u767E\u6377","State":0,"UserType":2,"LastLoginTime":"2015-12-12T00:00:00","CreateTime":"2015-12-12T00:00:00","CreatorId":1,"LastModifierId":1,"LastModifyTime":"2019-04-11T10:48:43"}}

            [Route("/without/transaction")]//根目录
            public async Task<IActionResult> WithoutTransaction()
            {
                var user = this._UserServiceDbContext.User.Find(1);
                this._Logger.LogWarning($"This is WithoutTransaction Invoke");
                await _iCapPublisher.PublishAsync(PublishName, user);//应该把数据写到publish表
                return Ok();
            }

            [Route("/adotransaction/sync")]//根目录
            public IActionResult AdoTransaction()
            {
                var user = this._UserServiceDbContext.User.Find(1);
                IDictionary<string, string> dicHeader = new Dictionary<string, string>();
                dicHeader.Add("Teacher", "Eleven");
                dicHeader.Add("Student", "Seven");
                dicHeader.Add("Version", "1.2");

                using (var connection = new SqlConnection(this._iConfiguration.GetConnectionString("UserServiceConnection")))
                {
                    using (var transaction = connection.BeginTransaction(this._iCapPublisher, true))
                    {
                        //user.Name += "2021";
                        //this._UserServiceDbContext.SaveChanges();
                        _iCapPublisher.Publish(PublishName, user, dicHeader);//带header
                    }
                }
                this._Logger.LogWarning($"This is AdoTransaction Invoke");
                return Ok();
            }

            [Route("/efcoretransaction/async")]//根目录
            public IActionResult EFCoreTransaction()
            {
                var user = this._UserServiceDbContext.User.Find(1);//读个数据
                var userNew = new User()
                {
                    Name = "Eleven" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"),
                    CompanyId = 1,
                    CompanyName = "朝夕教育",
                    CreateTime = DateTime.Now,
                    CreatorId = 1,
                    LastLoginTime = DateTime.Now,
                    LastModifierId = 1,
                    LastModifyTime = DateTime.Now,
                    Password = "123456",
                    State = 1,
                    Account = "Administrator",
                    Email = "57265177@qq.com",
                    Mobile = "18664876677",
                    UserType = 1
                };//new个对象

                IDictionary<string, string> dicHeader = new Dictionary<string, string>();
                dicHeader.Add("Teacher", "Eleven");
                dicHeader.Add("Student", "Seven");
                dicHeader.Add("Version", "1.2");
                //完成 业务+publish的本地事务
                using (var trans = this._UserServiceDbContext.Database.BeginTransaction(this._iCapPublisher, autoCommit: false))
                {
                    this._UserServiceDbContext.User.Add(userNew);//数据库插入对象
                    this._UserServiceDbContext.SaveChanges();//提交---Context事务的

                    _iCapPublisher.Publish(PublishName, user, dicHeader);//带header
                                                                         //publish做的就只是把数据写入到publish表

                    //throw new Exception();就都写不进去了

                    Console.WriteLine("数据库业务数据已经插入");
                    trans.Commit();
                }
                this._Logger.LogWarning($"This is EFCoreTransaction Invoke");
                return Ok("Done");
            }

            #region 多节点贯穿协作
            [Route("/Distributed/Demo/{id}")]//根目录
            public IActionResult Distributed(int? id)
            {
                int index = id ?? 11;
                string publishName = "RabbitMQ.SQLServer.DistributedDemo.User-Order";

                var user = this._UserServiceDbContext.User.Find(1);
                var userNew = new User()
                {
                    Name = "Eleven" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"),
                    CompanyId = 1,
                    CompanyName = "朝夕教育" + index,
                    CreateTime = DateTime.Now,
                    CreatorId = 1,
                    LastLoginTime = DateTime.Now,
                    LastModifierId = 1,
                    LastModifyTime = DateTime.Now,
                    Password = "123456" + index,
                    State = 1,
                    Account = "Administrator" + index,
                    Email = "57265177@qq.com",
                    Mobile = "18664876677",
                    UserType = 1
                };

                IDictionary<string, string> dicHeader = new Dictionary<string, string>();
                dicHeader.Add("Teacher", "Eleven");
                dicHeader.Add("Student", "Seven");
                dicHeader.Add("Version", "1.2");
                dicHeader.Add("Index", index.ToString());

                using (var trans = this._UserServiceDbContext.Database.BeginTransaction(this._iCapPublisher, autoCommit: false))
                {
                    this._UserServiceDbContext.User.Add(userNew);
                    this._iCapPublisher.Publish(publishName, user, dicHeader);//带header
                    this._UserServiceDbContext.SaveChanges();
                    Console.WriteLine("数据库业务数据已经插入");
                    trans.Commit();
                }
                this._Logger.LogWarning($"This is EFCoreTransaction Invoke");
                return Ok("Done");
            }

            #endregion

        }
        #endregion

        #region OrderService
        public class OrderServiceController : ControllerBase
        {
            private readonly IConfiguration _iConfiguration;
            private readonly ICapPublisher _iCapPublisher;
            private readonly CommonServiceDbContext _UserServiceDbContext;
            private readonly ILogger<OrderServiceController> _Logger;
            public OrderServiceController(IConfiguration configuration, CommonServiceDbContext userServiceDbContext, ILogger<OrderServiceController> logger
                , ICapPublisher capPublisher
                )
            {
                this._iCapPublisher = capPublisher;
                this._iConfiguration = configuration;
                this._UserServiceDbContext = userServiceDbContext;
                this._Logger = logger;
            }

            #region 多节点贯穿协作
            [NonAction]
            [CapSubscribe("RabbitMQ.SQLServer.DistributedDemo.User-Order")]
            public void Distributed(User u, [FromCap] CapHeader header)
            {
                try
                {
                    Console.WriteLine($@"{DateTime.Now} Subscriber invoked, Info: {Newtonsoft.Json.JsonConvert.SerializeObject(u)}");
                    if (header != null)
                    {
                        Console.WriteLine("message header Teacher:" + header["Teacher"]);
                        Console.WriteLine("message header Student:" + header["Student"]);
                        Console.WriteLine("message header Version:" + header["Version"]);
                        Console.WriteLine("message header   Index:" + header["Index"]);
                    }

                    int index = int.Parse(header["Index"]);
                    index++;
                    string publishName = "RabbitMQ.SQLServer.DistributedDemo.Order-Storage";

                    var user = this._UserServiceDbContext.User.Find(1);
                    var userNew = new User()
                    {
                        Name = "Eleven" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"),
                        CompanyId = 1,
                        CompanyName = "朝夕教育" + index,
                        CreateTime = DateTime.Now,
                        CreatorId = 1,
                        LastLoginTime = DateTime.Now,
                        LastModifierId = 1,
                        LastModifyTime = DateTime.Now,
                        Password = "123456" + index,
                        State = 1,
                        Account = "Administrator" + index,
                        Email = "57265177@qq.com",
                        Mobile = "18664876677",
                        UserType = 1
                    };

                    IDictionary<string, string> dicHeader = new Dictionary<string, string>();
                    dicHeader.Add("Teacher", header["Teacher"]);
                    dicHeader.Add("Student", header["Student"]);
                    dicHeader.Add("Version", header["Version"]);
                    dicHeader.Add("Index", index.ToString());
                    using (var trans = this._UserServiceDbContext.Database.BeginTransaction(this._iCapPublisher, autoCommit: false))
                    {
                        this._UserServiceDbContext.User.Add(userNew);
                        this._iCapPublisher.Publish(publishName, user, dicHeader);//带header
                        this._UserServiceDbContext.SaveChanges();
                        Console.WriteLine("数据库业务数据已经插入");
                        trans.Commit();
                    }
                    this._Logger.LogWarning($"This is EFCoreTransaction Invoke");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("****************************************************");
                    Console.WriteLine(ex.Message);
                }
            }
            #endregion
        }
        #endregion

        #region LogisticsService
        public class LogisticsServiceController : ControllerBase
        {
            private readonly IConfiguration _iConfiguration;
            private readonly ICapPublisher _iCapPublisher;
            private readonly CommonServiceDbContext _UserServiceDbContext;
            private readonly ILogger<LogisticsServiceController> _Logger;
            public LogisticsServiceController(IConfiguration configuration, CommonServiceDbContext userServiceDbContext, ILogger<LogisticsServiceController> logger
                , ICapPublisher capPublisher
                )
            {
                this._iCapPublisher = capPublisher;
                this._iConfiguration = configuration;
                this._UserServiceDbContext = userServiceDbContext;
                this._Logger = logger;
            }

            #region 多节点贯穿协作
            [NonAction]
            [CapSubscribe("RabbitMQ.SQLServer.DistributedDemo.Storage-Logistics")]
            public void Distributed(User u, [FromCap] CapHeader header)
            {
                try
                {
                    Console.WriteLine($@"{DateTime.Now} Subscriber invoked, Info: {Newtonsoft.Json.JsonConvert.SerializeObject(u)}");
                    if (header != null)
                    {
                        Console.WriteLine("message header Teacher:" + header["Teacher"]);
                        Console.WriteLine("message header Student:" + header["Student"]);
                        Console.WriteLine("message header Version:" + header["Version"]);
                        Console.WriteLine("message header   Index:" + header["Index"]);
                    }

                    int index = int.Parse(header["Index"]);
                    index++;
                    string publishName = "RabbitMQ.SQLServer.DistributedDemo.Logistics-Payment";

                    var user = this._UserServiceDbContext.User.Find(1);
                    var userNew = new User()
                    {
                        Name = "Eleven" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"),
                        CompanyId = 1,
                        CompanyName = "朝夕教育" + index,
                        CreateTime = DateTime.Now,
                        CreatorId = 1,
                        LastLoginTime = DateTime.Now,
                        LastModifierId = 1,
                        LastModifyTime = DateTime.Now,
                        Password = "123456" + index,
                        State = 1,
                        Account = "Administrator" + index,
                        Email = "57265177@qq.com",
                        Mobile = "18664876677",
                        UserType = 1
                    };

                    IDictionary<string, string> dicHeader = new Dictionary<string, string>();
                    dicHeader.Add("Teacher", header["Teacher"]);
                    dicHeader.Add("Student", header["Student"]);
                    dicHeader.Add("Version", header["Version"]);
                    dicHeader.Add("Index", index.ToString());
                    using (var trans = this._UserServiceDbContext.Database.BeginTransaction(this._iCapPublisher, autoCommit: false))
                    {
                        this._UserServiceDbContext.User.Add(userNew);
                        this._iCapPublisher.Publish(publishName, user, dicHeader);//带header
                        this._UserServiceDbContext.SaveChanges();
                        Console.WriteLine("数据库业务数据已经插入");
                        trans.Commit();
                    }
                    this._Logger.LogWarning($"This is EFCoreTransaction Invoke");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("****************************************************");
                    Console.WriteLine(ex.Message);
                }
            }
            #endregion
        }
        #endregion

        #region PaymentService
        public class PaymentController : ControllerBase
        {
            private static string PublishName = "RabbitMQ.SQLServer.PaymentService1";
            private readonly IMongoClient _iMongoClient;
            private readonly IConfiguration _iConfiguration;
            private readonly ICapPublisher _iCapPublisher;
            private readonly CommonServiceDbContext _UserServiceDbContext;
            private readonly ILogger<PaymentController> _Logger;
            public PaymentController(IMongoClient mongoClient, ICapPublisher capPublisher, IConfiguration configuration, CommonServiceDbContext userServiceDbContext, ILogger<PaymentController> logger)
            {
                this._iMongoClient = mongoClient;
                this._iCapPublisher = capPublisher;
                this._iConfiguration = configuration;
                this._UserServiceDbContext = userServiceDbContext;
                this._Logger = logger;

                //注意：MongoDB 不能在事务中创建数据库和集合，所以你需要单独创建它们，模拟一条记录插入则会自动创建
                var mycollection = _iMongoClient.GetDatabase("test1").GetCollection<BsonDocument>("test1.collection1");
                mycollection.InsertOne(new BsonDocument { { "test", "test" } });
            }

            //[NonAction]
            //[CapSubscribe("RabbitMQ.SQLServer.PaymentService")]
            //public void Subscriber(User u)
            //{
            //    Console.WriteLine($@"{DateTime.Now} Subscriber invoked, Info: {u}");
            //    //throw new Exception("Subscriber failed custom!!!!!");
            //}

            [NonAction]
            [CapSubscribe("RabbitMQ.SQLServer.PaymentService")]
            public void Subscriber2(User u, [FromCap] CapHeader header)
            {
                Console.WriteLine($@"{DateTime.Now} Subscriber invoked, Info: {u}");

                #region bussiness+发布
                {
                    var userNew = new User()
                    {
                        Name = "Eleven" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"),
                        CompanyId = 1,
                        CompanyName = "朝夕教育",
                        CreateTime = DateTime.Now,
                        CreatorId = 1,
                        LastLoginTime = DateTime.Now,
                        LastModifierId = 1,
                        LastModifyTime = DateTime.Now,
                        Password = "123456",
                        State = 1,
                        Account = "Administrator",
                        Email = "57265177@qq.com",
                        Mobile = "18664876677",
                        UserType = 1
                    };

                    using (var session = this._iMongoClient.StartTransaction(this._iCapPublisher, autoCommit: false))
                    {
                        var collection = this._iMongoClient.GetDatabase("test1").GetCollection<BsonDocument>("test1.collection1");
                        collection.InsertOne(session, new BsonDocument { { "hello", "world" } });
                        Console.WriteLine("写入MongoDB数据");

                        this._iCapPublisher.Publish(PublishName, userNew);
                        session.CommitTransaction();
                    }

                    this._Logger.LogWarning($"This is Subscriber2 Invoke");
                }
                #endregion
            }

            //[NonAction]
            //[CapSubscribe("RabbitMQ.SQLServer.PaymentService", Group = "PaymentService.Group.Queue3")]
            //public void Subscriber3(User u, [FromCap] CapHeader header)
            //{
            //    Console.WriteLine($@"{DateTime.Now} Subscriber3 invoked, Info: {u}");


            //    //#region bussiness+发布
            //    //{
            //    //    using (var session = this._iMongoClient.StartTransaction(this._iCapPublisher, autoCommit: false))
            //    //    {
            //    //        var collection = this._iMongoClient.GetDatabase("test").GetCollection<BsonDocument>("test.collection");
            //    //        collection.InsertOne(session, new BsonDocument { { "hello", "world" } });
            //    //        Console.WriteLine("只写入MongoDB数据，不做再次publish");

            //    //        session.CommitTransaction();
            //    //    }
            //    //    this._Logger.LogWarning($"This is Subscriber2 Invoke");
            //    //}
            //    //#endregion


            //}
        }
        #endregion

        #region StorageService
        public class StorageServiceController : ControllerBase
        {
            private readonly IConfiguration _iConfiguration;
            private readonly ICapPublisher _iCapPublisher;
            private readonly CommonServiceDbContext _UserServiceDbContext;
            private readonly ILogger<StorageServiceController> _Logger;
            public StorageServiceController(IConfiguration configuration, CommonServiceDbContext userServiceDbContext, ILogger<StorageServiceController> logger
                , ICapPublisher capPublisher
                )
            {
                this._iCapPublisher = capPublisher;
                this._iConfiguration = configuration;
                this._UserServiceDbContext = userServiceDbContext;
                this._Logger = logger;
            }

            #region 多节点贯穿协作
            [NonAction]
            [CapSubscribe("RabbitMQ.SQLServer.DistributedDemo.Order-Storage")]
            public void Distributed(User u, [FromCap] CapHeader header)
            {
                try
                {
                    Console.WriteLine($@"{DateTime.Now} Subscriber invoked, Info: {Newtonsoft.Json.JsonConvert.SerializeObject(u)}");
                    if (header != null)
                    {
                        Console.WriteLine("message header Teacher:" + header["Teacher"]);
                        Console.WriteLine("message header Student:" + header["Student"]);
                        Console.WriteLine("message header Version:" + header["Version"]);
                        Console.WriteLine("message header   Index:" + header["Index"]);
                    }

                    int index = int.Parse(header["Index"]);
                    index++;
                    string publishName = "RabbitMQ.SQLServer.DistributedDemo.Storage-Logistics";

                    var user = this._UserServiceDbContext.User.Find(1);
                    var userNew = new User()
                    {
                        Name = "Eleven" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"),
                        CompanyId = 1,
                        CompanyName = "朝夕教育" + index,
                        CreateTime = DateTime.Now,
                        CreatorId = 1,
                        LastLoginTime = DateTime.Now,
                        LastModifierId = 1,
                        LastModifyTime = DateTime.Now,
                        Password = "123456" + index,
                        State = 1,
                        Account = "Administrator" + index,
                        Email = "57265177@qq.com",
                        Mobile = "18664876677",
                        UserType = 1
                    };

                    IDictionary<string, string> dicHeader = new Dictionary<string, string>();
                    dicHeader.Add("Teacher", header["Teacher"]);
                    dicHeader.Add("Student", header["Student"]);
                    dicHeader.Add("Version", header["Version"]);
                    dicHeader.Add("Index", index.ToString());
                    using (var trans = this._UserServiceDbContext.Database.BeginTransaction(this._iCapPublisher, autoCommit: false))
                    {
                        this._UserServiceDbContext.User.Add(userNew);
                        this._iCapPublisher.Publish(publishName, user, dicHeader);//带header
                        this._UserServiceDbContext.SaveChanges();
                        Console.WriteLine("数据库业务数据已经插入");
                        trans.Commit();
                    }
                    this._Logger.LogWarning($"This is EFCoreTransaction Invoke");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("****************************************************");
                    Console.WriteLine(ex.Message);
                }
            }
            #endregion
        }

        public class StorageController : ControllerBase
        {
            private static string PublishName = "RabbitMQ.SQLServer.StorageService";

            private readonly IConfiguration _iConfiguration;
            private readonly ICapPublisher _iCapPublisher;
            private readonly CommonServiceDbContext _UserServiceDbContext;
            private readonly ILogger<StorageController> _Logger;
            public StorageController(ICapPublisher capPublisher, IConfiguration configuration, CommonServiceDbContext userServiceDbContext, ILogger<StorageController> logger)
            {
                this._iCapPublisher = capPublisher;
                this._iConfiguration = configuration;
                this._UserServiceDbContext = userServiceDbContext;
                this._Logger = logger;
            }


            [NonAction]
            [CapSubscribe("RabbitMQ.SQLServer.UserService")]
            public void Subscriber(User u)
            {
                Console.WriteLine($@"{DateTime.Now} Subscriber invoked, Info: {u}");
                throw new Exception("Subscriber failed custom!!!!!");
            }

            [NonAction]
            [CapSubscribe("RabbitMQ.SQLServer.UserService", Group = "StorageService.Group.Queue2")]
            public void Subscriber2(User u, [FromCap] CapHeader header)
            {
                Console.WriteLine($@"{DateTime.Now} Subscriber invoked, Info: {u}");
                #region bussiness+发布
                {
                    var user = this._UserServiceDbContext.User.Find(1);
                    var userNew = new User()
                    {
                        Name = "Eleven" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"),
                        CompanyId = 1,
                        CompanyName = "朝夕教育",
                        CreateTime = DateTime.Now,
                        CreatorId = 1,
                        LastLoginTime = DateTime.Now,
                        LastModifierId = 1,
                        LastModifyTime = DateTime.Now,
                        Password = "123456",
                        State = 1,
                        Account = "Administrator",
                        Email = "57265177@qq.com",
                        Mobile = "18664876677",
                        UserType = 1
                    };

                    IDictionary<string, string> dicHeader = new Dictionary<string, string>();
                    dicHeader.Add("Teacher", "Eleven");
                    dicHeader.Add("Student", "Seven");
                    dicHeader.Add("Version", "1.2");

                    using (var trans = this._UserServiceDbContext.Database.BeginTransaction(this._iCapPublisher, autoCommit: false))
                    {
                        this._UserServiceDbContext.User.Add(userNew);

                        _iCapPublisher.Publish(PublishName, user, dicHeader);//带header

                        this._UserServiceDbContext.SaveChanges();
                        Console.WriteLine("数据库业务数据已经插入");
                        trans.Commit();
                    }
                    this._Logger.LogWarning($"This is EFCoreTransaction Invoke");
                }
                #endregion
            }

            [NonAction]
            [CapSubscribe("RabbitMQ.SQLServer.UserService", Group = "StorageService.Group.Queue3")]
            public void Subscriber3(User u, [FromCap] CapHeader header)
            {
                Console.WriteLine($@"{DateTime.Now} Subscriber invoked, Info: {u}");
                Console.WriteLine("message header Teacher:" + header["Teacher"]);
                Console.WriteLine("message header Student:" + header["Student"]);
                Console.WriteLine("message header Version:" + header["Version"]);

                #region bussiness
                {
                    var user = this._UserServiceDbContext.User.Find(1);
                    var userNew = new User()
                    {
                        Name = "Eleven" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"),
                        CompanyId = 1,
                        CompanyName = "朝夕教育",
                        CreateTime = DateTime.Now,
                        CreatorId = 1,
                        LastLoginTime = DateTime.Now,
                        LastModifierId = 1,
                        LastModifyTime = DateTime.Now,
                        Password = "123456",
                        State = 1,
                        Account = "Administrator",
                        Email = "57265177@qq.com",
                        Mobile = "18664876677",
                        UserType = 1
                    };
                    this._UserServiceDbContext.User.Add(userNew);
                    if (new Random().Next(2019, 2021) == 2020)
                        throw new Exception("Just Exception!");
                    this._UserServiceDbContext.SaveChanges();
                    Console.WriteLine("数据库业务数据已经插入");
                }
                #endregion
            }
        }
        #endregion
    }
}
