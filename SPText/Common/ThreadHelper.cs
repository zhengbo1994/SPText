using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SPText.Common
{
    public class ThreadHelper
    {
        public void Show()
        {
            ThreadShow();
            ThreadPoolShow();
            TaskShow();
            ParallelShow();
            AwaitAsyncShow();
        }


        public void ThreadShow()
        {
            Console.WriteLine($"****************btnThread_Click Start {Thread.CurrentThread.ManagedThreadId.ToString("00")} {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}***************");
            {
                ParameterizedThreadStart method = o => this.DoSomethingLong("btnThread_Click");
                Thread thread = new Thread(method);
                thread.Start("123");//开启线程，执行委托的内容
            }
            {
                ThreadStart method = () =>
                {
                    Thread.Sleep(5000);
                    this.DoSomethingLong("btnThread_Click");
                    Thread.Sleep(5000);
                };
                Thread thread = new Thread(method);
                thread.Start();//开启线程，执行委托的内容
                //thread.Suspend();//暂停
                //thread.Resume();//恢复    真的不该要的，暂停不一定马上暂停；让线程操作太复杂了
                //thread.Abort();
                ////线程是计算机资源，程序想停下线程，只能向操作系统通知(线程抛异常)，
                ////会有延时/不一定能真的停下来
                //Thread.ResetAbort();
                //1等待
                //while (thread.ThreadState != ThreadState.Stopped)
                //{
                //    Thread.Sleep(200);//当前线程休息200ms
                //}
                //2 Join等待
                //thread.Join();//运行这句代码的线程，等待thread的完成
                //thread.Join(1000);//最多等待1000ms

                //Console.WriteLine("这里是线程执行完之后才操作。。。");

                //thread.Priority = ThreadPriority.Highest;
                ////最高优先级：优先执行，但不代表优先完成  甚至说极端情况下，还有意外发生，不能通过这个来控制线程的执行先后顺序
                thread.IsBackground = false;//默认是false 前台线程，进程关闭，线程需要计算完后才退出
                //thread.IsBackground = true;//关闭进程，线程退出
            }

            {
                ThreadStart threadStart = () => this.DoSomethingLong("btnThread_Click");
                Action actionCallBack = () =>
                  {
                      Thread.Sleep(2000);
                      Console.WriteLine($"This is Calllback {Thread.CurrentThread.ManagedThreadId.ToString("00")}");
                  };
                this.ThreadWithCallBack(threadStart, actionCallBack);
            }
            {
                Func<int> func = () =>
                    {
                        Thread.Sleep(5000);
                        return DateTime.Now.Year;
                    };
                Func<int> funcThread = this.ThreadWithReturn(func);//非阻塞
                Console.WriteLine("do something else/////");
                Console.WriteLine("do something else/////");
                Console.WriteLine("do something else/////");
                Console.WriteLine("do something else/////");
                Console.WriteLine("do something else/////");

                int iResult = funcThread.Invoke();//阻塞
            }
            {
                List<Thread> threads = new List<Thread>();
                for (int i = 0; i < 100; i++)
                {
                    if (threads.Count(t => t.ThreadState == System.Threading.ThreadState.Running) < 10)
                    {
                        Thread thread = new Thread(new ThreadStart(() => { }));
                        thread.Start();
                        threads.Add(thread);
                    }
                    else
                    {
                        Thread.Sleep(200);
                    }
                }
            }
            {
                //问题：比如有10个任务，每个任务都启动一个线程，每个线程都需要执行一段时间，最用要等待10个线程都执行完成，然后触发另外一个任务将前10个任务执行的结果打包返回，这样的场景怎么处理
                //启动10个thread---返回值保存到一个公开的集合(注意线程安全)---等待10个线程--都完成list就已经包含了全部结果
            }
            Console.WriteLine($"****************btnThread_Click End   {Thread.CurrentThread.ManagedThreadId.ToString("00")} {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}***************");
        }

        public void ThreadPoolShow()
        {
            Console.WriteLine($"****************btnThreadPool_Click Start {Thread.CurrentThread.ManagedThreadId.ToString("00")} {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}***************");
            {
                ThreadPool.QueueUserWorkItem(o => this.DoSomethingLong("btnThreadPool_Click1"));
                ThreadPool.QueueUserWorkItem(o => this.DoSomethingLong("btnThreadPool_Click2"), "昔梦");
            }

            {
                ThreadPool.GetMaxThreads(out int workerThreads, out int completionPortThreads);
                Console.WriteLine($"当前电脑最大workerThreads={workerThreads} 最大completionPortThreads={completionPortThreads}");

                ThreadPool.GetMinThreads(out int workerThreadsMin, out int completionPortThreadsMin);
                Console.WriteLine($"当前电脑最小workerThreads={workerThreadsMin} 最大completionPortThreads={completionPortThreadsMin}");

                //设置的线程池数量是进程全局的，
                //委托异步调用--Task--Parrallel--async/await 全部都是线程池的线程
                //直接new Thread不受这个数量限制的(但是会占用线程池的线程数量)
                ThreadPool.SetMaxThreads(8, 8);//设置的最大值，必须大于CPU核数，否则设置无效
                ThreadPool.SetMinThreads(2, 2);
                Console.WriteLine("&&&&&&&&&&&&&&&&&&&&&&&设置最大最小&&&&&&&&&&&&&&&&&&&&&&&&&&&");

                ThreadPool.GetMaxThreads(out int workerThreads1, out int completionPortThreads1);
                Console.WriteLine($"当前电脑最大workerThreads={workerThreads1} 最大completionPortThreads={completionPortThreads1}");

                ThreadPool.GetMinThreads(out int workerThreadsMin1, out int completionPortThreadsMin1);
                Console.WriteLine($"当前电脑最大workerThreads={workerThreadsMin1} 最大completionPortThreads={completionPortThreadsMin1}");
            }

            {
                //等待
                ManualResetEvent mre = new ManualResetEvent(false);
                //false---关闭---Set打开---true---WaitOne就能通过
                //true---打开--ReSet关闭---false--WaitOne就只能等待
                ThreadPool.QueueUserWorkItem(o =>
                {
                    this.DoSomethingLong("btnThreadPool_Click1");
                    mre.Set();
                });
                Console.WriteLine("Do Something else...");
                Console.WriteLine("Do Something else...");
                Console.WriteLine("Do Something else...");

                mre.WaitOne();
                Console.WriteLine("任务已经完成了。。。");
            }
            {
                //不要阻塞线程池里面的线程
                ThreadPool.SetMaxThreads(8, 8);
                ManualResetEvent mre = new ManualResetEvent(false);
                for (int i = 0; i < 10; i++)
                {
                    int k = i;
                    ThreadPool.QueueUserWorkItem(t =>
                    {
                        Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId.ToString("00")} show {k}");
                        if (k == 9)
                        {
                            mre.Set();
                        }
                        else
                        {
                            mre.WaitOne();
                        }
                    });
                }
                if (mre.WaitOne())
                {
                    Console.WriteLine("任务全部执行成功！");
                }
            }

            Console.WriteLine($"****************btnThreadPool_Click End   {Thread.CurrentThread.ManagedThreadId.ToString("00")} {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}***************");
        }

        public void TaskShow()
        {
            Console.WriteLine($"****************btnTask_Click Start {Thread.CurrentThread.ManagedThreadId.ToString("00")} {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}***************");
            {
                Task task = new Task(() => this.DoSomethingLong("btnTask_Click_1"));
                task.Start();
            }
            {
                Task task = Task.Run(() => this.DoSomethingLong("btnTask_Click_2"));
            }
            {
                TaskFactory taskFactory = Task.Factory;
                Task task = taskFactory.StartNew(() => this.DoSomethingLong("btnTask_Click_3"));
            }
            {
                ThreadPool.SetMaxThreads(8, 8);
                //线程池是单例的，全局唯一的
                //设置后，同时并发的Task只有8个；而且线程是复用的；
                //Task的线程是源于线程池
                //全局的，请不要这样设置！！！
                for (int i = 0; i < 100; i++)
                {
                    int k = i;
                    Task.Run(() =>
                    {
                        Console.WriteLine($"This is {k} running ThreadId={Thread.CurrentThread.ManagedThreadId.ToString("00")}");
                        Thread.Sleep(2000);
                    });
                }
                //假如说我想控制下Task的并发数量，该怎么做？
            }
            {
                {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    Console.WriteLine("在Sleep之前");
                    Thread.Sleep(2000);//同步等待--当前线程等待2s 然后继续
                    Console.WriteLine("在Sleep之后");
                    stopwatch.Stop();
                    Console.WriteLine($"Sleep耗时{stopwatch.ElapsedMilliseconds}");
                }
                {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    Console.WriteLine("在Delay之前");
                    Task task = Task.Delay(2000)
                        .ContinueWith(t =>
                        {
                            stopwatch.Stop();
                            Console.WriteLine($"Delay耗时{stopwatch.ElapsedMilliseconds}");

                            Console.WriteLine($"This is ThreadId={Thread.CurrentThread.ManagedThreadId.ToString("00")}");
                        });//异步等待--等待2s后启动新任务
                    Console.WriteLine("在Delay之后");
                    //stopwatch.Stop();
                    //Console.WriteLine($"Delay耗时{stopwatch.ElapsedMilliseconds}");
                }
            }
            {
                //什么时候能用多线程？ 任务能并发的时候
                //多线程能干嘛？提升速度/优化用户体验
                Console.WriteLine("Eleven开启了一学期的课程");
                this.Teach("Lesson1");
                this.Teach("Lesson2");
                this.Teach("Lesson3");
                //不能并发，因为有严格顺序(只有Eleven讲课)
                Console.WriteLine("部署一下项目实战作业，需要多人合作完成");
                //开发可以多人合作---多线程--提升性能

                TaskFactory taskFactory = new TaskFactory();
                List<Task> taskList = new List<Task>();
                taskList.Add(taskFactory.StartNew(() => this.Coding("冰封的心", "Portal")));
                taskList.Add(taskFactory.StartNew(() => this.Coding("随心随缘", "  DBA ")));
                taskList.Add(taskFactory.StartNew(() => this.Coding("心如迷醉", "Client")));
                taskList.Add(taskFactory.StartNew(() => this.Coding(" 千年虫", "BackService")));
                taskList.Add(taskFactory.StartNew(() => this.Coding("简单生活", "Wechat")));

                //谁第一个完成，获取一个红包奖励
                taskFactory.ContinueWhenAny(taskList.ToArray(), t => Console.WriteLine($"XXX开发完成，获取个红包奖励{Thread.CurrentThread.ManagedThreadId.ToString("00")}"));
                //实战作业完成后，一起庆祝一下
                taskList.Add(taskFactory.ContinueWhenAll(taskList.ToArray(), rArray => Console.WriteLine($"开发都完成，一起庆祝一下{Thread.CurrentThread.ManagedThreadId.ToString("00")}")));
                //ContinueWhenAny  ContinueWhenAll 非阻塞式的回调；而且使用的线程可能是新线程，也可能是刚完成任务的线程，唯一不可能是主线程


                //阻塞当前线程，等着任意一个任务完成
                Task.WaitAny(taskList.ToArray());//也可以限时等待
                Console.WriteLine("Eleven准备环境开始部署");
                //需要能够等待全部线程完成任务再继续  阻塞当前线程，等着全部任务完成
                Task.WaitAll(taskList.ToArray());
                Console.WriteLine("5个模块全部完成后，Eleven集中点评");

                //Task.WaitAny  WaitAll都是阻塞当前线程，等任务完成后执行操作
                //阻塞卡界面，是为了并发以及顺序控制
                //网站首页：A数据库 B接口 C分布式服务 D搜索引擎，适合多线程并发，都完成后才能返回给用户，需要等待WaitAll
                //列表页：核心数据可能来自数据库/接口服务/分布式搜索引擎/缓存，多线程并发请求，哪个先完成就用哪个结果，其他的就不管了
            }
            {
                TaskFactory taskFactory = new TaskFactory();
                List<Task> taskList = new List<Task>();
                taskList.Add(taskFactory.StartNew(o => this.Coding("冰封的心", "Portal"), "冰封的心"));
                taskList.Add(taskFactory.StartNew(o => this.Coding("随心随缘", "  DBA "), "随心随缘"));
                taskList.Add(taskFactory.StartNew(o => this.Coding("心如迷醉", "Client"), "心如迷醉"));
                taskList.Add(taskFactory.StartNew(o => this.Coding(" 千年虫", "BackService"), " 千年虫"));
                taskList.Add(taskFactory.StartNew(o => this.Coding("简单生活", "Wechat"), "简单生活"));

                //谁第一个完成，获取一个红包奖励
                taskFactory.ContinueWhenAny(taskList.ToArray(), t => Console.WriteLine($"{t.AsyncState}开发完成，获取个红包奖励{Thread.CurrentThread.ManagedThreadId.ToString("00")}"));
            }
            {
                Task.Run(() => this.DoSomethingLong("btnTask_Click")).ContinueWith(t => Console.WriteLine($"btnTask_Click已完成{Thread.CurrentThread.ManagedThreadId.ToString("00")}"));//回调
            }
            {
                Task<int> result = Task.Run<int>(() =>
                {
                    Thread.Sleep(2000);
                    return DateTime.Now.Year;
                });
                int i = result.Result;//会阻塞
            }
            {
                Task.Run<int>(() =>
                {
                    Thread.Sleep(2000);
                    return DateTime.Now.Year;
                }).ContinueWith(tInt =>
                {
                    int i = tInt.Result;
                });
                Task.Run(() =>
                {
                    //int i = result.Result;//会阻塞
                });

            }
            {
                //假如说我想控制下Task的并发数量，该怎么做？  20个
                List<Task> taskList = new List<Task>();
                for (int i = 0; i < 10000; i++)
                {
                    int k = i;
                    if (taskList.Count(t => t.Status != TaskStatus.RanToCompletion) >= 20)
                    {
                        Task.WaitAny(taskList.ToArray());
                        taskList = taskList.Where(t => t.Status != TaskStatus.RanToCompletion).ToList();
                    }
                    taskList.Add(Task.Run(() =>
                    {
                        Console.WriteLine($"This is {k} running ThreadId={Thread.CurrentThread.ManagedThreadId.ToString("00")}");
                        Thread.Sleep(2000);
                    }));
                }
            }
            {

            }


            Console.WriteLine($"****************btnTask_Click End   {Thread.CurrentThread.ManagedThreadId.ToString("00")} {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}***************");
        }

        public void ParallelShow()
        {
            Console.WriteLine($"****************btnParallel_Click Start   {Thread.CurrentThread.ManagedThreadId.ToString("00")} {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}***************");
            {
                //Parallel并发执行多个Action 多线程的，
                //主线程会参与计算---阻塞界面
                //等于TaskWaitAll+主线程计算
                Parallel.Invoke(() => this.DoSomethingLong("btnParallel_Click_1"),
                    () => this.DoSomethingLong("btnParallel_Click_2"),
                    () => this.DoSomethingLong("btnParallel_Click_3"),
                    () => this.DoSomethingLong("btnParallel_Click_4"),
                    () => this.DoSomethingLong("btnParallel_Click_5"));
            }
            {
                Parallel.For(0, 5, i => this.DoSomethingLong($"btnParallel_Click_{i}"));
            }
            {
                Parallel.ForEach(new int[] { 0, 1, 2, 3, 4 }, i => this.DoSomethingLong($"btnParallel_Click_{i}"));
            }
            {
                ParallelOptions options = new ParallelOptions();
                options.MaxDegreeOfParallelism = 3;
                Parallel.For(0, 10, options, i => this.DoSomethingLong($"btnParallel_Click_{i}"));
            }
            {
                //有没有办法不阻塞？
                Task.Run(() =>
                {
                    ParallelOptions options = new ParallelOptions();
                    options.MaxDegreeOfParallelism = 3;
                    Parallel.For(0, 10, options, i => this.DoSomethingLong($"btnParallel_Click_{i}"));
                });
            }

            Console.WriteLine($"****************btnParallel_Click End   {Thread.CurrentThread.ManagedThreadId.ToString("00")} {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}***************");
        }

        public void AwaitAsyncShow() {
            var v= Test.TestShow();
        }

        /// 1 多异常处理和线程取消
        /// 2 多线程的临时变量
        /// 3 线程安全和锁lock
        /// 4 await/async
        public void ThreadCoreShow()
        {
            object Form_Lock = new object();
            int iNumSync = 0;
            int iNumAsync = 0;//非线程安全
            List<int> iListAsync = new List<int>();
            Console.WriteLine($"****************btnThreadCore_Click Start   {Thread.CurrentThread.ManagedThreadId.ToString("00")} {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}***************");
            //throw new Exception("");
            #region 多线程异常处理
            {
                try
                {
                    List<Task> taskList = new List<Task>();
                    for (int i = 0; i < 100; i++)
                    {
                        string name = $"btnThreadCore_Click_{i}";
                        taskList.Add(Task.Run(() =>
                        {
                            if (name.Equals("btnThreadCore_Click_11"))
                            {
                                throw new Exception("btnThreadCore_Click_11异常");
                            }
                            else if (name.Equals("btnThreadCore_Click_12"))
                            {
                                throw new Exception("btnThreadCore_Click_12异常");
                            }
                            else if (name.Equals("btnThreadCore_Click_38"))
                            {
                                throw new Exception("btnThreadCore_Click_38异常");
                            }
                            Console.WriteLine($"This is {name}成功 ThreadId={Thread.CurrentThread.ManagedThreadId.ToString("00")}");
                        }));
                    }
                    //多线程里面抛出的异常，会终结当前线程；但是不会影响别的线程；
                    //那线程异常哪里去了？ 被吞了，
                    //假如我想获取异常信息，还需要通知别的线程
                    Task.WaitAll(taskList.ToArray());//1 可以捕获到线程的异常
                }
                catch (AggregateException aex)//2 需要try-catch-AggregateException
                {
                    foreach (var exception in aex.InnerExceptions)
                    {
                        Console.WriteLine(exception.Message);
                    }
                }
                catch (Exception ex)//可以多catch  先具体再全部
                {
                    Console.WriteLine(ex);
                }
                //线程异常后经常是需要通知别的线程，而不是等到WaitAll，问题就是要线程取消
                //工作中常规建议：多线程的委托里面不允许异常，包一层try-catch,然后记录下来异常信息，完成需要的操作
            }
            #endregion

            #region 线程取消
            {
                //多线程并发任务，某个失败后，希望通知别的线程，都停下来，how？
                //Thread.Abort--终止线程；向当前线程抛一个异常然后终结任务；线程属于OS资源，可能不会立即停下来
                //Task不能外部终止任务，只能自己终止自己(上帝才能打败自己)

                //cts有个bool属性IsCancellationRequested 初始化是false
                //调用Cancel方法后变成true(不能再变回去),可以重复cancel
                try
                {
                    CancellationTokenSource cts = new CancellationTokenSource();
                    List<Task> taskList = new List<Task>();
                    for (int i = 0; i < 50; i++)
                    {
                        string name = $"btnThreadCore_Click_{i}";
                        taskList.Add(Task.Run(() =>
                        {
                            try
                            {
                                if (!cts.IsCancellationRequested)
                                    Console.WriteLine($"This is {name} 开始 ThreadId={Thread.CurrentThread.ManagedThreadId.ToString("00")}");

                                Thread.Sleep(new Random().Next(50, 100));

                                if (name.Equals("btnThreadCore_Click_11"))
                                {
                                    throw new Exception("btnThreadCore_Click_11异常");
                                }
                                else if (name.Equals("btnThreadCore_Click_12"))
                                {
                                    throw new Exception("btnThreadCore_Click_12异常");
                                }
                                else if (name.Equals("btnThreadCore_Click_13"))
                                {
                                    cts.Cancel();
                                }
                                if (!cts.IsCancellationRequested)
                                {
                                    Console.WriteLine($"This is {name}成功结束 ThreadId={Thread.CurrentThread.ManagedThreadId.ToString("00")}");
                                }
                                else
                                {
                                    Console.WriteLine($"This is {name}中途停止 ThreadId={Thread.CurrentThread.ManagedThreadId.ToString("00")}");
                                    return;
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                cts.Cancel();
                            }
                        }, cts.Token));
                    }
                    //1 准备cts  2 try-catch-cancel  3 Action要随时判断IsCancellationRequested
                    //尽快停止，肯定有延迟，在判断环节才会结束

                    Task.WaitAll(taskList.ToArray());
                    //如果线程还没启动，能不能就别启动了？
                    //1 启动线程传递Token  2 异常抓取  
                    //在Cancel时还没有启动的任务，就不启动了；也是抛异常，cts.Token.ThrowIfCancellationRequested
                }
                catch (AggregateException aex)
                {
                    foreach (var exception in aex.InnerExceptions)
                    {
                        Console.WriteLine(exception.Message);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            #endregion

            #region 临时变量
            {
                for (int i = 0; i < 5; i++)
                {
                    Task.Run(() =>
                    {
                        Console.WriteLine($"This is btnThreadCore_Click_{i} ThreadId={Thread.CurrentThread.ManagedThreadId.ToString("00")}");
                    });
                }
                //临时变量问题，线程是非阻塞的，延迟启动的；线程执行的时候，i已经是5了
                //k是闭包里面的变量，每次循环都有一个独立的k
                //5个k变量  1个i变量
                for (int i = 0; i < 5; i++)
                {
                    int k = i;
                    Task.Run(() =>
                    {
                        Console.WriteLine($"This is btnThreadCore_Click_{i}_{k} ThreadId={Thread.CurrentThread.ManagedThreadId.ToString("00")}");
                    });
                }
            }
            #endregion

            #region 线程安全&lock
            {
                //线程安全：如果你的代码在进程中有多个线程同时运行这一段，如果每次运行的结果都跟单线程运行时的结果一致，那么就是线程安全的
                //线程安全问题一般都是有全局变量/共享变量/静态变量/硬盘文件/数据库的值，只要多线程都能访问和修改
                //发生是因为多个线程相同操作，出现了覆盖，怎么解决？
                //1 Lock解决多线程冲突
                //Lock是语法糖，Monitor.Enter,占据一个引用，别的线程就只能等着
                //推荐锁是private static readonly object，
                // A不能是Null，可以编译不能运行;
                //B 不推荐lock(this),外面如果也要用实例，就冲突了
                Test test0 = new Test();
                Task.Delay(1000).ContinueWith(t =>
                {
                    lock (test0)
                    {
                        Console.WriteLine("*********Start**********");
                        Thread.Sleep(5000);
                        Console.WriteLine("*********End**********");
                    }
                });
                test0.DoTest();

                //C 不应该是string； string在内存分配上是重用的，会冲突
                //D Lock里面的代码不要太多，这里是单线程的
                Test test = new Test();
                string student = "水煮鱼";
                Task.Delay(1000).ContinueWith(t =>
                {
                    lock (student)
                    {
                        Console.WriteLine("*********Start**********");
                        Thread.Sleep(5000);
                        Console.WriteLine("*********End**********");
                    }
                });
                test.DoTestString();
                //2 线程安全集合
                //System.Collections.Concurrent.ConcurrentQueue<int>

                //3 数据分拆，避免多线程操作同一个数据；又安全又高效

                for (int i = 0; i < 10000; i++)
                {
                    iNumSync++;
                }
                for (int i = 0; i < 10000; i++)
                {
                    Task.Run(() =>
                    {
                        lock (Form_Lock)//任意时刻只有一个线程能进入方法块儿，这不就变成了单线程
                        {
                            iNumAsync++;
                        }
                    });
                }
                for (int i = 0; i < 10000; i++)
                {
                    int k = i;
                    Task.Run(() => iListAsync.Add(k));
                }

                Thread.Sleep(5 * 1000);
                Console.WriteLine($"iNumSync={iNumSync} iNumAsync={iNumAsync} listNum={iListAsync.Count}");
                //iNumSync 和  iNumAsync分别是多少   9981/9988  1到10000以内
            }
            #endregion

            Console.WriteLine($"****************btnThreadCore_Click End   {Thread.CurrentThread.ManagedThreadId.ToString("00")} {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}***************");
        }

        #region  帮助方法
        /// <summary>
        /// 1 异步，非阻塞的
        /// 2 还能获取到最终计算结果
        /// 
        /// 既要不阻塞，又要计算结果？不可能！
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        private Func<T> ThreadWithReturn<T>(Func<T> func)
        {
            T t = default(T);
            ThreadStart threadStart = new ThreadStart(() =>
            {
                t = func.Invoke();
            });
            Thread thread = new Thread(threadStart);
            thread.Start();

            return new Func<T>(() =>
            {
                thread.Join();
                //thread.ThreadState
                return t;
            });
        }
        //基于thread封装一个回调
        //回调：启动子线程执行动作A--不阻塞--A执行完后子线程会执行动作B
        /// <summary>
        /// 
        /// </summary>
        /// <param name="threadStart">多线程执行的操作</param>
        /// <param name="actionCallback">线程完成后，回调的动作</param>
        private void ThreadWithCallBack(ThreadStart threadStart, Action actionCallback)
        {
            //Thread thread = new Thread(threadStart);
            //thread.Start();
            //thread.Join();//错了，因为方法被阻塞了
            //actionCallback.Invoke();

            ThreadStart method = new ThreadStart(() =>
            {
                threadStart.Invoke();
                actionCallback.Invoke();
            });
            new Thread(method).Start();
        }
        /// <summary>
        /// 一个比较耗时耗资源的私有方法
        /// </summary>
        /// <param name="name"></param>
        private void DoSomethingLong(string name)
        {
            Console.WriteLine($"****************DoSomethingLong Start  {name}  {Thread.CurrentThread.ManagedThreadId.ToString("00")} {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}***************");
            long lResult = 0;
            for (int i = 0; i < 1_000_000_000; i++)
            {
                lResult += i;
            }
            //Thread.Sleep(2000);

            Console.WriteLine($"****************DoSomethingLong   End  {name}  {Thread.CurrentThread.ManagedThreadId.ToString("00")} {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} {lResult}***************");
        }

        /// <summary>
        /// 模拟Coding过程
        /// </summary>
        /// <param name="name"></param>
        /// <param name="projectName"></param>
        private void Coding(string name, string projectName)
        {
            Console.WriteLine($"****************Coding Start  {name} {projectName}  {Thread.CurrentThread.ManagedThreadId.ToString("00")} {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}***************");
            long lResult = 0;
            for (int i = 0; i < 1_000_000_000; i++)
            {
                lResult += i;
            }

            Console.WriteLine($"****************Coding   End  {name} {projectName} {Thread.CurrentThread.ManagedThreadId.ToString("00")} {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} {lResult}***************");
        }
        private void Teach(string lesson)
        {
            Console.WriteLine($"{lesson}开始讲。。。");
            long lResult = 0;
            for (int i = 0; i < 1_000_000_000; i++)
            {
                lResult += i;
            }
            Console.WriteLine($"{lesson}讲完了。。。");
        }
        #endregion
    }

    public class Test
    {
        /// <summary>
        /// 
        /// </summary>
        public void DoTest()
        {
            lock (this)
            //递归调用，lock this  会不会死锁？ 98%说会！ 不会死锁！
            //这里是同一个线程，这个引用就是被这个线程所占据
            {
                Thread.Sleep(500);
                this.iDoTestNum++;
                if (DateTime.Now.Day < 28 && this.iDoTestNum < 10)
                {
                    Console.WriteLine($"This is {this.iDoTestNum}次 {DateTime.Now.Day}");
                    this.DoTest();
                }
                else
                {
                    Console.WriteLine("28号，课程结束！！");
                }
            }
        }
        public void DoTestString()
        {
            lock (this.Name)
            //递归调用，lock this  会不会死锁？ 98%说会！ 不会死锁！
            //这里是同一个线程，这个引用就是被这个线程所占据
            {
                Thread.Sleep(500);
                this.iDoTestNum++;
                if (DateTime.Now.Day < 28 && this.iDoTestNum < 10)
                {
                    Console.WriteLine($"This is {this.iDoTestNum}次 {DateTime.Now.Day}");
                    this.DoTestString();
                }
                else
                {
                    Console.WriteLine("28号，课程结束！！");
                }
            }
        }

        /// <summary>
        /// 只有async没有await，会有个warn
        /// 跟普通方法没有区别
        /// </summary>
        private static async void NoReturnNoAwait()
        {
            //主线程执行
            Console.WriteLine($"NoReturnNoAwait Sleep before Task,ThreadId={Thread.CurrentThread.ManagedThreadId}");
            Task task = Task.Run(() =>//启动新线程完成任务
            {
                Console.WriteLine($"NoReturnNoAwait Sleep before,ThreadId={Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(3000);
                Console.WriteLine($"NoReturnNoAwait Sleep after,ThreadId={Thread.CurrentThread.ManagedThreadId}");
            });

            //主线程执行
            Console.WriteLine($"NoReturnNoAwait Sleep after Task,ThreadId={Thread.CurrentThread.ManagedThreadId}");
        }

        /// <summary>
        /// async/await 
        /// 不能单独await
        /// await 只能放在task前面
        /// 不推荐void返回值，使用Task来代替
        /// Task和Task<T>能够使用await, Task.WhenAny, Task.WhenAll等方式组合使用。Async Void 不行
        /// </summary>
        private static async void NoReturn()
        {
            //主线程执行
            Console.WriteLine($"NoReturn Sleep before await,ThreadId={Thread.CurrentThread.ManagedThreadId}");
            TaskFactory taskFactory = new TaskFactory();
            Task task = taskFactory.StartNew(() =>
            {
                Console.WriteLine($"NoReturn Sleep before,ThreadId={Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(3000);
                Console.WriteLine($"NoReturn Sleep after,ThreadId={Thread.CurrentThread.ManagedThreadId}");
            });

            task.ContinueWith(t =>
            {
                Console.WriteLine($"NoReturn Sleep after await,ThreadId={Thread.CurrentThread.ManagedThreadId}");
            });
            await task;//主线程到这里就返回了，执行主线程任务
            Console.WriteLine($"NoReturn Sleep after await,ThreadId={Thread.CurrentThread.ManagedThreadId}");
            //一流水儿的写下去的，耗时任务就用await
            //nodejs
            //子线程执行   其实是封装成委托，在task之后成为回调（编译器功能  状态机实现）
            //task.ContinueWith()
            //这个回调的线程是不确定的：可能是主线程  可能是子线程  也可能是其他线程
        }

        /// <summary>
        /// 无返回值  async Task == async void
        /// Task和Task<T>能够使用await, Task.WhenAny, Task.WhenAll等方式组合使用。Async Void 不行
        /// </summary>
        /// <returns></returns>
        private static async Task NoReturnTask()
        {
            //这里还是主线程的id
            Console.WriteLine($"NoReturnTask Sleep before await,ThreadId={Thread.CurrentThread.ManagedThreadId}");

            Task task = Task.Run(() =>
            {
                Console.WriteLine($"NoReturnTask Sleep before,ThreadId={Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(3000);
                Console.WriteLine($"NoReturnTask Sleep after,ThreadId={Thread.CurrentThread.ManagedThreadId}");
            });
            await task;
            Console.WriteLine($"NoReturnTask Sleep after await,ThreadId={Thread.CurrentThread.ManagedThreadId}");

            //return new TaskFactory().StartNew(() => { });  //不能return  没有async才行
        }

        #region  Task
        /// <summary>
        /// 带返回值的Task  
        /// 要使用返回值就一定要等子线程计算完毕
        /// </summary>
        /// <returns>async 就只返回long</returns>
        private static async Task<long> SumAsync()
        {
            Console.WriteLine($"SumAsync 111 start ManagedThreadId={Thread.CurrentThread.ManagedThreadId}");
            long result = 0;

            await Task.Run(() =>
            {
                for (int k = 0; k < 10; k++)
                {
                    Console.WriteLine($"SumAsync {k} await Task.Run ManagedThreadId={Thread.CurrentThread.ManagedThreadId}");
                    Thread.Sleep(1000);
                }
                for (long i = 0; i < 999_999_999; i++)
                {
                    result += i;
                }
            });

            Console.WriteLine($"SumFactory 111   end ManagedThreadId={Thread.CurrentThread.ManagedThreadId}");
            await Task.Run(() =>
            {
                for (int k = 0; k < 10; k++)
                {
                    Console.WriteLine($"SumAsync {k} await Task.Run ManagedThreadId={Thread.CurrentThread.ManagedThreadId}");
                    Thread.Sleep(1000);
                }
                for (long i = 0; i < 999999999; i++)
                {
                    result += i;
                }
            });

            Console.WriteLine($"SumFactory 111   end ManagedThreadId={Thread.CurrentThread.ManagedThreadId}");

            await Task.Run(() =>
            {
                for (int k = 0; k < 10; k++)
                {
                    Console.WriteLine($"SumAsync {k} await Task.Run ManagedThreadId={Thread.CurrentThread.ManagedThreadId}");
                    Thread.Sleep(1000);
                }

                for (long i = 0; i < 999999999; i++)
                {
                    result += i;
                }
            });

            Console.WriteLine($"SumFactory 111   end ManagedThreadId={Thread.CurrentThread.ManagedThreadId}");

            return result;
        }
        #endregion

        /// <summary>
        /// 真的返回Task  不是async  
        /// 
        /// 要使用返回值就一定要等子线程计算完毕
        /// </summary>
        /// <returns>没有async Task</returns>
        private static Task<int> SumFactory()
        {
            Console.WriteLine($"SumFactory 111 start ManagedThreadId={Thread.CurrentThread.ManagedThreadId}");
            TaskFactory taskFactory = new TaskFactory();
            Task<int> iResult = taskFactory.StartNew<int>(() =>
            {
                Thread.Sleep(3000);
                Console.WriteLine($"SumFactory 123 Task.Run ManagedThreadId={Thread.CurrentThread.ManagedThreadId}");
                return 123;
            });
            //Console.WriteLine($"This is {iResult.Result}");
            Console.WriteLine($"SumFactory 111   end ManagedThreadId={Thread.CurrentThread.ManagedThreadId}");
            return iResult;
        }

        public async static Task TestShow()
        {
            Console.WriteLine($"当前主线程id={Thread.CurrentThread.ManagedThreadId.ToString("00")}");
            {
                NoReturnNoAwait();
            }
            {
                NoReturn();
                for (int i = 0; i < 10; i++)
                {
                    Thread.Sleep(300);
                    Console.WriteLine($"Main Thread Task ManagedThreadId={Thread.CurrentThread.ManagedThreadId.ToString("00")} i={i}");
                }
            }
            {
                Task t = NoReturnTask();
                Console.WriteLine($"Main Thread Task ManagedThreadId={Thread.CurrentThread.ManagedThreadId}");
                t.Wait();//主线程等待Task的完成  阻塞的
                await t;//await后的代码会由线程池的线程执行  非阻塞
            }
            {
                Task<long> t = SumAsync();
                Console.WriteLine($"Main Thread Task ManagedThreadId={Thread.CurrentThread.ManagedThreadId}");
                long lResult = t.Result;//访问result   主线程等待Task的完成
                t.Wait();//等价于上一行
            }
            {
                Task<int> t = SumFactory();
                Console.WriteLine($"Main Thread Task ManagedThreadId={Thread.CurrentThread.ManagedThreadId}");
                long lResult = t.Result;//没有await和async 普通的task
                t.Wait();
            }
            Console.WriteLine($"Test Sleep Start {Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(10000);
            Console.WriteLine($"Test Sleep End {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"Main Thread Task ManagedThreadId={Thread.CurrentThread.ManagedThreadId}");
            Console.Read();
        }
        private int iDoTestNum = 0;
        private string Name = "水煮鱼";

    }
}
