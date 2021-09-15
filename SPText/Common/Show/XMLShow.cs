using IOSerialize.Serialize;
using SPTextCommon;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SPText.Common.Show
{
    public class XMLShow
    {
        public void Show()
        {
            //xmlOperation1();
            //xmlSerialize();
            //xmlDeserialize();
            //xmlOperation();
            //FileOperation();
            readConfigSet();
        }

        public void readConfigSet()
        {
            string localPath = AppDomain.CurrentDomain.BaseDirectory;
            string readySetPath = Path.Combine(localPath, @"config\set.xml");

            ConfigSet configSet = new ConfigSet();
            //using (var fs = new FileStream(readySetPath, FileMode.OpenOrCreate))
            //{
            //    using (var sr = new StreamWriter(fs, System.Text.Encoding.UTF8))
            //    {
            //        var configText = ODEDIXML.Common.XmlConvert.XmlSerializer(configSet.set);
            //        sr.Write(configText);
            //    }
            //}

            if (File.Exists(readySetPath))
            {
                using (var fs = new FileStream(readySetPath, FileMode.Open))
                {
                    using (var sr = new StreamReader(fs))
                    {
                        configSet.set = XmlConvert.XmlDeserializer<Set>(sr.ReadToEnd(), System.Text.Encoding.UTF8);
                    }
                }
            }
        }

        #region  xml数据操作

        public static void xmlSerialize()
        {
            Book b1 = new Book("111", "书1");
            Book b2 = new Book("222", "书2");
            Book b3 = new Book("333", "书3");
            Books bs1 = new Books();
            Books bs2 = new Books();
            bs1.BookList.Add(b1);
            bs1.BookList.Add(b2);
            bs2.BookList.Add(b3);
            Person p1 = new Person("张三", 11);
            Person p2 = new Person("李四", 22);
            p1.BookList.Add(bs1);
            p2.BookList.Add(bs2);
            BaseInfo baseInfo = new BaseInfo();
            baseInfo.PersonList.Add(p1);
            baseInfo.PersonList.Add(p2);

            string path = GetFilePath();
            using (var fs = new FileStream(path, FileMode.Create))
            {
                using (var sr = new StreamWriter(fs, System.Text.Encoding.UTF8))
                {
                    var configText = XmlConvert.XmlSerializer<BaseInfo>(baseInfo);
                    sr.Write(configText);
                }
            }
        }

        public static void xmlDeserialize()
        {
            var info = new BaseInfo();
            string path = GetFilePath();
            #region  读取节点
            using (var fs = new FileStream(path, FileMode.Open))
            {
                using (var sr = new StreamReader(fs))
                {
                    info = XmlConvert.XmlDeserializer<BaseInfo>(sr.ReadToEnd(), System.Text.Encoding.UTF8);
                }
            }
            #endregion
            foreach (Person per in info.PersonList)
            {
                Console.WriteLine("人员：");
                Console.WriteLine(" 姓名：" + per.Name);
                Console.WriteLine(" 年龄：" + per.Age);
                foreach (Books b1 in per.BookList)
                {
                    foreach (Book b in b1.BookList)
                    {
                        Console.WriteLine(" 书：");
                        Console.WriteLine("     ISBN:" + b.ISBN);
                        Console.WriteLine("     书名:" + b.Title);
                    }
                }
            }

            info.PersonList[0].Age = 99;
            #region  储存节点
            using (var fs = new FileStream(path, FileMode.Create))
            {
                using (var sr = new StreamWriter(fs, System.Text.Encoding.UTF8))
                {
                    var configText = XmlConvert.XmlSerializer<BaseInfo>(info);
                    sr.Write(configText);
                }
            }
            #endregion
        }

        public static void xmlOperation()
        {
            string path = GetFilePath();
            string vakue = SPTextCommon.XmlHelper.Read(path, "/BaseInfo/Person[@Person='1']/Name");
        }

        public static string GetFilePath(string pathName = "xmlBaseInfo.xml")
        {
            string baseDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo di = new DirectoryInfo(string.Format(@"{0}..\..\", baseDirectory));
            string path = Path.Combine(di.FullName, pathName);
            return path;
        }

        //读取文件/存储文件
        public static void FileOperation()
        {
            Book b1 = new Book("111", "书1");
            Book b2 = new Book("222", "书2");
            Book b3 = new Book("333", "书3");
            Books bs1 = new Books();
            Books bs2 = new Books();
            bs1.BookList.Add(b1);
            bs1.BookList.Add(b2);
            bs2.BookList.Add(b3);
            Person p1 = new Person("张三", 11);
            Person p2 = new Person("李四", 22);
            p1.BookList.Add(bs1);
            p2.BookList.Add(bs2);
            BaseInfo baseInfo = new BaseInfo();
            baseInfo.PersonList.Add(p1);
            baseInfo.PersonList.Add(p2);

            string path1 = GetFilePath("123");
            SPTextCommon.XmlHelper.SetFile(path1, JsonHelper.SerializeObject(baseInfo));
            var returnFile = JsonHelper.DeserializeObject<BaseInfo>(XmlHelper.GetFile(path1));
        }

        public static void xmlOperation1()
        {
            var xml = string.Empty;
            {
                xmlStudent stu1 = new xmlStudent() { Name = "okbase", Age = 10 };
                xml = XmlConvert.Serializer(typeof(xmlStudent), stu1);
                Console.Write(xml);
            }
            {
                xmlStudent stu2 = XmlConvert.Deserialize(typeof(xmlStudent), xml) as xmlStudent;
                Console.Write(string.Format("名字:{0},年龄:{1}", stu2.Name, stu2.Age));
            }
            {
                // 生成DataTable对象用于测试
                System.Data.DataTable dt1 = new System.Data.DataTable("mytable");   // 必须指明DataTable名称

                dt1.Columns.Add("Dosage", typeof(int));
                dt1.Columns.Add("Drug", typeof(string));
                dt1.Columns.Add("Patient", typeof(string));
                dt1.Columns.Add("Date", typeof(DateTime));

                // 添加行
                dt1.Rows.Add(25, "Indocin", "David", DateTime.Now);
                dt1.Rows.Add(50, "Enebrel", "Sam", DateTime.Now);
                dt1.Rows.Add(10, "Hydralazine", "Christoff", DateTime.Now);
                dt1.Rows.Add(21, "Combivent", "Janet", DateTime.Now);
                dt1.Rows.Add(100, "Dilantin", "Melanie", DateTime.Now);

                // 序列化
                xml = XmlConvert.Serializer(typeof(System.Data.DataTable), dt1);
                Console.Write(xml);
            }
            {
                // 反序列化
                System.Data.DataTable dt2 = XmlConvert.Deserialize(typeof(System.Data.DataTable), xml) as System.Data.DataTable;

                // 输出测试结果
                foreach (DataRow dr in dt2.Rows)
                {
                    foreach (DataColumn col in dt2.Columns)
                    {
                        Console.Write(dr[col].ToString() + " ");
                    }

                    Console.Write("\r\n");
                }
            }
            {
                // 生成List对象用于测试
                List<xmlStudent> list1 = new List<xmlStudent>(3);

                list1.Add(new xmlStudent() { Name = "okbase", Age = 10 });
                list1.Add(new xmlStudent() { Name = "csdn", Age = 15 });
                // 序列化
                xml = XmlConvert.Serializer(typeof(List<xmlStudent>), list1);
                Console.Write(xml);
            }
            {
                List<xmlStudent> list2 = XmlConvert.Deserialize(typeof(List<xmlStudent>), xml) as List<xmlStudent>;
                foreach (xmlStudent stu in list2)
                {
                    Console.WriteLine(stu.Name + "," + stu.Age.ToString());
                }
            }
        }

        #endregion

    }

    #region  xml类
    [Serializable]
    public class BaseInfo
    {
        List<Person> perList = new List<Person>();

        [XmlElement(ElementName = "Person")]
        public List<Person> PersonList
        {
            get { return perList; }
            set { perList = value; }
        }
    }
    public class Person
    {
        string name;
        int age;
        List<Books> bookList = new List<Books>();

        /// <summary>
        /// 必须有默认的构造函数
        /// </summary>
        public Person()
        { }

        public Person(string name, int age)
        {
            this.name = name;
            this.age = age;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Age
        {
            get { return age; }
            set { age = value; }
        }

        [XmlElement(ElementName = "Books")]
        public List<Books> BookList
        {
            get { return bookList; }
            set { bookList = value; }
        }
    }
    public class Books
    {
        List<Book> bookList = new List<Book>();

        [XmlElement(ElementName = "Book")]
        public List<Book> BookList
        {
            get { return bookList; }
            set { bookList = value; }
        }
    }

    public class Book
    {
        string isbn;
        string title;

        public Book() { }

        public Book(string isbn, string title)
        {
            this.isbn = isbn;
            this.title = title;
        }

        public string ISBN
        {
            get { return isbn; }
            set { isbn = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }
    }
    #endregion


    #region  Set
    public class ConfigSet
    {
        public Set set { get; set; }
    }
    [XmlType(TypeName = "set")]
    public class Set
    {
        public Customer[] customers { get; set; }
    }
    [XmlType(TypeName = "customer")]
    public class Customer
    {
        public DatabaseSet[] databaseSets { get; set; }
        public SendEmailSet[] sendEmailSets { get; set; }
        public FtpSet[] ftpSets { get; set; }
    }
    #region databaseSet
    [XmlType(TypeName = "databaseSet")]
    public class DatabaseSet
    {
        [XmlAttribute(AttributeName = "customerName")]
        public string customerName { get; set; }
        public string dataSource { get; set; }
        public string database { get; set; }
        public string user { get; set; }
        public string pwd { get; set; }
    }
    #endregion

    #region  sendEmailSet
    [XmlType(TypeName = "sendEmailSet")]
    public class SendEmailSet
    {
        [XmlAttribute(AttributeName = "customerName")]
        public string customerName { get; set; }
        public string smtpHost { get; set; }
        public string smtpPort { get; set; }
        public string emailAccount { get; set; }
        public string emailPassword { get; set; }
        public string emailRecipients { get; set; }
        public string ccRecipients { get; set; }
        public string lastEmailTime { get; set; }
        public string sendEmailCount { get; set; }
    }
    #endregion

    #region  ftpSet
    [XmlType(TypeName = "ftpSet")]
    public class FtpSet
    {
        [XmlAttribute(AttributeName = "customerName")]
        public string customerName { get; set; }
        public string ftpMode { get; set; }
        public string ip { get; set; }
        public string port { get; set; }
        public string user { get; set; }
        public string pwd { get; set; }
    }
    #endregion
    #endregion
}
