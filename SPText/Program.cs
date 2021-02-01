using CLSLibrary;
using Dapper;
using InfoEarthFrame.Data;
using IOSerialize.IO;
using IOSerialize.Serialize;
using log4net.Config;
using Microsoft.Graph;
using Microsoft.Office.Interop.Word;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ServiceStack;
using ServiceStack.DataAnnotations;
using ServiceStack.Redis;
using SPText.Common;
using SPText.Common.DataHelper;
using SPText.Common.DataHelper.Sql;
using SPText.Common.ExpressionExtend;
using SPText.Common.Redis;
using SPText.Common.Redis.Service;
using SPText.EF;
using SPText.EF.EF2;
using SPText.Unity;
using SPText.Unity.Aop;
using SPTextCommon;
using SPTextCommon.CacheRedis;
using SPTextCommon.EFBaseServices;
using SPTextCommon.EFBaseServices.Model;
using SPTextLK.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;
using Unity;
using Unity.Lifetime;
using static Microsoft.Graph.CoreConstants.MimeTypeNames;


namespace SPText
{
    class Program
    {
        private static int[] dataArr = new int[100];
        static string connectionStrings = ConfigurationManager.ConnectionStrings["DataContext"].ToString();
        static void Main(string[] args)
        {
            #region  linq交叉并补
            //linqUse();
            #endregion

            #region  委托
            //CreateDelegate();
            #endregion

            #region  常见算法
            //sf();
            //int[] intArray = new int[] { 2, 5, 4, 1, 3 };
            //QuickSort(intArray, 0, intArray.Length - 1);
            #endregion

            #region  泛型
            //for (int i = 0; i < 5; i++)
            //{
            //    Console.WriteLine(GenericCache<int>.GetCache());
            //    Thread.Sleep(10);
            //    Console.WriteLine(GenericCache<long>.GetCache());
            //    Thread.Sleep(10);
            //    Console.WriteLine(GenericCache<DateTime>.GetCache());
            //    Thread.Sleep(10);
            //    Console.WriteLine(GenericCache<string>.GetCache());
            //    Thread.Sleep(10);
            //    //Console.WriteLine(GenericCache<Program>.GetCache());
            //    Thread.Sleep(10);

            //    Console.WriteLine("123");
            //}
            #endregion

            #region  反射
            //AssemblyActivator();
            #endregion

            #region  日志
            //InitLog4Net();

            //var logger = LogManager.GetLogger(typeof(Program));

            //logger.Info("消息");
            //logger.Warn("警告");
            //logger.Error("异常");
            //logger.Fatal("错误");
            #endregion

            #region  多线程 Thread、ThreadPool、Task、Parallel
            //ThreadDemo();
            #endregion

            #region  IOC
            //IOCShow();
            #endregion

            #region  设计模式
            //DesignPattern();
            #endregion

            #region  xml
            //xmlOperation1();
            //xmlSerialize();
            //xmlDeserialize();
            //xmlOperation();
            //FileOperation();
            //LoginUser.UseName = "123";//错误！未实现
            #endregion

            #region Unity
            //IUnityContainer container = Unity.ContainnerFactory.GetContainer();
            //IText iCompanyUserService = container.Resolve<IText>();
            #endregion

            #region  Lambda
            //lambdaOperation();
            #endregion

            #region  设计模式六大原则
            //单一原则
            //里式替换原则
            //迪米特原则
            //面向抽象原则
            //接口隔离原则
            //开闭原则

            #endregion

            #region  表达式目录树
            //Expression<Action> expression = () => Console.WriteLine("123");
            //Expression<Action<int,int>> expression1 = (x,y) => Console.WriteLine($"{x}{y}123");
            //expression1.Compile().Invoke(1, 2);

            //Expression<Func<Text, bool>> textc = c => c.Id == 1 && c.UseName.ToString() == "123";

            //public bool ExpressionText<T>(Expression<Func<T, bool>> textc)
            //{
            //    Type type = typeof(T);
            //    //ExpressionVisitor conditionBuilderVisitor = new ExpressionVisitor();
            //    //conditionBuilderVisitor.Visit(textc);
            //    //string result = conditionBuilderVisitor.Condition();
            //    return true;
            //}
            #endregion

            #region  IO(序列化&反序列化、读取文件信息)
            //JsonAndFile();
            #endregion

            #region  数据类型/特殊类型
            //DataTypeAndSpecialType();
            #endregion

            #region  （nosql）Redis
            //RedisShow();
            #endregion

            #region  NPOI
            //NOPIHelper.Show();
            //NOPIHelper.show2();
            #endregion

            #region  比较和计时
            //object.ReferenceEquals(1,1);//用于比较


            //var texts0 = new List<Text>().AsQueryable().Where(p => p.Id > 100 && p.UseName.Contains("张三"));
            //var texts1 = TextWhere(new List<Text>(), p => p.Id > 100 && p.UseName.Contains("张三"));

            //{
            //    long commonSecond = 0;
            //    Stopwatch watch = new Stopwatch();
            //    watch.Start();
            //    for (int i = 0; i < 100_000_000; i++)
            //    {
            //        Thread.Sleep(100);
            //    }
            //    watch.Stop();
            //    commonSecond = watch.ElapsedMilliseconds;
            //}
            #endregion

            #region  数据库操作
            //DatabaseOperations();
            #endregion

            #region  二维码
            //GetQRCode();
            #endregion

            #region  SFtp
            //SharpSSH();
            #endregion

            #region  加密
            //Encrypt();
            #endregion

            #region  Aop(面向切面编程)
            //Aop();
            #endregion

            #region  值储存方式（DataTable，Hashtable，Dictionary，List）
            //EnumerableData();
            #endregion

            #region  加密
            //Encrypt();
            #endregion

            #region  爬虫
            //Crawler();
            #endregion

            #region  定时调度(有错误)
            //Quartz();
            #endregion

            #region  WebSocket
            //WebSocket();
            #endregion

            #region  RabbitMQ
            //RabbitMQ();
            #endregion

            #region  文件压缩
            //ZipShow();
            #endregion

            #region  测试代码
            TestHelper testHelper = new TestHelper();
            testHelper.Show();
            #endregion




            //dynamic  避开编译器检查
            Console.ReadKey();

            #region  
            //string aa = @"div+css、layui、vue、bootstrap、jQuery、ado.net、ef、wcf、api、linq、xml、orm、ef、ioc、NoSql、WebSocket、委托、特性、泛型、数组、反射、多线程、爬虫、.Net Core、微服务";
            //1.分布式（应用程序、文件、数据库）
            //2.缓存
            //3.集群
            //3.消息队列
            //4.延迟加载
            #endregion
        }


        #region  linq交叉并补
        public static void linqUse()
        {
            int[] a = { 1, 2, 3, 4, 5, 6, 7 };
            int[] b = { 4, 5, 6, 7, 8, 9, 10 };
            int[] c = { 1, 2, 3, 3, 4, 1, 2, 4, 6, 1, 6, 5 };
            // 交集
            var fuck = a.Intersect(b);
            // 并集
            var shit = a.Union(b);
            // a有b没有的
            var diff1 = a.Except(b);
            // b有a没有的
            var diff2 = b.Except(a);
            var max = a.Max();
            var min = a.Min();
            var avg = a.Average();
            var dis = c.Distinct();


            var a1 = a.Join(b, p => p.ToString(), q => q.ToString(), (p, q) => p).ToList();//连接
            var a2 = a.Zip(c, (p, q) => p + q).ToList();//合并
            //var a3 = a.ToLookup(p => p == 1).ToList();
            var a4 = c.Concat(a); //连接



            Console.WriteLine(max);
            Console.WriteLine(min);
            Console.WriteLine(avg);

            List<string> strList = new List<string>();
            strList = strList.Where((p, i) => strList.FindIndex(m => m.ToString() == p.ToString()) == i).ToList();//自定义去重（未验证）
            Console.ReadKey();
        }
        #endregion

        #region  委托
        public static void CreateDelegate()
        {
            DYDeleGate deleGate = new DYDeleGate(insert);
            int ret = deleGate.Invoke(10, 20);

            Console.WriteLine("10+20={0}", ret);

            deleGate -= insert;
            deleGate += delete;

            ret = deleGate.Invoke(10, 20);

            Console.WriteLine("10-20={0}", ret);

            Console.ReadLine();
            Action action = () => Console.WriteLine("这是一个无参无返回值的委托");
            Func<int, int, string> func = (int i, int j) => $"{i}{j}这是一个有参返回值为字符串的委托";
            string funcReturn = func.Invoke(1, 2);


            CreateDelete createDelete = new CreateDelete();
            createDelete.ActionText += createDelete.ShowText1;
            createDelete.ActionText += createDelete.ShowText2;
            createDelete.ActionText += createDelete.ShowText3;
            createDelete.ShowText();
        }
        #endregion

        #region  常见算法
        public static void sf()
        {
            cjsf();
            dysjx();
            suijicharu();
            mppx(dataArr.ToArray());
            QuickSort(dataArr.ToArray(), 0, 10);
            daoxu();
            Combine(dataArr.ToArray());
            jiecheng(10);
            qh();
            Foo(10);
            jisuanzhishu();
            qssz(dataArr.ToArray());
            cf(dataArr.ToArray());
            MaxAndMin(dataArr.ToArray());
            gys(8, 64);

            AlgorithmHelper.Show0();
            AlgorithmHelper.Show1();
            AlgorithmHelper.Show2();
            AlgorithmHelper.Show4();
            AlgorithmHelper.Show5();
        }


        // 9*9乘法表
        public static void cjsf()
        {
            for (int a = 0; a < 9; a++)
            {
                for (int b = 0; b <= a; b++)
                {
                    Console.Write($"{a}*{b}={a * b}\t");
                }
                Console.WriteLine();
            }
        }

        // 等腰三角形
        public static void dysjx()
        {
            for (int a = 0; a <= 5; a++)
            {
                for (int b = 1; b <= 5 - a; b++)
                {
                    Console.Write(" ");
                }
                for (int c = 1; c < 2 * a; c++)
                {
                    Console.Write("*");
                }
                Console.Write("\n");
            }
        }

        //随机数
        public static int[] suijicharu()
        {
            int[] arr = new int[100];
            ArrayList arrList = new ArrayList();
            Random rdm = new Random();
            while (arrList.Count < 100)
            {
                int num = rdm.Next(1, 101);
                if (!arrList.Contains(num))
                {
                    arrList.Add(num);
                }
            }
            for (int i = 0; i < 100; i++)
            {
                arr[i] = int.Parse(arrList[i].ToString());
            }
            dataArr = arr;
            return arr;
        }

        //冒泡排序
        public static int[] mppx(int[] data)
        {
            int temp = 0;

            for (int i = 0; i < data.Length - 1; i++)
            {
                for (int j = i + 1; j < data.Length; j++)
                {
                    if (data[i] > data[j])
                    {
                        temp = data[i];
                        data[i] = data[j];
                        data[j] = temp;
                    }
                }
            }
            return data;
        }

        //实现快速排序
        public static string QuickSort(int[] num, int left, int right)
        {
            string data = "";
            if (left >= right)
                return "";

            int key = num[left];
            int i = left;
            int j = right;

            while (i < j)
            {
                while (i < j && key < num[j])
                    j--;
                if (i >= j)
                {
                    num[i] = key;
                    break;
                }
                num[i] = num[j];

                while (i < j && key >= num[i])
                    i++;
                if (i >= j)
                {
                    num[i] = key;
                    break;
                }
                num[j] = num[i];
            }
            num[i] = key;

            QuickSort(num, left, i - 1);
            QuickSort(num, i + 1, right);

            for (int k = 0; k < num.Length; k++)
            {
                data += num[k] + ",";
            }

            return data;
        }

        //字符串倒叙
        public static string daoxu()
        {
            string data = "";
            string str = "I am a student";
            string[] strdata = str.Split(' ');
            for (int i = strdata.Length - 1; i >= 0; i--)
            {
                data += strdata[i];
                //Console.Write(strdata[i]);
                if (i != 0)
                {
                    data += " ";
                    //Console.Write(' ');
                }
            }

            return data;
        }

        //数组转字符串
        public static string Combine(int[] data)
        {
            string str = "";
            foreach (var i in data)
            {
                str = i.ToString() + ",";
            }

            return str;
        }

        //阶乘
        public static int jiecheng(int i)
        {
            if (i == 1)
            {
                return 1;
            }
            else if (i == 2)
            {
                return 2;
            }
            else
            {
                return i * (jiecheng(i - 1));
            }
        }

        #region  求和
        public static void qh()
        {
            // 2、得到结果
            int sums = Sums(10000, 4);

            Console.WriteLine("1++++++++10000求和：{0}", sums);
        }

        public static int Sums(int num, int threadCount)
        {
            int sum = 0;
            // 1、计算每一个线程计算的单位
            int threadUnit = num / threadCount; // 25 

            // 2、计算开始和结束(如果 100 / 6 = 16 余 3)
            //  1、能除尽
            //  2、不能除尽
            for (int i = 0; i <= threadCount; i++)
            {
                // i 最大 只能为5
                // 3.1 开始
                int startConut = threadUnit * i + 1; // 25 *0  + 1= 1; // 25 * 1 + 1 = 26

                // 3.2 结束
                int endCount = threadUnit * (i + 1); // 25 *(0+1) = 25; // 25 *(1+1) = 50

                // 3.3 除不尽的判断
                if (endCount > num)
                {
                    endCount = num;
                }

                // 3.5 计算结果
                sum += GetSum(startConut, endCount);
            }
            return sum;
        }

        public static int GetSum(int startCount, int endCount)
        {
            // 1、开始线程来计算
            Task<int> taskSum = System.Threading.Tasks.Task.Run<int>(() =>
            {
                int num = 0;
                for (int i = startCount; i <= endCount; i++)
                {
                    num += i;
                }

                return num;
            });

            // 2、获取计算结果
            int result = taskSum.Result;

            Console.WriteLine("{0} 获取结果：{1}", taskSum.Id, result);
            return result;
        }
        #endregion

        #region  递归
        public static int Foo(int i)
        {
            if (i == 0)
            {
                return 0;
            }
            else if (i > 1 && i <= 2)
            {
                return 1;
            }
            else
            {
                return Foo(i - 1) + Foo(i - 2);
            }
        }
        #endregion

        #region  计算输入的质数
        //计算输入的质数
        public static void jisuanzhishu()
        {
            long i;
            while (true)
            {
                Console.Write("请输入要计算的质数（0退出）：");
                i = long.Parse(Console.ReadLine());
                if (i == 0) break;
                DateTime t1 = DateTime.Now;
                switch (i)
                {
                    case 1: Console.WriteLine("1 不是质数！"); break;
                    case 2: Console.WriteLine("2 是质数！"); break;
                    default: cal(i); break;
                }
                DateTime t2 = DateTime.Now;
                Console.WriteLine("时间为：{0} 毫秒\n", (t2 - t1).Ticks / 10000f);
            }
        }

        //以下为函数部分 
        public static void cal(long x)
        {
            long sum = 1;
            byte row = 1;
            Console.Write("\n");
            for (long a = 3; a < x + 1; a++)
            {
                bool flag = true;
                for (long b = 2; b < (a / 2) + 1; b++)
                {
                    if (a % b != 0) continue;
                    flag = false;
                    break;
                }
                if (flag)
                {
                    if (row == 10)
                    {
                        Console.WriteLine();
                        row = 0;
                    }
                    if (sum == 1)
                        Console.Write("{0,7}", 2);
                    Console.Write("{0,7}", a);
                    sum++; row++;
                }
            }
            Console.WriteLine("\n\n{0} 以内共有 {1} 个质数\n", x, sum);
        }
        #endregion

        #region  求数组缺失的数
        public static void qssz(int[] nums)
        {
            int n = nums.Length;
            int sum = (n + 1) * (n + 2) / 2;
            int otherSum = 0;
            for (int i = 0; i < nums.Length; i++)
            {
                otherSum += nums[i];
            }
            int defextdata = sum - otherSum;
        }
        #endregion

        #region  给定的整型数组中的重复数字
        public static void cf(int[] nums)
        {
            Dictionary<int, int> keyValues = new Dictionary<int, int>();
            for (int i = 0; i < nums.Length; i++)
            {
                if (keyValues.Keys.Contains(nums[i]))
                {
                    keyValues[nums[i]]++;
                }
                else
                {
                    keyValues[nums[i]] = 1;
                }
            }

            int[] numArr = keyValues.Where(t => t.Value > 1).ToDictionary(t => t.Key, t => t.Value).Keys.ToArray();
        }
        #endregion

        #region  求数组最大值和最小值
        public static void MaxAndMin(int[] nums)
        {
            int Max = nums[0];
            int Min = nums[0];
            for (int i = 0; i < nums.Length; i++)
            {
                if (Max < nums[i])
                {
                    Max = nums[i];
                }
            }

            for (int i = 0; i < nums.Length; i++)
            {
                if (nums[Min] > nums[i])
                {
                    Min = nums[i];
                }
            }
        }
        #endregion

        #region  公约数
        public static void gys(int x, int y)
        {
            int min = x < y ? x : y;
            for (int i = 0; i < min - 1; i++)
            {
                if (x % i == 0 && y % i == 0)
                {
                    Console.WriteLine("{0}和{1}的最大公约数是：{2}", x, i, i);
                    break;

                }
            }
        }
        #endregion
        #endregion

        #region  泛型 Dictionary List T(自定义类型)
        //泛型方法
        public object obj<T>(T obj)
        {
            return obj;
        }

        //泛型类
        public class ObjClass<T>
        {
            public ObjClass()
            {
                Console.WriteLine("这是一个构造函数泛型类！");
                intvalue = 1;
            }

            private int intvalue;

            public int retValue()
            {
                return intvalue;
            }
        }

        //  泛型类
        public class GenericClass<T>
        {
            public T _T;
        }
        //泛型缓存
        public class GenericCache<T>
        {
            static GenericCache()
            {
                Console.WriteLine("This is GenericCache 静态构造函数");
                _TypeTime = string.Format("{0}_{1}", typeof(T).FullName, DateTime.Now.ToString("yyyyMMddHHmmss.fff"));
            }

            private static string _TypeTime = "";

            public static string GetCache()
            {
                return _TypeTime;
            }
        }

        #region  泛型
        //泛型方法
        public T VoidFangFa<T>()
        //where T : class//引用类型约束
        //where T : struct//值类型
        /*where T : new()*///无参数构造函数
                           //where T:Text//类约束
                           //where T:IText //接口约束
        {
            return default(T);
        }
        //泛型类
        public class ClassFangFa<T>
        {
        }
        //泛型接口
        public interface interfaceFangFa<T>
        {

        }
        //泛型委托
        public delegate void delegateFangFa<T>() where T : class;

        #region  协变/逆变
        public static void ShowGeneric()
        {
            {
                Bird bird1 = new Bird();
                Bird bird2 = new Sparrow();
                Sparrow sparrow1 = new Sparrow();
                //Sparrow sparrow2 = new Bird();
            }


            {
                List<Bird> birdList1 = new List<Bird>();
                //List<Bird> birdList2 = new List<Sparrow>();

                List<Bird> birdList3 = new List<Sparrow>().Select(c => (Bird)c).ToList();
            }



            {//协变
                IEnumerable<Bird> birdList1 = new List<Bird>();
                IEnumerable<Bird> birdList2 = new List<Sparrow>();

                Func<Bird> func = new Func<Sparrow>(() => null);

                ICustomerListOut<Bird> customerList1 = new CustomerListOut<Bird>();
                ICustomerListOut<Bird> customerList2 = new CustomerListOut<Sparrow>();
            }



            {//逆变
                ICustomerListIn<Sparrow> customerList2 = new CustomerListIn<Sparrow>();
                ICustomerListIn<Sparrow> customerList1 = new CustomerListIn<Bird>();

                ICustomerListIn<Bird> birdList1 = new CustomerListIn<Bird>();
                birdList1.Show(new Sparrow());
                birdList1.Show(new Bird());

                Action<Sparrow> act = new Action<Bird>((Bird i) => { });
            }


            {
                IMyList<Sparrow, Bird> myList1 = new MyList<Sparrow, Bird>();
                IMyList<Sparrow, Bird> myList2 = new MyList<Sparrow, Sparrow>();//协变
                IMyList<Sparrow, Bird> myList3 = new MyList<Bird, Bird>();//逆变
                IMyList<Sparrow, Bird> myList4 = new MyList<Bird, Sparrow>();//协变+逆变
            }
        }


        public class Bird
        {
            public int Id { get; set; }
        }
        public class Sparrow : Bird
        {
            public string Name { get; set; }
        }
        public interface ICustomerListIn<in T>
        {
            //T Get();

            void Show(T t);
        }

        public class CustomerListIn<T> : ICustomerListIn<T>
        {
            //public T Get()
            //{
            //    return default(T);
            //}

            public void Show(T t)
            {
            }
        }

        /// <summary>
        /// out 协变 只能是返回结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public interface ICustomerListOut<out T>
        {
            T Get();

            //void Show(T t);
        }

        public class CustomerListOut<T> : ICustomerListOut<T>
        {
            public T Get()
            {
                return default(T);
            }

            //public void Show(T t)
            //{

            //}
        }





        public interface IMyList<in inT, out outT>
        {
            void Show(inT t);
            outT Get();
            outT Do(inT t);

            ////out 只能是返回值   in只能是参数
            //void Show1(outT t);
            //inT Get1();

        }

        public class MyList<T1, T2> : IMyList<T1, T2>
        {

            public void Show(T1 t)
            {
                Console.WriteLine(t.GetType().Name);
            }

            public T2 Get()
            {
                Console.WriteLine(typeof(T2).Name);
                return default(T2);
            }

            public T2 Do(T1 t)
            {
                Console.WriteLine(t.GetType().Name);
                Console.WriteLine(typeof(T2).Name);
                return default(T2);
            }
        }
        #endregion


        #endregion
        #endregion

        #region  供委托调用
        public static int insert(int a, int b)
        {
            return a + b;
        }

        public static int delete(int a, int b)
        {
            return a - b;
        }
        #endregion

        #region  16进制字符串转数组
        public static byte[] HexToByte(string msg)
        {
            msg = msg.Replace("", "");
            byte[] comBuffer = new byte[msg.Length / 2];
            for (int i = 0; i < msg.Length; i += 2)
            {
                comBuffer[i / 2] = (byte)Convert.ToByte(msg.Substring(i, 2), 16); ;
            }
            return comBuffer;
        }

        #endregion

        #region  反射
        public static void AssemblyActivator()
        {
            Console.WriteLine("反射学习");
            Assembly assembly = Assembly.Load("SPMVCText");//相对路径
            //Assembly assembly1 = Assembly.LoadFile(@"D:\VS有关\VS项目\SPText\SPMVCText\SPMVCText.dll");//绝对路径
            //Assembly assembly2 = Assembly.LoadFrom("SPMVCText.dll");//相对路径和绝对路径都可以

            //foreach (var typeText in assembly.GetTypes())//获取所有类名称
            //{
            //    Console.WriteLine($"{typeText.Name}");
            //    foreach (var members in typeText.GetMembers())//获取所有方法名称
            //    {
            //        Console.WriteLine($"{members.Name}");
            //    }
            //}

            Type type = assembly.GetType("SPMVCText.Models.Text");
            //foreach (var constructor in type.GetConstructors())//获取当前类所有的构造函数
            //{
            //    Console.WriteLine($"{constructor.Name}");
            //    foreach (var parameter in constructor.GetParameters())//获取构造函数参数
            //    {
            //        Console.WriteLine($"{parameter.ParameterType}");//获取参数类型
            //    }
            //}



            object obj = Activator.CreateInstance(type);//获取实例
            //dynamic obj1= Activator.CreateInstance(type);  //dynamic可以避开编译器检查
            //IText obj2 = obj as IText;//强制转化



            //object obj3 = Activator.CreateInstance(type, new object[] { 123,"456" });//参数化默认构造函数
            {
                //普通方法（无参）
                MethodInfo memberInfo = type.GetMethod("Query");
                memberInfo.Invoke(obj, null);
            }
            {
                //静态方法
                MethodInfo memberInfo = type.GetMethod("Query");
                memberInfo.Invoke(null, new object[] { "" });
            }
            {
                //重载（两个参数）
                MethodInfo memberInfo1 = type.GetMethod("Query", new Type[] { typeof(int), typeof(string) });
                memberInfo1.Invoke(obj, new object[] { 111, "1111" });
            }
            {
                //调用私有方法
                MethodInfo memberInfo = type.GetMethod("Query", BindingFlags.NonPublic | BindingFlags.Instance);
                memberInfo.Invoke(null, new object[] { "1111" });
            }
            {
                Type typeText = typeof(Text);
                object objText = Activator.CreateInstance(typeText);
                //获取所有的公共属性
                foreach (PropertyInfo item in typeText.GetProperties())
                {
                    Console.WriteLine("{0}:{1}", item.Name, item.GetValue(objText));

                    if (item.Name.Equals("Id"))
                    {
                        item.SetValue(objText, "1");
                    }

                    Console.WriteLine("{0}:{1}", item.Name, item.GetValue(objText));
                }
                //获取所有的公共字段
                foreach (FieldInfo item in type.GetFields())
                {
                    Console.WriteLine("{0}:{1}", item.Name, item.GetValue(objText));

                    if (item.Name.Equals("Id"))
                    {
                        item.SetValue(objText, "1");
                    }

                    Console.WriteLine("{0}:{1}", item.Name, item.GetValue(objText));
                }
            }
            {
                //泛型类
                Type typeT = assembly.GetType("SPMVCText.Models.Text~3");//泛型类创建（~1表示泛型个数）
                Type typeMake = type.MakeGenericType(new Type[] { typeof(string), typeof(int), typeof(DateTime) });
                object objMake = Activator.CreateInstance(typeMake);
            }
            {
                //泛型方法
                MethodInfo method = type.GetMethod("Query");
                var methodNew = method.MakeGenericMethod(new Type[] { typeof(string), typeof(int), typeof(DateTime) });
                methodNew.Invoke(obj, new object[] { "123", 456, DateTime.Now });
            }
            {
                //泛型类和泛型方法
                Type typeT = assembly.GetType("SPMVCText.Models.Text~3").MakeGenericType(new Type[] { typeof(string), typeof(int), typeof(DateTime) });
                MethodInfo methodInfo = typeT.GetMethod("Query").MakeGenericMethod(new Type[] { typeof(string), typeof(int), typeof(DateTime) });
                methodInfo.Invoke(obj, new object[] { "123", 456, DateTime.Now });
            }

        }

        #endregion

        #region  添加日志
        //        CREATE TABLE[dbo].[Log]
        //        (

        //   [Id][int] IDENTITY (1, 1) NOT NULL,

        //  [Date] [datetime]
        //        NOT NULL,

        //  [Thread] [varchar] (255) NOT NULL,

        //  [Level] [varchar] (50) NOT NULL,

        //  [Logger] [varchar] (255) NOT NULL,

        //  [Message] [varchar] (4000) NOT NULL,

        //  [Exception] [varchar] (2000) NULL
        //)



        private static void InitLog4Net()
        {
            var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            XmlConfigurator.ConfigureAndWatch(logCfg);
        }
        #endregion

        #region  委托+linq(基于委托的封装，把通用逻辑完成，把可变逻辑当成委托进行传递)
        public static List<Text> TextWhere(List<Text> textList, Func<Text, bool> func)
        {
            List<Text> texts = new List<Text>();
            foreach (var item in textList)
            {
                if (func.Invoke(item))
                {
                    texts.Add(item);
                }
            }
            return texts;
        }
        #endregion

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
            string vakue = XmlHelper.Read(path, "/BaseInfo/Person[@Person='1']/Name");
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
            XmlHelper.SetFile(path1, JsonHelper.SerializeObject(baseInfo));
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

        #region  Cache
        private const string LoginUserKey = "CacheKey-LoginUserCacheKey";
        /// <summary>
        /// 获取或设置当前登录用户
        /// </summary>
        public static Text LoginUser
        {
            get { return WebCache.GetCache(LoginUserKey) as Text; }
            set { WebCache.SetCache(LoginUserKey, value); }
        }
        #endregion

        #region  测试方法
        public static void Show()
        {
            Console.WriteLine("线程1");
        }
        public static int Show1()
        {
            Console.WriteLine("线程2");
            return Thread.CurrentThread.ManagedThreadId;
        }
        #endregion

        #region  IO(序列化&反序列化、读取文件信息)
        public static void JsonAndFile()
        {
            {
                MyIO.Show();
                //MyIO.Log("1235677");

                //var directoryInfos = Recursion.GetAllDirectory(@"D:\软谋教育\Git_Work");
            }

            {
                string str = "字符串";
                //生成验证码
                IOSerialize.IO.ImageHelper.Drawing(str);
            }
            {
                //序列化&反序列化
                //Console.WriteLine("**************Serialize*************");
                SerializeHelper.BinarySerialize();
                SerializeHelper.SoapSerialize();
                SerializeHelper.XmlSerialize();
            }
            SerializeHelper.Json();

            List<Programmer> list = DataFactory.BuildProgrammerList();
            {
                Console.WriteLine("********************XmlHelper**********************");
                string xmlResult = XmlHelper.ToXml<List<Programmer>>(list);
                List<Programmer> list1 = XmlHelper.ToObject<List<Programmer>>(xmlResult);
                //List<Programmer> list2 = XmlHelper.FileToObject<List<Programmer>>("");
            }



            {
                string jResult = JsonHelper.SerializeObject<List<Programmer>>(list);
                List<Programmer> list1 = JsonHelper.DeserializeObject<List<Programmer>>(jResult);
            }
            {
                string jResult = JsonHelper.SerializeObject<List<Programmer>>(list);
                List<Programmer> list1 = JsonHelper.DeserializeObject<List<Programmer>>(jResult);
            }
        }
        #endregion

        #region  多线程（Thread、ThreadPool、Task、Parallel）
        public static void ThreadDemo()
        {
            {
                new ThreadHelper().Show();//多线程所有方法
            }
            {
                Action action = new Action(Show);
                System.Threading.Tasks.Task task = new System.Threading.Tasks.Task(action);
                task.Start();
                System.Threading.Tasks.Task.Delay(2000);//不阻塞
            }
            {
                System.Threading.Tasks.Task task = new System.Threading.Tasks.Task(() => Console.WriteLine("线程启动！"));
                task.Start();
            }
            //有返回值
            {
                Func<int> func = new Func<int>(Show1);
                Task<int> task = new Task<int>(func);
                task.Start();
                int i = task.Result;//阻塞当前线程
                Console.WriteLine($"线程返回值{i}");
            }
            {
                Task<int> taskSum = System.Threading.Tasks.Task.Run<int>(() =>
                {
                    int sum = 0;
                    for (int i = 0; i < 100; i++)
                    {
                        sum += i;
                    }
                    return sum;
                });
                Console.WriteLine($"最终结果1+....+100为：{taskSum.Result}");
            }
            {
                Action action = () =>
                {
                    Console.WriteLine($"这是一个Task线程 Start{Thread.CurrentThread.ManagedThreadId}");
                    Thread.Sleep(2000);
                    Console.WriteLine($"这是一个Task线程 End{Thread.CurrentThread.ManagedThreadId}");
                };
                System.Threading.Tasks.Task task = new System.Threading.Tasks.Task(action);
                task.Start();
            }
            {
                List<System.Threading.Tasks.Task> tasks = new List<System.Threading.Tasks.Task>();

                tasks.Add(System.Threading.Tasks.Task.Run(() => Console.WriteLine("1", "9")));
                tasks.Add(System.Threading.Tasks.Task.Run(() => Console.WriteLine("2", "9")));
                tasks.Add(System.Threading.Tasks.Task.Run(() => Console.WriteLine("3", "9")));
                TaskFactory taskFactory = new TaskFactory();
                taskFactory.ContinueWhenAny(tasks.ToArray(), p =>
                {
                    Console.WriteLine($"指令！，{Thread.CurrentThread.ManagedThreadId}");
                });
                taskFactory.ContinueWhenAll(tasks.ToArray(), p =>
                {
                    Console.WriteLine($"指令！，{Thread.CurrentThread.ManagedThreadId}");
                });

                System.Threading.Tasks.Task.WaitAny(tasks.ToArray());//阻塞当前线程，直到任意一个任务结束
                System.Threading.Tasks.Task.WaitAll(tasks.ToArray());//阻塞当前线程，直到全部任务结束
            }
            //Async
            {
                //1.
                Action action = () => Console.WriteLine("这是一个委托！");
                IAsyncResult asyncResult = null;
                AsyncCallback asyncCallback = (x) =>
                {
                    Console.WriteLine($"{object.ReferenceEquals(x, asyncResult)}");
                    Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}{new object[] { x.AsyncState, x.AsyncState }}");
                    Console.WriteLine("启用线程委托！");
                };
                asyncResult = action.BeginInvoke(asyncCallback, new object[] { "参数1", "参数2" });

                //2.
                int i = 0;
                while (!asyncResult.IsCompleted)
                {
                    if (i < 9)
                    {
                        Console.WriteLine($"{++i * 10}进行中");
                    }
                    else
                    {
                        Console.WriteLine($"进行中将完成");
                    }
                    Thread.Sleep(200);
                }
                Console.WriteLine("asyncResult.IsCompleted 已完成");

                //3.
                asyncResult.AsyncWaitHandle.WaitOne();//直接等待任务完成
                asyncResult.AsyncWaitHandle.WaitOne(-1);//一直等待任务完成        
                asyncResult.AsyncWaitHandle.WaitOne(1000);//最多等待1000ms，超时就不等了    

                //4 EndInvoke  即时等待,而且可以获取委托的返回值 一个异步操作只能End一次
                action.EndInvoke(asyncResult);//等待某次异步调用操作结束
            }
            //async await
            {
                Console.WriteLine("主线程方法调用开始");
                Task<int> t = IntAsyncTask();
                Console.WriteLine("主线程方法调用结束");
                long lResult = t.Result;//访问result   主线程等待Task的完成
                t.Wait();//等价于上一行
            }
            {
                Parallel.Invoke(
                    () => { Console.WriteLine("1"); },
                    () => { Console.WriteLine("2"); }
                    );

                Parallel.For(0, 2, i => { Console.WriteLine("打印"); });

            }
        }

        public static async Task<int> IntAsyncTask()
        {
            Console.WriteLine("方法调用开始");
            int ireturn = 0;
            await System.Threading.Tasks.Task.Run(() =>//启动新线程完成任务
            {
                Console.WriteLine("子线程调用进行中");
                for (int i = 0; i < 100_000_000; i++)
                {
                    ireturn += i;
                    Thread.Sleep(1000);
                }
                Console.WriteLine("子线程调用结束");
            });

            Console.WriteLine("方法调用结束");
            return ireturn;
        }
        #endregion

        #region  IOC
        public static void IOCShow()
        {
            {
                IUnityContainer container = new UnityContainer();//实例化容器
                container.RegisterType<SPTextLK.Text.IText, SPTextLK.Text.Text>();//注册容器
                SPTextLK.Text.IText text = container.Resolve<SPTextLK.Text.IText>();//获取实例
                text.One();
            }
            {
                IUnityContainer container = ContainnerFactory.GetContainer();
                IText text = container.Resolve<IText>();
                IClassBase classBase = container.Resolve<IClassBase>();
                text.One();
                classBase.Show();
            }
        }
        #endregion

        #region  设计模式
        public static void DesignPattern()
        {
            DesignPatternHelper designPatternHelper = new DesignPatternHelper();
            designPatternHelper.Show();
        }
        #endregion


        #region  数据类型/特殊类型
        public static void DataTypeAndSpecialType()
        {
            //数组
            {
                //1.
                int[] array = new int[] { 1, 2, 3 };

                //2.
                ArrayList arrayList = new ArrayList();
                arrayList.Add("1");
                arrayList.Add(2);
                arrayList.Add(DateTime.Now);

                //3.
                List<int> intList = new List<int>();
                intList.Add(1);
                intList.Add(2);
                intList.Add(3);
            }
            //链表
            {
                LinkedList<int> linkedList = new LinkedList<int>();
                linkedList.AddFirst(1);
                linkedList.AddLast(2);

                LinkedListNode<int> linkedListNode = linkedList.Find(1);//节点
                linkedList.AddBefore(linkedListNode, 3);
                linkedList.AddAfter(linkedListNode, 4);


                linkedList.Remove(1);
                linkedList.RemoveFirst();
                linkedList.RemoveLast();
                linkedList.Clear();
            }
            {
                Queue<string> queueList = new Queue<string>();//先进先出
                queueList.Enqueue("1");
                queueList.Enqueue("2");
                queueList.Enqueue("3");
                queueList.Enqueue("4");

                queueList.Dequeue();//移除第一个
                queueList.Peek();//返回第一个
            }
            {
                Stack<string> stackList = new Stack<string>();//先进后出
                stackList.Push("1");
                stackList.Push("2");
                stackList.Push("3");
                stackList.Push("4");

                stackList.Pop();//移除顶部
                stackList.Peek();//返回第一个
            }
            //Set
            {
                HashSet<string> hashSetList = new HashSet<string>();//去重

                hashSetList.Add("1");
                hashSetList.Add("2");
                hashSetList.Add("3");
                hashSetList.Count();
                hashSetList.Contains("1");
                hashSetList.ToList();
                hashSetList.Clear();

                HashSet<string> hashSetList2 = new HashSet<string>();//去重
                hashSetList.SymmetricExceptWith(hashSetList2);//补
                hashSetList.UnionWith(hashSetList2);//并
                hashSetList.ExceptWith(hashSetList2);//差
                hashSetList.IntersectWith(hashSetList2);//交
            }
            {
                //SortedSet
            }
            //Key-Value（哈希）
            {
                //Hashtable
                //Dictionary
                //SortedDictionary
            }

        }
        #endregion

        #region  字典缓存
        //List<Program> programList = null;
        //string key = $"{nameof(DBHelper)}_Query_{123}";

        //programList = SPText.CustomCache.GetT<List<Program>>(key, () => DBHelper.Query<Program>(123));
        #endregion

        #region  Redis
        public class StudentRedisParem
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Remark { get; set; }
        }
        public static void RedisShow()
        {
            {
                //事务模式 
                using (RedisClient client = new RedisClient("127.0.0.1", 6379))
                {
                    client.SetEntryInHash("ceshi", "id", "001");
                    Console.WriteLine(client.GetValuesFromHash("ceshi", "id").FirstOrDefault());
                }
                Data_StringTest.Show();
                Data_HashTest.Show();
                Data_SetAndZsetTest.Show();
                Data_ListTest.Show();
            }

            StudentRedisParem student_1 = new StudentRedisParem()
            {
                Id = 11,
                Name = "Eleven"
            };
            StudentRedisParem student_2 = new StudentRedisParem()
            {
                Id = 12,
                Name = "Twelve",
                Remark = "123423245"
            };

            //String（键值对 Key-Name）
            Console.WriteLine("********************String*********************");
            {
                using (RedisStringService service = new RedisStringService())
                {
                    service.Set<string>("student1", "梦的翅膀");
                    Console.WriteLine(service.Get("student1"));

                    service.Append("student1", "20180802");
                    Console.WriteLine(service.Get("student1"));

                    Console.WriteLine(service.GetAndSetValue("student1", "程序错误"));
                    Console.WriteLine(service.Get("student1"));

                    service.Set<string>("student2", "王", DateTime.Now.AddSeconds(5));
                    Thread.Sleep(5100);
                    Console.WriteLine(service.Get("student2"));

                    service.Set<int>("Age", 32);
                    Console.WriteLine(service.Incr("Age"));
                    Console.WriteLine(service.IncrBy("Age", 3));
                    Console.WriteLine(service.Decr("Age"));
                    Console.WriteLine(service.DecrBy("Age", 3));
                }
            }
            //Hashtable（集合对象）
            Console.WriteLine("********************Hashtable*********************");
            {
                using (RedisHashService service = new RedisHashService())
                {
                    service.SetEntryInHash("student", "id", "123456");
                    service.SetEntryInHash("student", "name", "张xx");
                    service.SetEntryInHash("student", "remark", "高级班的学员");

                    var keys = service.GetHashKeys("student");
                    var values = service.GetHashValues("student");
                    var keyValues = service.GetAllEntriesFromHash("student");
                    Console.WriteLine(service.GetValueFromHash("student", "id"));

                    service.SetEntryInHashIfNotExists("student", "name", "太子爷");
                    service.SetEntryInHashIfNotExists("student", "description", "高级班的学员2");

                    Console.WriteLine(service.GetValueFromHash("student", "name"));
                    Console.WriteLine(service.GetValueFromHash("student", "description"));
                    service.RemoveEntryFromHash("student", "description");
                    Console.WriteLine(service.GetValueFromHash("student", "description"));
                }
            }
            //Set（去重，交差并补）
            Console.WriteLine("***********************Set******************");
            {
                using (RedisSetService service = new RedisSetService())
                {
                    service.FlushAll();//清理全部数据

                    service.Add("advanced", "111");
                    service.Add("advanced", "112");
                    service.Add("advanced", "114");
                    service.Add("advanced", "114");
                    service.Add("advanced", "115");
                    service.Add("advanced", "115");
                    service.Add("advanced", "113");

                    var result = service.GetAllItemsFromSet("advanced");

                    var random = service.GetRandomItemFromSet("advanced");//随机获取
                    service.GetCount("advanced");//独立的ip数
                    service.RemoveItemFromSet("advanced", "114");

                    {
                        service.Add("begin", "111");
                        service.Add("begin", "112");
                        service.Add("begin", "115");

                        service.Add("end", "111");
                        service.Add("end", "114");
                        service.Add("end", "113");

                        var result1 = service.GetIntersectFromSets("begin", "end");
                        var result2 = service.GetDifferencesFromSet("begin", "end");
                        var result3 = service.GetUnionFromSets("begin", "end");
                        //共同好友   共同关注
                    }
                }
            }
            //ZSet：有序集合，排列（去重，交差并补）
            Console.WriteLine("********************ZSet*********************");
            {
                using (RedisZSetService service = new RedisZSetService())
                {
                    service.FlushAll();//清理全部数据

                    service.Add("advanced", "1");
                    service.Add("advanced", "2");
                    service.Add("advanced", "5");
                    service.Add("advanced", "4");
                    service.Add("advanced", "7");
                    service.Add("advanced", "5");
                    service.Add("advanced", "9");

                    var result1 = service.GetAll("advanced");
                    var result2 = service.GetAllDesc("advanced");

                    service.AddItemToSortedSet("Sort", "BY", 123234);
                    service.AddItemToSortedSet("Sort", "走自己的路", 123);
                    service.AddItemToSortedSet("Sort", "redboy", 45);
                    service.AddItemToSortedSet("Sort", "大蛤蟆", 7567);
                    service.AddItemToSortedSet("Sort", "路人甲", 9879);
                    service.AddRangeToSortedSet("Sort", new List<string>() { "123", "花生", "加菲猫" }, 3232);
                    var result3 = service.GetAllWithScoresFromSortedSet("Sort");

                    //交叉并
                }
            }

            //List（链表）
            Console.WriteLine("********************List*********************");
            {
                using (RedisListService service = new RedisListService())
                {
                    service.FlushAll();

                    service.Add("article", "eleven1234");
                    service.Add("article", "kevin");
                    service.Add("article", "大叔");
                    service.Add("article", "C卡");
                    service.Add("article", "触不到的线");
                    service.Add("article", "程序错误");
                    service.RPush("article", "eleven1234");
                    service.RPush("article", "kevin");
                    service.RPush("article", "大叔");
                    service.RPush("article", "C卡");
                    service.RPush("article", "触不到的线");
                    service.RPush("article", "程序错误");

                    var result1 = service.Get("article");
                    var result2 = service.Get("article", 0, 3);
                    //可以按照添加顺序自动排序；而且可以分页获取

                    Console.WriteLine("&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&");
                    //栈
                    service.FlushAll();
                    service.Add("article", "eleven1234");
                    service.Add("article", "kevin");
                    service.Add("article", "大叔");
                    service.Add("article", "C卡");
                    service.Add("article", "触不到的线");
                    service.Add("article", "程序错误");

                    for (int i = 0; i < 5; i++)
                    {
                        Console.WriteLine(service.PopItemFromList("article"));
                        var result3 = service.Get("article");
                    }
                    Console.WriteLine("&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&");
                    // 队列：生产者消费者模型   
                    service.FlushAll();
                    service.RPush("article", "eleven1234");
                    service.RPush("article", "kevin");
                    service.RPush("article", "大叔");
                    service.RPush("article", "C卡");
                    service.RPush("article", "触不到的线");
                    service.RPush("article", "程序错误");

                    for (int i = 0; i < 5; i++)
                    {
                        Console.WriteLine(service.PopItemFromList("article"));
                        var result4 = service.Get("article");
                    }
                    //分布式缓存，多服务器都可以访问到，多个生产者，多个消费者，任何产品只被消费一次
                }

                #region 生产者消费者
                using (RedisListService service = new RedisListService())
                {
                    service.Add("test", "这是一个学生Add1");
                    service.Add("test", "这是一个学生Add2");
                    service.Add("test", "这是一个学生Add3");

                    service.LPush("test", "这是一个学生LPush1");
                    service.LPush("test", "这是一个学生LPush2");
                    service.LPush("test", "这是一个学生LPush3");
                    service.LPush("test", "这是一个学生LPush4");
                    service.LPush("test", "这是一个学生LPush5");
                    service.LPush("test", "这是一个学生LPush6");

                    service.RPush("test", "这是一个学生RPush1");
                    service.RPush("test", "这是一个学生RPush2");
                    service.RPush("test", "这是一个学生RPush3");
                    service.RPush("test", "这是一个学生RPush4");
                    service.RPush("test", "这是一个学生RPush5");
                    service.RPush("test", "这是一个学生RPush6");

                    List<string> stringList = new List<string>();
                    for (int i = 0; i < 10; i++)
                    {
                        stringList.Add(string.Format($"放入任务{i}"));
                    }
                    service.Add("task", stringList);

                    Console.WriteLine(service.Count("test"));
                    Console.WriteLine(service.Count("task"));
                    var list = service.Get("test");
                    list = service.Get("task", 2, 4);

                    Action act = new Action(() =>
                    {
                        while (true)
                        {
                            Console.WriteLine("************请输入数据**************");
                            string testTask = Console.ReadLine();
                            service.LPush("test", testTask);
                        }
                    });
                    act.Invoke();
                    act.EndInvoke(act.BeginInvoke(null, null));
                }
                #endregion

                #region 发布订阅:观察者，一个数据源，多个接受者，只要订阅了就可以收到的，能被多个数据源共享
                System.Threading.Tasks.Task.Run(() =>
                {
                    using (RedisListService service = new RedisListService())
                    {
                        service.Subscribe("Eleven", (c, message, iRedisSubscription) =>
                        {
                            Console.WriteLine($"注册{1}{c}:{message}，Dosomething else");
                            if (message.Equals("exit"))
                                iRedisSubscription.UnSubscribeFromChannels("Eleven");
                        });//blocking
                    }
                });
                System.Threading.Tasks.Task.Run(() =>
                {
                    using (RedisListService service = new RedisListService())
                    {
                        service.Subscribe("ZhengBo", (c, message, iRedisSubscription) =>
                        {
                            Console.WriteLine($"注册{2}{c}:{message}，Dosomething else");
                            if (message.Equals("exit"))
                                iRedisSubscription.UnSubscribeFromChannels("ZhengBo");
                        });//blocking
                    }
                });
                System.Threading.Tasks.Task.Run(() =>
                {
                    using (RedisListService service = new RedisListService())
                    {
                        service.Subscribe("Twelve", (c, message, iRedisSubscription) =>
                        {
                            Console.WriteLine($"注册{3}{c}:{message}，Dosomething else");
                            if (message.Equals("exit"))
                                iRedisSubscription.UnSubscribeFromChannels("Twelve");
                        });//blocking
                    }
                });
                using (RedisListService service = new RedisListService())
                {
                    Thread.Sleep(1000);

                    service.Publish("Eleven", "Eleven123");
                    service.Publish("Eleven", "Eleven234");
                    service.Publish("Eleven", "Eleven345");
                    service.Publish("Eleven", "Eleven456");

                    service.Publish("Twelve", "Twelve123");
                    service.Publish("Twelve", "Twelve234");
                    service.Publish("Twelve", "Twelve345");
                    service.Publish("Twelve", "Twelve456");
                    Console.WriteLine("**********************************************");

                    service.Publish("ZhengBo", "exit");
                    service.Publish("ZhengBo", "123Eleven");
                    service.Publish("ZhengBo", "234Eleven");
                    service.Publish("ZhengBo", "345Eleven");
                    service.Publish("ZhengBo", "456Eleven");

                    service.Publish("Twelve", "exit");
                    service.Publish("Twelve", "123Twelve");
                    service.Publish("Twelve", "234Twelve");
                    service.Publish("Twelve", "345Twelve");
                    service.Publish("Twelve", "456Twelve");
                }
                //观察者模式：微信订阅号---群聊天---数据同步--
                //MSMQ---RabbitMQ---ZeroMQ---RedisList:学习成本 技术成本
                #endregion
            }

            {
                //RedisCache.Set("RedisCache", "111");
            }
        }
        #endregion

        #region  数据库相关操作
        public static void DatabaseOperations()
        {

            {
                Uow uow = new Uow();
                Expression<Func<Company, bool>> eps = PredicateBuilder.True<Company>();
                eps = eps.And(p => p.Id > 0);
                List<Company> a = uow.Company.GetAll().Where(eps.Compile()).ToList();
            }
            {
                ModelDbset modelDbset = new ModelDbset();
                IUnitWork unitWork = new UnitWork(modelDbset);
                List<Company> a = unitWork.Find<Company>().Where(p => p.Id > 0).ToList();
            }
            {
                {
                    SPText.EF.IDatabase database = new SqlserverDatabase(connectionStrings);
                    string sql = "select * from Company";
                    var i = database.FindTable(sql);
                    i.Columns.RemoveAt(1);
                }
                {
                    SPText.EF.IDatabase database = new SqlserverDatabase(connectionStrings);
                    var iList = database.FindList<Company>(p => p.Id > 0).ToList();
                }
            }
            {
                SPText.EF.DatabaseContext databaseContext = new SPText.EF.DatabaseContext();
                IBaseService baseService = new BaseService(databaseContext);
                List<Company> textModel = baseService.Query<Company>(p => p.Id > 0).ToList();
            }
            {
                //手写ORM
                CustomDBHelper database = new CustomDBHelper();
                List<Company> textModel = database.FindAll<Company>().ToList();
            }
            {

                InfoEarthFrame.Data.IDatabase database = new InfoEarthFrame.Data.SqlDatabase(connectionStrings);
                string sql = "select * from Company";
                var i = database.GetDataSetFromExcuteCommand(sql, new SqlParameter[] { });
            }
            {
                DBHelper dBHelper = new DBHelper();
                string sql = "select * from Company";
                var i = dBHelper.DataSet(sql, CommandType.Text, new SqlParameter[] { });
            }
            {//Dapper
                Common.DataHelper.IDatabase database = new SPText.Common.DataHelper.Dapper.SqlDatabase(connectionStrings);
                string sql = "select * from Company";
                var i = database.FindList<Company>();
                var value = database.FindTable(sql);
            }
            { //Dapper
                string sql = "select * from Company";
                var datatabel = Common.DataHelper.Dapper.DbContext.Query<Company>(sql, "where 1=1 and [Name]='王五'");
            }
            {//EF
                Common.DataHelper.IDatabase database = new SPText.Common.DataHelper.EF.SqlserverDatabase(connectionStrings);
                string sql = "select * from Company";
                var i = database.FindList<Company>();
                var value = database.FindTable(sql);
            }
            {//Sql
                var sql = SPText.Common.DataHelper.Sql.DatabaseCommon.SelectSql<Company>();
                var datatabel = SqlHelper.ExecuteDataTable(sql.ToString(), CommandType.Text, null);
            }
            {
                {
                    //使用示例 SqlServer
                    string sql = "SELECT * FROM Company order by Id desc";
                    CurrencyDBHelper db = new CurrencyDBHelper(DbProviderType.SqlServer, connectionStrings);
                    System.Data.DataTable data = db.GetDataSet(sql, null).Tables[0];
                    DbDataReader reader = db.ExecuteReader(sql, null);
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var value = reader[i];
                        }
                    }
                    reader.Close();
                }
                {
                    //使用示例 SqlServer
                    string sql = "SELECT * FROM Company order by Id desc";
                    DbUtility db = new DbUtility(connectionStrings, DbProviderType.SqlServer);
                    System.Data.DataTable data = db.ExecuteDataTable(sql, null);
                    DbDataReader reader = db.ExecuteReader(sql, null);

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var value = reader[i];
                        }
                    }
                    reader.Close();
                }
                //{
                //    //使用示例 SQLite
                //    string sql = "SELECT * FROM Company order by Id desc";
                //    DbUtility db = new DbUtility(connectionStrings, DbProviderType.SQLite);
                //    DataTable data = db.ExecuteDataTable(sql, null);
                //    DbDataReader reader = db.ExecuteReader(sql, null);
                //    reader.Close();
                //}
                //{

                //    //使用示例 MySql
                //    string sql = "SELECT * FROM Company order by Id desc";
                //    DbUtility db = new DbUtility(connectionStrings, DbProviderType.MySql);
                //    DataTable data = db.ExecuteDataTable(sql, null);
                //    DbDataReader reader = db.ExecuteReader(sql, null);
                //    reader.Close();
                //}
                //{
                //    string path0 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"File\NA312Model.xls");
                //    //string path = System.Web.HttpContext.Current.Server.MapPath("~/File/NA312Model.xls");
                //    //使用示例 Execl
                //    string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path0 + ";Extended Properties=Excel 8.0;";
                //    string sql = "SELECT * FROM [Sheet1$]";
                //    DbUtility db = new DbUtility(connectionStrings, DbProviderType.OleDb);
                //    System.Data.DataTable data = db.ExecuteDataTable(sql, null);
                //}
            }

        }
        #endregion

        #region  Lambda
        public static void lambdaOperation()
        {
            #region  Lambda
            #region 匿名类（只读）
            {
                var obj = new
                {
                    Id = 1,
                    Name = "张三"
                };
            }
            {
                object obj = new
                {
                    Id = 1,
                    Name = "张三"
                };
            }
            #endregion
            List<Text> texts = new List<Text>();
            texts.ObjWhere(p => p.Id == 1);


            //内连接
            var list0 = from x in texts
                        join y in texts on x.Id equals y.Id
                        select new
                        {
                            Id = 0,
                            Name = "1"
                        };

            //左连接
            var list1 = from x in texts
                        join y in texts on x.Id equals y.Id
                        into xylist
                        from xy in xylist.DefaultIfEmpty()
                        select new
                        {
                            Id = 0,
                            Name = "1"
                        };


            #endregion
        }
        #endregion

        #region  QRCode
        public static void GetQRCode()
        {
            string file = System.IO.Directory.GetCurrentDirectory();
            string filePath = Path.Combine(file, "QRCode");
            if (!System.IO.Directory.Exists(filePath))
            {
                System.IO.Directory.CreateDirectory(filePath);
            }

            QRCodeHelper qRCodeHelper = new QRCodeHelper();
            qRCodeHelper.GetQRCODEByString("https://www.baidu.com", file, 60);
        }

        #endregion

        #region  SFtp
        public static void SharpSSH()
        {

            string SftpIp = "192.168.3.3";
            string SftpPort = "23";
            string SftpUser = "user";
            string SftpPwd = "user";
            //上传至FTP
            SFtpHelper sftpHelper = new SFtpHelper(SftpIp, Convert.ToInt32(SftpPort), SftpUser, SftpPwd);
            if (sftpHelper.Connect())
            {
                if (SFtpHelper.Put("本地路径", "远程路径"))
                {
                    Console.WriteLine("操作完成");
                }
            }
            else
            {
                Console.WriteLine("SFTP连接失败");
            }
            sftpHelper.Disconnect();
        }
        #endregion

        #region  加密
        public static void Encrypt()
        {
            {
                string md50 = HashEncrypt.Encrypt("1");
                string md51 = HashEncrypt.Encrypt("1");
                string md52 = HashEncrypt.Encrypt("123456小李");
                string md53 = HashEncrypt.Encrypt("113456小李");
            }
            {
                string desEn = HashEncrypt.Encrypt("Richard老师");
                string desDe = HashEncrypt.Decrypt(desEn);
                string desEn1 = HashEncrypt.Encrypt("张三李四");
                string desDe1 = HashEncrypt.Decrypt(desEn1);
            }
            {
                KeyValuePair<string, string> encryptDecrypt = HashEncrypt.GetKeyPair();
                string rsaEn1 = HashEncrypt.Encrypt("net", encryptDecrypt.Key);
                string rsaDe1 = HashEncrypt.Decrypt(rsaEn1, encryptDecrypt.Value);
            }
        }
        #endregion


        #region  aop(面向切面编程)
        public static void Aop()
        {
            DecoratorAOP.Show();
            ProxyAOP.Show();
            RealProxyAOP.Show();
            CastleProxyAOP.Show();
            UnityConfigAOP.Show();
        }
        #endregion

        #region  爬虫
        public static void Crawler()
        {
            try
            {
                #region 抓取腾讯课堂类别数据 
                Common.Crawler.CategorySearch search = new Common.Crawler.CategorySearch();
                search.Crawler();
                #endregion

                //获取所有页的数据
                // 发现每一页的数据URL后面拼接的page数据不一样
                //1.需要先获取最大页数
                //2.就可以通过拼接不同的URL来获取每一页的数据 
                //4.循环读取数据
                //CourseSearch search = new CourseSearch(category);
                //search.ShowPageData("https://ke.qq.com/course/list?tuin=a3ff93bc");

                #region 抓取课程
                Common.Crawler.TencentCategoryEntity tencentCategoryEntity = new Common.Crawler.TencentCategoryEntity()
                {
                    Url = "https://ke.qq.com/course/list/.net?tuin=a3ff93bc"
                };
                Common.Crawler.CourseSearch search1 = new Common.Crawler.CourseSearch(tencentCategoryEntity);
                search1.Crawler();
                #endregion



                #region 获取Ajax数据 
                Common.Crawler.CourseSearch courseSearch = new Common.Crawler.CourseSearch();
                courseSearch.GetAjaxRequest();
                #endregion

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        #region  定时调度
        public static void Quartz()
        {
            {
                QuartzHelper quartzHelper = new QuartzHelper();
                quartzHelper.Show().GetAwaiter().GetResult();
            }
            {
                QuartzHelper quartzHelper = new QuartzHelper();
                quartzHelper.Show1();
            }
            {
                System.Timers.Timer timer = new System.Timers.Timer();
                timer.Enabled = true;
                timer.Interval = 1000 * 60 * 10;
                timer.Start();
                timer.Elapsed += new System.Timers.ElapsedEventHandler((source, e) =>
                {
                    var sourcePara = source;
                    var ePara = e;
                    if (DateTime.Now.Hour == 10 && DateTime.Now.Minute == 30)  //如果当前时间是10点30分
                        Console.WriteLine("OK, event fired at: " + DateTime.Now.ToString());
                });
            }
        }
        #endregion

        #region  值储存方式（DataTable，Hashtable，Dictionary，List）
        public static void EnumerableData()
        {
            {//DataTable
             //创建一个数据表
                System.Data.DataTable CustomersTable = new System.Data.DataTable();
                CustomersTable.TableName = "Customers";
                //声明数据表的行和列变量
                DataColumn column;
                DataRow row;
                //创建一个新列，设置列的数据列性和列名，并把这个新列添加到Customers表中
                column = new DataColumn();
                column.DataType = System.Type.GetType("System.Int32");
                column.ColumnName = " CustID ";
                CustomersTable.Columns.Add(column);
                //再创建一个新列
                column = new DataColumn();
                column.DataType = Type.GetType("System.String");
                column.ColumnName = " CustLName ";
                CustomersTable.Rows.Add(column);
                //创建新的一行并把这个行添加到Customers表中
                for (int i = 0; i < 10; i++)
                {
                    row = CustomersTable.NewRow();
                    row["CustID "] = i;
                    row["CustLName "] = "item " + i.ToString();
                    CustomersTable.Rows.Add(row);
                }
            }
            {//Hashtable
                Hashtable ht = new Hashtable(); //创建一个Hashtable实例
                ht.Add("E", "e");//添加keyvalue键值对
                ht.Add("A", "a");
                ht.Add("C", "c");
                ht.Add("B", "b");

                string s = (string)ht["A"];
                if (ht.Contains("E")) //判断哈希表是否包含特定键,其返回值为true或false
                    Console.WriteLine("the E key exist");
                ht.Remove("C");//移除一个keyvalue键值对
                Console.WriteLine(ht["A"]);//此处输出a
                ht.Clear();//移除所有元素
                Console.WriteLine(ht["A"]); //此处将不会有任何输出

                foreach (DictionaryEntry de in ht) //ht为一个Hashtable实例
                {
                    Console.WriteLine(de.Key);//de.Key对应于keyvalue键值对key
                    Console.WriteLine(de.Value);//de.Key对应于keyvalue键值对value
                }

                ArrayList akeys = new ArrayList(ht.Keys); //别忘了导入System.Collections
                akeys.Sort(); //按字母顺序进行排序
                foreach (string skey in akeys)
                {
                    Console.Write(skey + ":");
                    Console.WriteLine(ht[skey]); //排序后输出
                }

            }
            {//Dictionary
             //创建泛型哈希表,Key类型为int,Value类型为string
                Dictionary<int, string> myDictionary = new Dictionary<int, string>();
                //1.添加元素
                myDictionary.Add(1, "a");
                myDictionary.Add(2, "b");
                myDictionary.Add(3, "c");
                //2.删除元素
                myDictionary.Remove(3);
                //3.假如不存在元素则添加元素
                if (!myDictionary.ContainsKey(4))
                {
                    myDictionary.Add(4, "d");
                }
                //4.显示容量和元素个数
                Console.WriteLine("元素个数：{0}", myDictionary.Count);
                //5.通过key查找元素
                if (myDictionary.ContainsKey(1))
                {
                    Console.WriteLine("key:{0},value:{1}", "1", myDictionary[1]);
                    Console.WriteLine(myDictionary[1]);
                }
                //6.通过KeyValuePair遍历元素
                foreach (KeyValuePair<int, string> kvp in myDictionary)
                {
                    Console.WriteLine("key={0},value={1}", kvp.Key, kvp.Value);

                }
                //7.得到哈希表键的集合
                Dictionary<int, string>.KeyCollection keyCol = myDictionary.Keys;
                //遍历键的集合
                foreach (int n in keyCol)
                {
                    Console.WriteLine("key={0}", n);
                }
                //8.得到哈希表值的集合
                Dictionary<int, string>.ValueCollection valCol = myDictionary.Values;
                //遍历值的集合
                foreach (string s in valCol)
                {
                    Console.WriteLine("value：{0}", s);
                }
                //9.使用TryGetValue方法获取指定键对应的值
                string slove = string.Empty;
                if (myDictionary.TryGetValue(5, out slove))
                {
                    Console.WriteLine("查找结果：{0}", slove);
                }
                else
                {
                    Console.WriteLine("查找失败");
                }
                //10.清空哈希表
                myDictionary.Clear();
            }
            {//List
                List<int> list = new List<int>();
                list.Add(2);
                list.Add(3);
                list.Add(7);
                foreach (int prime in list) // Loop through List with foreach
                {
                    Console.WriteLine(prime);
                }
                for (int i = 0; i < list.Count; i++) // Loop through List with for
                {
                    Console.WriteLine(list[i]);
                }
            }
            List<string> strList = new List<string>();
        }
        #endregion

        #region  WebSocket
        public static void WebSocket()
        {
            WebSocketHelper webSocket = new WebSocketHelper();
            if (HttpContext.Current.IsWebSocketRequest)
            {
                HttpContext.Current.AcceptWebSocketRequest(webSocket.ProcessChat);
                HttpContext.Current.AcceptWebSocketRequest(webSocket.ProcessChat0);
            }
            else
            {
                HttpContext.Current.Response.Write("我不处理");
            }
        }
        #endregion

        #region  RabbitMQ（测试未通过）
        public static void RabbitMQ()
        {
            {//向RabbitMQ服务器发送消息
                var factory = new ConnectionFactory();
                factory.HostName = "localhost";//主机名，Rabbit会拿这个IP生成一个endpoint，这个很熟悉吧，就是socket绑定的那个终结点。
                factory.UserName = "guest";//默认用户名,用户可以在服务端自定义创建，有相关命令行
                factory.Password = "guest";//默认密码

                using (var connection = factory.CreateConnection())//连接服务器，即正在创建终结点。
                {
                    //创建一个通道，这个就是Rabbit自己定义的规则了，如果自己写消息队列，这个就可以开脑洞设计了
                    //这里Rabbit的玩法就是一个通道channel下包含多个队列Queue
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare("kibaQueue", false, false, false, null);//创建一个名称为kibaqueue的消息队列
                        var properties = channel.CreateBasicProperties();
                        properties.DeliveryMode = 1;
                        string message = "I am Kiba518"; //传递的消息内容
                        channel.BasicPublish("", "kibaQueue", properties, Encoding.UTF8.GetBytes(message)); //生产消息
                        Console.WriteLine($"Send:{message}");
                    }
                }
            }
            {//去RabbitMQ的服务器查看当前消息队列
                var factory = new ConnectionFactory();
                factory.HostName = "localhost";
                factory.UserName = "guest";
                factory.Password = "guest";

                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare("kibaQueue", false, false, false, null);

                        /* 这里定义了一个消费者，用于消费服务器接受的消息
                         * C#开发需要注意下这里，在一些非面向对象和面向对象比较差的语言中，是非常重视这种设计模式的。
                         * 比如RabbitMQ使用了生产者与消费者模式，然后很多相关的使用文章都在拿这个生产者和消费者来表述。
                         * 但是，在C#里，生产者与消费者对我们而言，根本算不上一种设计模式，他就是一种最基础的代码编写规则。
                         * 所以，大家不要复杂的名词吓到，其实，并没那么复杂。
                         * 这里，其实就是定义一个EventingBasicConsumer类型的对象，然后该对象有个Received事件，
                         * 该事件会在服务接收到数据时触发。
                         */
                        var consumer = new EventingBasicConsumer(channel);//消费者
                        channel.BasicConsume("kibaQueue", true, consumer);//消费消息
                        consumer.Received += (model, ea) =>
                        {
                            var body = ea.Body;
                            var message = Encoding.UTF8.GetString(body.ToArray());
                        };
                    }
                }
            }
        }
        #endregion

        #region  文件压缩
        public static void ZipShow()
        {
            {
                ZipHelper.ZipDirectory("E:\\test", "E:\\test1.zip");   //压缩文件夹，无密码
                ZipHelper.ZipDirectory("E:\\test", "E:\\test1.zip", "123456");  //压缩文件夹，有密码
            }
            //{
            //    SPTextCommon.HelperCommon.ImageUpload imageUpload = new SPTextCommon.HelperCommon.ImageUpload();
            //    imageUpload.Upload();
            //}
        }
        #endregion
    }
    #region  特性
    public static class AttributcMapping
    {
        public static bool ValiData<T>(T t)
        {
            Type type = typeof(T);

            foreach (var prop in type.GetProperties())
            {
                if (prop.IsDefined(typeof(AbstractValidateAttribute), true))
                {
                    object oValue = prop.GetValue(t);
                    AbstractValidateAttribute attribute = (AbstractValidateAttribute)prop.GetCustomAttribute(typeof(AbstractValidateAttribute), true);
                    attribute.Validate(oValue);
                }
                else
                {
                    continue;
                }
            }
            return true;
        }
        //获取特性上面标记内容
        public static string GetName(this MemberInfo member)
        {
            if (member.IsDefined(typeof(AbstractBaseAttribute), true))
            {
                AbstractBaseAttribute attribute = member.GetCustomAttribute<AbstractBaseAttribute>();
                return attribute.GetRcalName();
            }
            else
            {
                return member.Name;
            }
        }


    }

    #region  反射

    public class ClassAttribute : AbstractBaseAttribute
    {
        public ClassAttribute(string name) : base(name) { }
    }
    public class PropertyAttribute : AbstractBaseAttribute
    {
        public PropertyAttribute(string name) : base(name) { }
    }
    public class AbstractBaseAttribute : Attribute
    {
        private string _RcalName = null;

        public AbstractBaseAttribute(string RcalName)
        {
            this._RcalName = RcalName;
        }

        public string GetRcalName()
        {
            return this._RcalName;
        }
    }
    #endregion

    #region  特性进行验证
    public class RequiredAttribute : AbstractValidateAttribute
    {
        public override bool Validate(object oValue)
        {
            if (oValue == null || string.IsNullOrWhiteSpace(oValue.ToString()))
                return false;
            else
                return true;
        }
    }
    public class LengthAttribute : AbstractValidateAttribute
    {
        public int _Min = 0;
        public int _Max = 0;
        public LengthAttribute(int Min, int Max)
        {
            _Min = Min;
            _Max = Max;
        }

        public override bool Validate(object oValue)
        {
            if (oValue == null || oValue.ToString().Length < this._Min || oValue.ToString().Length > this._Max)
                return false;
            else
                return true;

        }
    }
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public abstract class AbstractValidateAttribute : Attribute
    {
        public abstract bool Validate(object oValue);
    }
    #endregion
    #endregion

    #region  测试类（特性类别名）
    [AbstractBaseAttribute("User")]
    public class Text
    {

        public int Id { get; set; }
        [AbstractBaseAttribute("Name")]
        [RequiredAttribute]
        public string UseName { get; set; }
    }
    #endregion

    #region  委托
    public delegate int DYDeleGate(int a, int b);

    public class CreateDelete
    {
        public Action ActionText;
        public void ShowText()
        {
            Console.WriteLine($"{this.GetType().Name}");
            ActionText?.Invoke();
        }

        public void ShowText1()
        {
            Console.WriteLine($"{this.GetType().Name}");
        }
        public void ShowText2()
        {
            Console.WriteLine($"{this.GetType().Name}");
        }
        public void ShowText3()
        {
            Console.WriteLine($"{this.GetType().Name}");
        }
    }


    #endregion

    #region  Linq
    public static class LinqClass
    {
        public static List<T> ObjWhere<T>(this List<T> _tList, Func<T, bool> func)
        {
            List<T> tList = new List<T>();
            foreach (var _t in _tList)
            {
                if (func.Invoke(_t))
                {
                    tList.Add(_t);
                }
            }
            return tList;
        }
    }
    #endregion

    #region  测试实体
    public class xmlStudent
    {
        public string Name { set; get; }
        public int Age { set; get; }
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
    #endregion

    #region  字典缓存
    public static class CustomCache
    {
        //多线程可能会出现线程冲突
        //private static readonly object CustomCache_Lock = new object();

        /// <summary>
        /// 主动清理
        /// </summary>
        static CustomCache()
        {
            System.Threading.Tasks.Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        Thread.Sleep(1000 * 60 * 10);
                        List<string> keyList = new List<string>();
                        //lock (CustomCache_Lock)
                        //{
                        foreach (var key in CustomCacheDictionary.Keys)
                        {
                            DataModel model = (DataModel)CustomCacheDictionary[key];
                            if (model.ObsloteType != ObsloteType.Never && model.DeadLine < DateTime.Now)
                            {
                                keyList.Add(key);
                            }
                        }
                        keyList.ForEach(s => Remove(s));
                        //}
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        continue;
                    }
                }
            });
        }

        private static Dictionary<string, object> CustomCacheDictionary = new Dictionary<string, object>();

        public static void Add(string key, object oVaule)
        {
            //lock (CustomCache_Lock)
            CustomCacheDictionary.Add(key, new DataModel()
            {
                Value = oVaule,
                ObsloteType = ObsloteType.Never,
            });
        }

        /// <summary>
        /// 绝对过期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="oVaule"></param>
        /// <param name="timeOutSecond"></param>
        public static void Add(string key, object oVaule, int timeOutSecond)
        {
            //lock (CustomCache_Lock)
            CustomCacheDictionary.Add(key, new DataModel()
            {
                Value = oVaule,
                ObsloteType = ObsloteType.Absolutely,
                DeadLine = DateTime.Now.AddSeconds(timeOutSecond)
            });
        }

        /// <summary>
        /// 相对过期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="oVaule"></param>
        /// <param name="duration"></param>
        public static void Add(string key, object oVaule, TimeSpan duration)
        {
            //lock (CustomCache_Lock)
            CustomCacheDictionary.Add(key, new DataModel()
            {
                Value = oVaule,
                ObsloteType = ObsloteType.Relative,
                DeadLine = DateTime.Now.Add(duration),
                Duration = duration
            });
        }

        /// <summary>
        /// 要求在Get前做Exists检测
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            return (T)(((DataModel)CustomCacheDictionary[key]).Value);
        }


        /// <summary>
        /// 被动清理，请求了数据，才能清理
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Exists(string key)
        {
            if (CustomCacheDictionary.ContainsKey(key))
            {
                DataModel model = (DataModel)CustomCacheDictionary[key];
                if (model.ObsloteType == ObsloteType.Never)
                {
                    return true;
                }
                else if (model.DeadLine < DateTime.Now)//现在已经超过你的最后时间
                {
                    //lock (CustomCache_Lock)
                    CustomCacheDictionary.Remove(key);
                    return false;
                }
                else
                {
                    if (model.ObsloteType == ObsloteType.Relative)//没有过期&是滑动 所以要更新
                    {
                        model.DeadLine = DateTime.Now.Add(model.Duration);
                    }
                    return true;
                }
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 删除key
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(string key)
        {
            //lock (CustomCache_Lock)
            CustomCacheDictionary.Remove(key);
        }

        public static void RemoveAll()
        {
            //lock (CustomCache_Lock)
            CustomCacheDictionary.Clear();
        }

        /// <summary>
        /// 按条件删除
        /// </summary>
        /// <param name="func"></param>
        public static void RemoveCondition(Func<string, bool> func)
        {
            List<string> keyList = new List<string>();
            //lock (CustomCache_Lock)
            foreach (var key in CustomCacheDictionary.Keys)
            {
                if (func.Invoke(key))
                {
                    keyList.Add(key);
                }
            }
            keyList.ForEach(s => Remove(s));
        }

        public static T GetT<T>(string key, Func<T> func)
        {
            T t = default(T);
            if (!CustomCache.Exists(key))
            {
                t = func.Invoke();
                CustomCache.Add(key, t);

            }
            else
            {
                t = CustomCache.Get<T>(key);
            }
            return t;
        }

        /// <summary>
        /// 缓存的信息
        /// </summary>
        internal class DataModel
        {
            public object Value { get; set; }
            public ObsloteType ObsloteType { get; set; }

            public DateTime DeadLine { get; set; }
            public TimeSpan Duration { get; set; }

            //数据清理后出发事件
            public event Action DataClearEvent;
        }

        public enum ObsloteType
        {
            //永久有效
            Never,
            //绝对过期
            Absolutely,
            //滑动过期
            Relative
        }
    }

    #endregion
}
