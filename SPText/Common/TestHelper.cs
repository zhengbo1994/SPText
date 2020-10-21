using NPOI.SS.Formula.Functions;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SPText.Common
{
    //委托的声明
    public delegate void DoSometing();
    public class TestHelper
    {
        public void Show()
        {
            //this.EventShow();
            //this.Tshow();
            //this.ReflectionShow();
            //this.TheardShow();
        }

        #region  事件代码测试
        public void EventShow()
        {
            #region 事件1
            MyEvent myEvent = new MyEvent();
            myEvent.doSometingEvent += ShowSomething;
            myEvent.Invoke();
            #endregion

            #region 事件2
            Bridegroom bridegroom = new Bridegroom();
            Friend friend1 = new Friend("张三");
            Friend friend2 = new Friend("李四");
            Friend friend3 = new Friend("王五");

            //使用 “+=” 来订阅事件
            bridegroom.MarryEvent += new Bridegroom.MarryHandler(friend1.SendMessage);
            bridegroom.MarryEvent += friend2.SendMessage;

            //发出通知，此时只有订阅了事件的对象才能收到通知
            bridegroom.OnMarriageComing("朋友门，我要结婚了，到时候准时参加婚礼！");
            Console.WriteLine("---------------------------------------------");

            //使用 “-=” 来取消事件订阅，此时李四将收不到通知
            bridegroom.MarryEvent -= friend2.SendMessage;

            //使用 “+=” 来订阅事件，此时王五可以收到通知
            bridegroom.MarryEvent += friend3.SendMessage;
            //发出通知
            bridegroom.OnMarriageComing("朋友门，我要结婚了，到时候准时参加婚礼！");
            #endregion
        }
        #endregion

        #region 泛型
        public void Tshow()
        {
            //协变：接口泛型参数加了个out，就是为了解决上述的问题
            {
                ICustomerListOut<Bird> customerList2 = new CustomerListOut<Sparrow>();
                customerList2.Get();/*有返回值*/
            }
            //逆变
            {
                ICustomerListIn<Sparrow> customerList1 = new CustomerListIn<Bird>();
                customerList1.Show(new Sparrow() { Id = 1, Name = "张三" });/*有参数*/
            }
            {
                IMyList<Sparrow, Bird> myList2 = new MyList<Sparrow, Sparrow>();//协变
                IMyList<Sparrow, Bird> myList3 = new MyList<Bird, Bird>();//逆变
                IMyList<Sparrow, Bird> myList4 = new MyList<Bird, Sparrow>();//协变+逆变
            }


        }
        /// <summary>
        /// 逆变：只能修饰传入参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public interface ICustomerListIn<in T>
        {
            void Show(T t);
        }

        public class CustomerListIn<T> : ICustomerListIn<T>
        {
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
        }

        public class CustomerListOut<T> : ICustomerListOut<T>
        {
            public T Get()
            {
                return default(T);
            }
        }

        public interface IMyList<in inT, out outT>
        {
            void Show(inT t);
            outT Get();
            outT Do(inT t);
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

        #region  反射
        public void ReflectionShow()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assemblie in assemblies)
            {
                Assembly assembly = Assembly.Load(assemblie.FullName);//程序集名称
                //Assembly assembly1 = Assembly.LoadFile(assemblie.Location);//绝对路径
                //Assembly assembly2 = Assembly.LoadFrom(assemblie.FullName);//程序集加后缀
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.Name == this.GetType().Name)
                    {
                        object obj = Activator.CreateInstance(type);
                        foreach (MemberInfo memberInfo in type.GetMethods())
                        {
                            if (memberInfo.Name == "MmemberInfo")
                            {
                                MethodInfo mmemberInfo = type.GetMethod(memberInfo.Name);
                                mmemberInfo.Invoke(obj, null);

                                //mmemberInfo.GetParameters(); /*指定方法的所有参数*/
                                //mmemberInfo.GetCustomAttributes();/*指定方法的所有自定义特性参数*/
                                //mmemberInfo.GetGenericArguments();/*泛型方法的所有参数。*/
                                //mmemberInfo.GetMethodImplementationFlags();
                                //mmemberInfo.MakeGenericMethod();/*泛型方法*/
                            }

                            //type.GetEnumNames(); /*枚举类型的名称*/
                            //type.GetEvents(); /*所有公共事件*/
                            //type.GetFields(); /*所有公共字段*/
                            //type.GetProperties(); /*所有公共属性*/
                            //type.GetMethods(); /*所有公共方法*/
                            //type.MakeGenericType();泛型类（~1）/**************/
                        }
                    }

                }



            }

        }
        #endregion

        #region  多线程
        public void TheardShow()
        {
            #region Thread
            {
                IAsyncResult asyncResult;
                AsyncCallback asyncCallback = (ar) =>
                {
                    Console.WriteLine(string.Format("这是一个回调，参数{0}！", ar.AsyncState));
                };
                Action action = new Action(ShowSomething);
                asyncResult = action.BeginInvoke(asyncCallback, "参数1");
                asyncResult.AsyncWaitHandle.WaitOne();
                action.EndInvoke(asyncResult);
            }
            {
                Thread thread = new Thread(() =>
                {
                    Console.WriteLine("异步线程开始！");
                });
                thread.Start();
            }
            {
                ThreadStart threadStart = new ThreadStart(ShowSomething);
                Thread thread1 = new Thread(threadStart);
                thread1.Start();
            }
            {
                ParameterizedThreadStart parameterizedThreadStart = new ParameterizedThreadStart((p) =>
                {
                    Console.WriteLine(string.Format("这是一个带有参数的委托，参数为{0}！", p));
                });
                Thread thread2 = new Thread(parameterizedThreadStart);
                thread2.Start();
            }
            #endregion

            #region ThreadPool
            WaitCallback waitCallback = new WaitCallback((ar) =>
            {
                Console.WriteLine(string.Format("这是一个ThreadPool的回调委托,参数为：{0}", ar));
            });
            ThreadPool.SetMinThreads(8, 8);
            ThreadPool.QueueUserWorkItem(waitCallback);
            #endregion

            #region Task
            {
                Task task = new Task(ShowSomething);
                task.Start();
            }
            {
                Task.Run(ShowSomething);
            }
            {
                Task task = new Task(() =>
                {
                    Console.WriteLine("这是一个Task方法启动！");
                });
                task.Start();
            }
            #endregion


            #region Parallel
            Parallel.For(0, 0, i =>
            {
                Console.WriteLine("第一个有参数的委托");
                Console.WriteLine("第二个有参数的委托");
                Console.WriteLine("第三个有参数的委托");
            });
            #endregion

            //CancellationTokenSource  /*线程取消*/
        }
        #endregion

        #region  方法
        private static void ShowSomething()
        {
            Console.WriteLine("ShowSomething");
        }

        private T ShowT<T, U>(T t, U u)
            where T : new()
            where U : class
        {
            Console.WriteLine("这是一个泛型方法");
            return t;
        }
        #endregion



        #region
        //URL格式必须严格为[url=http://pan.baidu.com/share/init?shareid=xxx&uk=xxx]http://pan.baidu.com/share/init?shareid=xxx&uk=xxx[/url]
        public void init(string url)
        {
            HttpGet(url);
            info = url.Replace("http://pan.baidu.com/share/init?", "");
        }

        //尝试密码，3次后请重新建立实例（否则会有验证码）
        public int trypwd(string pwd)
        {
            //取得时间戳
            System.DateTime time = System.DateTime.Now;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long ts = (time.Ticks - startTime.Ticks) / 10000;
            string data = HttpPost("http://pan.baidu.com/share/verify?" + info + "&t=" + ts.ToString() + "&bdstoken=null&channel=chunlei&clienttype=0&web=1&app_id=123456&logid=MTUwMTEyNDM2OTY5MzAuOTE5NTU5NjQwMTk0NDM0OA==", "pwd=" + pwd + "&vcode=&vcode_str=");
            if (data.Contains("\"errno\":-9")) return 0;
            else if (data.Contains("\"errno\":0")) return 1;
            else return -1;
        }
        private string HttpPost(string Url, string Data)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                request.Referer = "http://pan.baidu.com/share/init?" + info;
                request.ContentLength = Encoding.UTF8.GetByteCount(Data);
                request.CookieContainer = cookie;
                Stream myRequestStream = request.GetRequestStream();
                byte[] postBytes = Encoding.UTF8.GetBytes(Data);
                myRequestStream.Write(postBytes, 0, postBytes.Length);

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                response.Cookies = cookie.GetCookies(response.ResponseUri);
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();

                return retString;
            }
            catch (System.Exception ex)
            {
                return "error";
            }
        }
        private string HttpGet(string Url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            request.CookieContainer = cookie;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        //哈希类 36进制
        //有效哈希[10000000,11679615]
        static public string Hash2Str(int hash)
        {
            string ret = "";
            hash -= 10000000;
            while (hash > 0)
            {
                ret += Hash2Char(hash % 36);
                hash /= 36;
            }
            if (ret.Length == 3) ret += "0";    //前导零
            if (ret.Length == 2) ret += "00";
            if (ret.Length == 1) ret += "000";
            if (ret.Length == 0) ret += "0000";
            return reverse(ret);
        }
        static public int Str2Hash(string str)
        {
            int ret = 0;
            ret += Char2Hash(str[0]) * 46656;
            ret += Char2Hash(str[1]) * 1296;
            ret += Char2Hash(str[2]) * 36;
            ret += Char2Hash(str[3]);
            ret += 10000000;    //防止前导零
            return ret;
        }
        static private int Char2Hash(char c)
        {
            if (c >= '0' && c <= '9') return c - '0';
            else return c - 'a' + 10;
        }
        static private char Hash2Char(int hash)
        {
            if (hash >= 0 && hash <= 9) return (char)('0' + hash);
            else return (char)('a' + hash - 10);
        }
        static private string reverse(string str)
        {
            char[] arr = str.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        private CookieContainer cookie = new CookieContainer();
        private string info;
    }
    #endregion
}

    #region  事件
    #region 事件1
    public class MyEvent
    {
        public DoSometing doSometingDelegate;
        public event DoSometing doSometingEvent;

        public void Invoke()
        {
            if (this.doSometingEvent != null)
            {
                this.doSometingEvent.Invoke();
            }
        }
    }
    #endregion

    #region 事件2
    //新郎官，充当事件发布者角色
    public class Bridegroom
    {
        //自定义委托
        public delegate void MarryHandler(string msg);
        //定义事件
        public event MarryHandler MarryEvent;

        //发布事件
        public void OnMarriageComing(string msg)
        {
            if (MarryEvent != null)
            {
                MarryEvent.Invoke(msg);
            }
        }
    }

    public class Friend
    {
        //属性
        public string Name { get; set; }
        //构造函数
        public Friend(string name)
        {
            Name = name;
        }
        //事件处理函数，该函数需要符合 MarryHandler委托的定义
        public void SendMessage(string message)
        {
            //输出通知信息
            Console.WriteLine(message);
            //对事件做出处理
            Console.WriteLine(this.Name + "收到了，到时候准时参加");
        }
    }
    #endregion
    #endregion

    #region  泛型
    public class Bird
    {
        public int Id { get; set; }
    }
    public class Sparrow : Bird
    {
        public string Name { get; set; }
    }

    #endregion

    #region  
    #endregion
}
