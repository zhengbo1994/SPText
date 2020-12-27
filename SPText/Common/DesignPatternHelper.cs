using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPText.Common
{
    /// <summary>
    /// 设计模式帮助类案列   （https://www.cnblogs.com/xuan666/articles/10643017.html）
    /// </summary>
    public class DesignPatternHelper
    {

        public void Show()
        {
            SingletonPatternShow();//单例模式
            SimpleFactoryFactoryShow();//简单工厂
            ApproachFactoryShow(); //工厂方法
            AbstractFactoryPatternShow();//抽象工厂
            BuilderPatternShow();//建造者模式
            PrototypePatternShow();//原型模式
            AdapterPatternShow();//适配器模式
            BridgePatternShow();//桥梁模式
            DecoratorPatternShow();//装饰者模式
            CompositePatternShow();//组合模式
            FacadePatternShow();//外观模式
            FlyweightPatternShow();//享元模式
            ProxyPatternShow();//代理模式
            TemplateMethodPatternShow();//模板方法模式
            CommandPatternShow(); //命令模式
            IteratorPatternShow();//迭代器模式
            ObserverPatternShow();//观察者模式
            MediatorPatternShow();//中介者模式
            StatePatternShow();//状态者模式
            StragetyPatternShow();//策略者模式
            ChainOfResponsibityShow();//责任链模式
            VistorPatternShow();//访问者模式
            MementoPatternShow();//备忘录模式
        }
        #region  调用
        #region  单例模式
        public void SingletonPatternShow()
        {
            SingletonPattern.Singleton0 singleton0 = SingletonPattern.Singleton0.CreateSingletonInstance();
            SingletonPattern.Singleton1 singleton1 = SingletonPattern.Singleton1.CreateSingletonInstance();
            SingletonPattern.Singleton2 singleton2 = SingletonPattern.Singleton2.CreateSingletonInstance();
        }
        #endregion

        #region  简单工厂
        public void SimpleFactoryFactoryShow()
        {

            SimpleFactoryFactory.Food food = SimpleFactoryFactory.FoodSimpleFactory.FoodFactory(SimpleFactoryFactory.EnumSimpleType.EnumTomatoScrambledEggs);
            food.Print();

        }
        #endregion

        #region  工厂方法
        public void ApproachFactoryShow()
        {
            // 初始化做菜的两个工厂（）
            ApproachFactory.Creator shreddedPorkWithPotatoesFactory = new ApproachFactory.ShreddedPorkWithPotatoesFactory();
            ApproachFactory.Creator tomatoScrambledEggsFactory = new ApproachFactory.TomatoScrambledEggsFactory();

            // 开始做西红柿炒蛋
            ApproachFactory.Food tomatoScrambleEggs = tomatoScrambledEggsFactory.CreateFoddFactory();
            tomatoScrambleEggs.Print();

            //开始做土豆肉丝
            ApproachFactory.Food shreddedPorkWithPotatoes = shreddedPorkWithPotatoesFactory.CreateFoddFactory();
            shreddedPorkWithPotatoes.Print();
        }
        #endregion

        #region  抽象工厂
        public void AbstractFactoryPatternShow()
        {
            // 南昌工厂制作南昌的鸭脖和鸭架
            AbstractFactoryPattern.AbstractFactory nanChangFactory = new AbstractFactoryPattern.NanChangFactory();
            AbstractFactoryPattern.YaBo nanChangYabo = nanChangFactory.CreateYaBo();
            nanChangYabo.Print();
            AbstractFactoryPattern.YaJia nanChangYajia = nanChangFactory.CreateYaJia();
            nanChangYajia.Print();

            // 上海工厂制作上海的鸭脖和鸭架
            AbstractFactoryPattern.AbstractFactory shangHaiFactory = new AbstractFactoryPattern.ShangHaiFactory();
            shangHaiFactory.CreateYaBo().Print();
            shangHaiFactory.CreateYaJia().Print();
        }
        #endregion

        #region  建造者模式
        public void BuilderPatternShow()
        {
            // 客户找到电脑城老板说要买电脑，这里要装两台电脑
            // 创建指挥者和构造者
            BuilderPattern.Director director = new BuilderPattern.Director();
            BuilderPattern.Builder b1 = new BuilderPattern.ConcreteBuilder1();
            BuilderPattern.Builder b2 = new BuilderPattern.ConcreteBuilder2();

            // 老板叫员工去组装第一台电脑
            director.Construct(b1);
            // 组装完，组装人员搬来组装好的电脑
            BuilderPattern.Computer computer1 = b1.GetComputer();
            computer1.Show();

            // 老板叫员工去组装第二台电脑
            director.Construct(b2);
            BuilderPattern.Computer computer2 = b2.GetComputer();
            computer2.Show();
        }
        #endregion

        #region  原型模式
        public void PrototypePatternShow()
        {
            // 孙悟空 原型
            PrototypePattern.MonkeyKingPrototype prototypeMonkeyKing = new PrototypePattern.ConcretePrototype("MonkeyKing");

            // 变一个
            PrototypePattern.MonkeyKingPrototype cloneMonkeyKing = prototypeMonkeyKing.Clone() as PrototypePattern.ConcretePrototype;
            Console.WriteLine("Cloned1:\t" + cloneMonkeyKing.Id);

            // 变两个
            PrototypePattern.MonkeyKingPrototype cloneMonkeyKing2 = prototypeMonkeyKing.Clone() as PrototypePattern.ConcretePrototype;
            Console.WriteLine("Cloned2:\t" + cloneMonkeyKing2.Id);
        }
        #endregion

        #region  适配器模式
        public void AdapterPatternShow()
        {
            // 现在客户端可以通过电适配要使用2个孔的插头了
            AdapterPattern.IThreeHole threehole = new AdapterPattern.PowerAdapter();
            threehole.Request();
        }

        #endregion

        #region  桥梁模式
        public void BridgePatternShow()
        {
            // 创建一个遥控器
            BridgePattern.RemoteControl remoteControl = new BridgePattern.ConcreteRemote();
            // 长虹电视机
            remoteControl.Implementor = new BridgePattern.ChangHong();
            remoteControl.On();
            remoteControl.SetChannel();
            remoteControl.Off();
            Console.WriteLine();

            // 三星牌电视机
            remoteControl.Implementor = new BridgePattern.Samsung();
            remoteControl.On();
            remoteControl.SetChannel();
            remoteControl.Off();
        }
        #endregion

        #region  装饰者模式
        public void DecoratorPatternShow()
        {
            // 我买了个苹果手机
            DecoratorPattern.Phone phone = new DecoratorPattern.ApplePhone();

            // 现在想贴膜了
            DecoratorPattern.Decorator applePhoneWithSticker = new DecoratorPattern.Sticker(phone);
            // 扩展贴膜行为
            applePhoneWithSticker.Print();
            Console.WriteLine("----------------------\n");

            // 现在我想有挂件了
            DecoratorPattern.Decorator applePhoneWithAccessories = new DecoratorPattern.Accessories(phone);
            // 扩展手机挂件行为
            applePhoneWithAccessories.Print();
            Console.WriteLine("----------------------\n");

            // 现在我同时有贴膜和手机挂件了
            DecoratorPattern.Sticker sticker = new DecoratorPattern.Sticker(phone);
            DecoratorPattern.Accessories applePhoneWithAccessoriesAndSticker = new DecoratorPattern.Accessories(sticker);
            applePhoneWithAccessoriesAndSticker.Print();
        }
        #endregion

        #region  组合模式
        public void CompositePatternShow()
        {

            /*////安全式的组合模式
            //// 此方式实现的组合模式把管理子对象的方法声明在树枝构件ComplexGraphics类中
            /// 这样如果叶子节点Line、Circle使用了Add或Remove方法时，就能在编译期间出现错误
            //// 但这种方式虽然解决了透明式组合模式的问题，但是它使得叶子节点和树枝构件具有不一样的接口。
            /// 所以这两种方式实现的组合模式各有优缺点，具体使用哪个，可以根据问题的实际情况而定
            /// */
            CompositePattern.ComplexGraphics complexGraphics = new CompositePattern.ComplexGraphics("一个复杂图形和两条线段组成的复杂图形");
            complexGraphics.Add(new CompositePattern.Line("线段A"));
            CompositePattern.ComplexGraphics CompositeCG = new CompositePattern.ComplexGraphics("一个圆和一条线组成的复杂图形");
            CompositeCG.Add(new CompositePattern.Circle("圆"));
            CompositeCG.Add(new CompositePattern.Circle("线段B"));
            complexGraphics.Add(CompositeCG);
            CompositePattern.Line l = new CompositePattern.Line("线段C");
            complexGraphics.Add(l);

            // 显示复杂图形的画法
            Console.WriteLine("复杂图形的绘制如下：");
            Console.WriteLine("---------------------");
            complexGraphics.Draw();
            Console.WriteLine("复杂图形绘制完成");
            Console.WriteLine("---------------------");
            Console.WriteLine();

            // 移除一个组件再显示复杂图形的画法
            complexGraphics.Remove(l);
            Console.WriteLine("移除线段C后，复杂图形的绘制如下：");
            Console.WriteLine("---------------------");
            complexGraphics.Draw();
            Console.WriteLine("复杂图形绘制完成");
            Console.WriteLine("---------------------");
        }
        #endregion

        #region  外观模式
        public void FacadePatternShow()
        {
            FacadePattern.SubSystemA a = new FacadePattern.SubSystemA();
            FacadePattern.SubSystemB b = new FacadePattern.SubSystemB();
            FacadePattern.SubSystemC c = new FacadePattern.SubSystemC();
            a.MethodA();
            b.MethodB();
            c.MethodC();
        }
        #endregion

        #region  享元模式
        public void FlyweightPatternShow()
        {
            // 定义外部状态，例如字母的位置等信息
            int externalstate = 10;
            // 初始化享元工厂
            FlyweightPattern.FlyweightFactory factory = new FlyweightPattern.FlyweightFactory();

            // 判断是否已经创建了字母A，如果已经创建就直接使用创建的对象A
            FlyweightPattern.Flyweight fa = factory.GetFlyweight("A");
            if (fa != null)
            {
                // 把外部状态作为享元对象的方法调用参数
                fa.Operation(--externalstate);
            }
            // 判断是否已经创建了字母B
            FlyweightPattern.Flyweight fb = factory.GetFlyweight("B");
            if (fb != null)
            {
                fb.Operation(--externalstate);
            }
            // 判断是否已经创建了字母C
            FlyweightPattern.Flyweight fc = factory.GetFlyweight("C");
            if (fc != null)
            {
                fc.Operation(--externalstate);
            }
            // 判断是否已经创建了字母D
            FlyweightPattern.Flyweight fd = factory.GetFlyweight("D");
            if (fd != null)
            {
                fd.Operation(--externalstate);
            }
            else
            {
                Console.WriteLine("驻留池中不存在字符串D");
                // 这时候就需要创建一个对象并放入驻留池中
                FlyweightPattern.ConcreteFlyweight d = new FlyweightPattern.ConcreteFlyweight("D");
                factory.flyweights.Add("D", d);
            }
        }
        #endregion

        #region  代理模式
        public void ProxyPatternShow()
        {
            // 创建一个代理对象并发出请求
            ProxyPattern.Person proxy = new ProxyPattern.Friend();
            proxy.BuyProduct();
        }
        #endregion

        #region  模板方法模式
        public void TemplateMethodPatternShow()
        {
            // 创建一个菠菜实例并调用模板方法
            TemplateMethodPattern.Spinach spinach = new TemplateMethodPattern.Spinach();
            spinach.CookVegetabel();
            Console.Read();
        }
        #endregion

        #region  命令模式
        public void CommandPatternShow()
        {
            // 初始化Receiver、Invoke和Command
            CommandPattern.Receiver r = new CommandPattern.Receiver();
            CommandPattern.Command c = new CommandPattern.ConcreteCommand(r);
            CommandPattern.Invoke i = new CommandPattern.Invoke(c);

            // 院领导发出命令
            i.ExecuteCommand();
        }
        #endregion

        #region  迭代器模式
        public void IteratorPatternShow()
        {
            IteratorPattern.Iterator iterator;
            IteratorPattern.IListCollection list = new IteratorPattern.ConcreteList();
            iterator = list.GetIterator();

            while (iterator.MoveNext())
            {
                int i = (int)iterator.GetCurrent();
                Console.WriteLine(i.ToString());
                iterator.Next();
            }
        }
        #endregion

        #region  观察者模式
        public void ObserverPatternShow()
        {
            // 实例化订阅者和订阅号对象
            ObserverPattern.Subscriber LearningHardSub = new ObserverPattern.Subscriber("LearningHard");
            ObserverPattern.TenxunGame txGame = new ObserverPattern.TenxunGame();

            txGame.Subscriber = LearningHardSub;
            txGame.Symbol = "TenXun Game";
            txGame.Info = "Have a new game published ....";

            txGame.Update();
        }
        #endregion

        #region  中介者模式
        public void MediatorPatternShow()
        {
            MediatorPattern.AbstractCardPartner A = new MediatorPattern.ParterA();
            A.MoneyCount = 20;
            MediatorPattern.AbstractCardPartner B = new MediatorPattern.ParterB();
            B.MoneyCount = 20;

            // A 赢了则B的钱就减少
            A.ChangeCount(5, B);
            Console.WriteLine("A 现在的钱是：{0}", A.MoneyCount);// 应该是25
            Console.WriteLine("B 现在的钱是：{0}", B.MoneyCount); // 应该是15

            // B赢了A的钱也减少
            B.ChangeCount(10, A);
            Console.WriteLine("A 现在的钱是：{0}", A.MoneyCount); // 应该是15
            Console.WriteLine("B 现在的钱是：{0}", B.MoneyCount); // 应该是25
        }
        #endregion

        #region  状态者模式
        public void StatePatternShow()
        {
            // 开一个新的账户
            StatePattern.Account account = new StatePattern.Account("Learning Hard");

            // 进行交易
            // 存钱
            account.Deposit(1000.0);
            account.Deposit(200.0);
            account.Deposit(600.0);

            // 付利息
            account.PayInterest();

            // 取钱
            account.Withdraw(2000.00);
            account.Withdraw(500.00);

            // 等待用户输入
            Console.ReadKey();
        }
        #endregion

        #region  策略者模式
        public void StragetyPatternShow()
        {
            // 个人所得税方式
            StragetyPattern.InterestOperation operation = new StragetyPattern.InterestOperation(new StragetyPattern.PersonalTaxStrategy());
            Console.WriteLine("个人支付的税为：{0}", operation.GetTax(5000.00));

            // 企业所得税
            operation = new StragetyPattern.InterestOperation(new StragetyPattern.EnterpriseTaxStrategy());
            Console.WriteLine("企业支付的税为：{0}", operation.GetTax(50000.00));
        }
        #endregion

        #region  责任链模式
        public void ChainOfResponsibityShow()
        {
            ChainOfResponsibity.PurchaseRequest requestTelphone = new ChainOfResponsibity.PurchaseRequest(4000.0, "Telphone");
            ChainOfResponsibity.PurchaseRequest requestSoftware = new ChainOfResponsibity.PurchaseRequest(10000.0, "Visual Studio");
            ChainOfResponsibity.PurchaseRequest requestComputers = new ChainOfResponsibity.PurchaseRequest(40000.0, "Computers");

            ChainOfResponsibity.Approver manager = new ChainOfResponsibity.Manager("LearningHard");
            ChainOfResponsibity.Approver Vp = new ChainOfResponsibity.VicePresident("Tony");
            ChainOfResponsibity.Approver Pre = new ChainOfResponsibity.President("BossTom");

            // 设置责任链
            manager.NextApprover = Vp;
            Vp.NextApprover = Pre;

            // 处理请求
            manager.ProcessRequest(requestTelphone);
            manager.ProcessRequest(requestSoftware);
            manager.ProcessRequest(requestComputers);
        }
        #endregion

        #region  访问者模式
        public void VistorPatternShow()
        {
            VistorPattern.ObjectStructure objectStructure = new VistorPattern.ObjectStructure();
            // 遍历对象结构中的对象集合，访问每个元素的Print方法打印元素信息
            foreach (VistorPattern.Element e in objectStructure.Elements)
            {
                e.Print();
            }
        }
        #endregion

        #region  备忘录模式
        public void MementoPatternShow()
        {
            List<MementoPattern.ContactPerson> persons = new List<MementoPattern.ContactPerson>()
            {
                new MementoPattern.ContactPerson() { Name= "Learning Hard", MobileNum = "123445"},
                new MementoPattern.ContactPerson() { Name = "Tony", MobileNum = "234565"},
                new MementoPattern.ContactPerson() { Name = "Jock", MobileNum = "231455"}
            };
            MementoPattern.MobileOwner mobileOwner = new MementoPattern.MobileOwner(persons);
            mobileOwner.Show();

            // 创建备忘录并保存备忘录对象
            MementoPattern.Caretaker caretaker = new MementoPattern.Caretaker();
            caretaker.ContactM = mobileOwner.CreateMemento();

            // 更改发起人联系人列表
            Console.WriteLine("----移除最后一个联系人--------");
            mobileOwner.ContactPersons.RemoveAt(2);
            mobileOwner.Show();

            // 恢复到原始状态
            Console.WriteLine("-------恢复联系人列表------");
            mobileOwner.RestoreMemento(caretaker.ContactM);
            mobileOwner.Show();
        }
        #endregion
        #endregion


        #region  单例
        public class SingletonPattern
        {

            public sealed class Singleton0
            {
                // 定义一个静态变量来保存类的实例
                private static Singleton0 uniqueInstance = null;
                static Singleton0()
                {
                    uniqueInstance = new Singleton0();
                }
                public static Singleton0 CreateSingletonInstance()
                {
                    return uniqueInstance;
                }
            }

            public sealed class Singleton1
            {
                public static Singleton1 singleton1 = new Singleton1();
                public static Singleton1 CreateSingletonInstance()
                {
                    return singleton1;
                }
            }

            public sealed class Singleton2
            {
                // 定义一个静态变量来保存类的实例
                private static Singleton2 uniqueInstance = null;
                private static readonly object lock_obj = new object();

                public static Singleton2 CreateSingletonInstance()
                {
                    if (uniqueInstance == null)
                    {
                        lock (lock_obj)
                        {
                            if (uniqueInstance == null)
                            {
                                uniqueInstance = new Singleton2();
                            }
                        }
                    }
                    return uniqueInstance;
                }
            }
        }
        #endregion

        #region  简单工厂
        public class SimpleFactoryFactory
        {
            public class FoodSimpleFactory
            {
                static Food getFood = null;
                public static Food FoodFactory(EnumSimpleType type)
                {
                    if (EnumSimpleType.EnumShreddedPorkWithPotatoes.ToString() == type.ToString())
                    {
                        getFood = new ShreddedPorkWithPotatoes();
                    }
                    else if (EnumSimpleType.EnumTomatoScrambledEggs.ToString() == type.ToString())
                    {
                        getFood = new TomatoScrambledEggs();
                    }
                    return getFood;
                }
            }

            public class TomatoScrambledEggs : Food
            {
                public override void Print()
                {
                    Console.WriteLine("一份西红柿炒蛋！");
                }
            }
            public class ShreddedPorkWithPotatoes : Food
            {
                public override void Print()
                {
                    Console.WriteLine("一份土豆肉丝");
                }
            }


            public abstract class Food
            {
                public abstract void Print();
            }

            public enum EnumSimpleType
            {
                EnumTomatoScrambledEggs,
                EnumShreddedPorkWithPotatoes
            }
        }
        #endregion

        #region  工厂方法
        public class ApproachFactory
        {
            /// <summary>
            /// 抽象类
            /// </summary>
            public abstract class Food
            {
                public abstract void Print();
            }
            /// <summary>
            /// 西红柿炒鸡蛋这道菜
            /// </summary>
            public class TomatoScrambledEggs : Food
            {
                public override void Print()
                {
                    Console.WriteLine("西红柿炒蛋好了！");
                }
            }
            /// <summary>
            /// 土豆肉丝这道菜
            /// </summary>
            public class ShreddedPorkWithPotatoes : Food
            {
                public override void Print()
                {
                    Console.WriteLine("土豆肉丝好了");
                }
            }
            /// <summary>
            /// 抽象工厂类
            /// </summary>
            public abstract class Creator
            {
                // 工厂方法
                public abstract Food CreateFoddFactory();
            }
            /// <summary>
            /// 西红柿炒蛋工厂类
            /// </summary>
            public class TomatoScrambledEggsFactory : Creator
            {
                /// <summary>
                /// 负责创建西红柿炒蛋这道菜
                /// </summary>
                /// <returns></returns>
                public override Food CreateFoddFactory()
                {
                    return new TomatoScrambledEggs();
                }
            }
            /// <summary>
            /// 土豆肉丝工厂类
            /// </summary>
            public class ShreddedPorkWithPotatoesFactory : Creator
            {
                /// <summary>
                /// 负责创建土豆肉丝这道菜
                /// </summary>
                /// <returns></returns>
                public override Food CreateFoddFactory()
                {
                    return new ShreddedPorkWithPotatoes();
                }
            }
        }
        #endregion

        #region  抽象工厂
        public class AbstractFactoryPattern
        {
            public abstract class YaBo
            {
                public abstract void Print();
            }
            public abstract class YaJia
            {
                public abstract void Print();
            }
            public class NanChangYaBo : YaBo
            {
                public override void Print()
                {
                    Console.WriteLine("南昌的鸭脖");
                }
            }
            public class ShangHaiYaBo : YaBo
            {
                public override void Print()
                {
                    Console.WriteLine("上海的鸭脖");
                }
            }
            public class NanChangYaJia : YaJia
            {
                public override void Print()
                {
                    Console.WriteLine("南昌的鸭架子");
                }
            }
            public class ShangHaiYaJia : YaJia
            {
                public override void Print()
                {
                    Console.WriteLine("上海的鸭架子");
                }
            }

            public abstract class AbstractFactory
            {
                // 抽象工厂提供创建一系列产品的接口，这里作为例子，只给出了绝味中鸭脖和鸭架的创建接口
                public abstract YaBo CreateYaBo();
                public abstract YaJia CreateYaJia();
            }
            public class ShangHaiFactory : AbstractFactory
            {
                // 制作上海鸭脖
                public override YaBo CreateYaBo()
                {
                    return new ShangHaiYaBo();
                }
                // 制作上海鸭架
                public override YaJia CreateYaJia()
                {
                    return new ShangHaiYaJia();
                }
            }


            public class NanChangFactory : AbstractFactory
            {
                // 制作南昌鸭脖
                public override YaBo CreateYaBo()
                {
                    return new NanChangYaBo();
                }
                // 制作南昌鸭架
                public override YaJia CreateYaJia()
                {
                    return new NanChangYaJia();
                }
            }
        }
        #endregion

        #region  建造者模式
        public abstract class BuilderPattern
        {
            public class Director
            {
                public void Construct(Builder builder)
                {
                    builder.BuildPartCPU();
                    builder.BuildPartMainBoard();
                }
            }

            public abstract class Builder
            {
                // 装CPU
                public abstract void BuildPartCPU();
                // 装主板
                public abstract void BuildPartMainBoard();
                // 获得组装好的电脑
                public abstract Computer GetComputer();
            }


            public class Computer
            {
                // 电脑组件集合
                private IList<string> parts = new List<string>();

                // 把单个组件添加到电脑组件集合中
                public void Add(string part)
                {
                    parts.Add(part);
                }
                public void Show()
                {
                    Console.WriteLine("电脑开始在组装.......");
                    foreach (string part in parts)
                    {
                        Console.WriteLine("组件" + part + "已装好");
                    }

                    Console.WriteLine("电脑组装好了");
                }
            }

            public class ConcreteBuilder1 : Builder
            {
                Computer computer = new Computer();
                public override void BuildPartCPU()
                {
                    computer.Add("CPU1");
                }

                public override void BuildPartMainBoard()
                {
                    computer.Add("Main board1");
                }

                public override Computer GetComputer()
                {
                    return computer;
                }
            }
            public class ConcreteBuilder2 : Builder
            {
                Computer computer = new Computer();
                public override void BuildPartCPU()
                {
                    computer.Add("CPU2");
                }

                public override void BuildPartMainBoard()
                {
                    computer.Add("Main board2");
                }

                public override Computer GetComputer()
                {
                    return computer;
                }
            }
        }
        #endregion

        #region  原型模式
        public class PrototypePattern
        {
            public abstract class MonkeyKingPrototype
            {
                public string Id { get; set; }
                public MonkeyKingPrototype(string _Id)
                {
                    this.Id = _Id;
                }
                public abstract MonkeyKingPrototype Clone();
            }

            public class ConcretePrototype : MonkeyKingPrototype
            {
                public ConcretePrototype(string Id) : base(Id) { }
                public override MonkeyKingPrototype Clone()
                {
                    return (MonkeyKingPrototype)this.MemberwiseClone();
                }
            }
        }
        #endregion

        #region  适配器模式
        public class AdapterPattern
        {
            /// <summary>
            /// 三个孔的插头，也就是适配器模式中的目标角色
            /// </summary>
            public interface IThreeHole
            {
                void Request();
            }

            /// <summary>
            /// 两个孔的插头，源角色——需要适配的类
            /// </summary>
            public abstract class TwoHole
            {
                public void SpecificRequest()
                {
                    Console.WriteLine("我是两个孔的插头");
                }
            }

            /// <summary>
            /// 适配器类，接口要放在类的后面
            /// 适配器类提供了三个孔插头的行为，但其本质是调用两个孔插头的方法
            /// </summary>
            public class PowerAdapter : TwoHole, IThreeHole
            {
                /// <summary>
                /// 实现三个孔插头接口方法
                /// </summary>
                public void Request()
                {
                    // 调用两个孔插头方法
                    this.SpecificRequest();
                }
            }
        }
        #endregion

        #region  桥梁模式
        public class BridgePattern
        {
            /// <summary>
            /// 电视机，提供抽象方法
            /// </summary>
            public abstract class TV
            {
                public abstract void On();
                public abstract void Off();
                public abstract void tuneChannel();
            }
            /// <summary>
            /// 长虹牌电视机，重写基类的抽象方法
            /// 提供具体的实现
            public class ChangHong : TV
            {
                public override void Off()
                {
                    Console.WriteLine("长虹牌电视机已经关掉了");
                }

                public override void On()
                {
                    Console.WriteLine("长虹牌电视机已经打开了");
                }

                public override void tuneChannel()
                {
                    Console.WriteLine("长虹牌电视机换频道");
                }
            }

            /// <summary>
            /// 三星牌电视机，重写基类的抽象方法
            /// </summary>
            public class Samsung : TV
            {
                public override void Off()
                {
                    Console.WriteLine("三星牌电视机已经关掉了");
                }

                public override void On()
                {
                    Console.WriteLine("三星牌电视机已经打开了");
                }

                public override void tuneChannel()
                {
                    Console.WriteLine("三星牌电视机换频道");
                }
            }

            /// <summary>
            /// 抽象概念中的遥控器，扮演抽象化角色
            /// </summary>
            public class RemoteControl
            {
                // 字段
                private TV implementor;
                // 属性
                public TV Implementor
                {
                    get { return implementor; }
                    set { implementor = value; }
                }

                /// <summary>
                /// 开电视机，这里抽象类中不再提供实现了，而是调用实现类中的实现
                /// </summary>
                public virtual void On()
                {
                    implementor.On();
                }

                /// <summary>
                /// 关电视机
                /// </summary>
                public virtual void Off()
                {
                    implementor.Off();
                }

                /// <summary>
                /// 换频道
                /// </summary>
                public virtual void SetChannel()
                {
                    implementor.tuneChannel();
                }
            }

            public class ConcreteRemote : RemoteControl
            {
                public override void SetChannel()
                {
                    Console.WriteLine("---------------------");
                    base.SetChannel();
                    Console.WriteLine("---------------------");
                }
            }
        }
        #endregion

        #region  装饰者模式
        public class DecoratorPattern
        {
            /// <summary>
            /// 手机抽象类，即装饰者模式中的抽象组件类
            /// </summary>
            public abstract class Phone
            {
                public abstract void Print();
            }

            /// <summary>
            /// 苹果手机，即装饰着模式中的具体组件类
            /// </summary>
            public class ApplePhone : Phone
            {
                /// <summary>
                /// 重写基类方法
                /// </summary>
                public override void Print()
                {
                    Console.WriteLine("开始执行具体的对象——苹果手机");
                }
            }

            /// <summary>
            /// 装饰抽象类,要让装饰完全取代抽象组件，所以必须继承自Photo
            /// </summary>
            public abstract class Decorator : Phone
            {
                public Phone phone { get; set; }
                public Decorator(Phone _phone)
                {
                    phone = _phone;
                }
                public override void Print()
                {
                    if (phone != null)
                    {
                        phone.Print();
                    }
                }
            }

            /// <summary>
            /// 贴膜，即具体装饰者
            /// </summary>
            public class Sticker : Decorator
            {
                public Sticker(Phone phonep) : base(phonep)
                {
                }

                /// <summary>
                /// 新的行为方法
                /// </summary>
                public void AddSticker()
                {
                    Console.WriteLine("现在苹果手机有贴膜了");
                }

                public override void Print()
                {
                    base.Print();

                    AddSticker();
                }
            }

            /// <summary>
            /// 手机挂件
            /// </summary>
            public class Accessories : Decorator
            {
                public Accessories(Phone phone) : base(phone)
                {
                }

                /// <summary>
                /// 新的行为方法
                /// </summary>
                public void AddAccessories()
                {
                    Console.WriteLine("现在苹果手机有漂亮的挂件了");
                }

                public override void Print()
                {
                    base.Print();

                    // 添加新的行为
                    AddAccessories();
                }
            }
        }
        #endregion

        #region   组合模式
        public class CompositePattern
        {
            /// <summary>
            /// 图形抽象类，
            /// </summary>
            public abstract class Graphics
            {
                public string Name { get; set; }
                public Graphics(string name)
                {
                    this.Name = name;
                }

                public abstract void Draw();
                public abstract void Add(Graphics g);
                public abstract void Remove(Graphics g);
            }

            public class Line : Graphics
            {
                public Line(string name) : base(name)
                { }

                // 重写父类抽象方法
                public override void Draw()
                {
                    Console.WriteLine("画  " + Name);
                }
                // 因为简单图形在添加或移除其他图形，所以简单图形Add或Remove方法没有任何意义
                // 如果客户端调用了简单图形的Add或Remove方法将会在运行时抛出异常
                // 我们可以在客户端捕获该类移除并处理
                public override void Add(Graphics g)
                {
                    throw new Exception("不能向简单图形Line添加其他图形");
                }
                public override void Remove(Graphics g)
                {
                    throw new Exception("不能向简单图形Line移除其他图形");
                }
            }
            /// <summary>
            /// 简单图形类——圆
            /// </summary>
            public class Circle : Graphics
            {
                public Circle(string name) : base(name)
                { }

                // 重写父类抽象方法
                public override void Draw()
                {
                    Console.WriteLine("画  " + Name);
                }

                public override void Add(Graphics g)
                {
                    throw new Exception("不能向简单图形Circle添加其他图形");
                }
                public override void Remove(Graphics g)
                {
                    throw new Exception("不能向简单图形Circle移除其他图形");
                }
            }

            /// <summary>
            /// 复杂图形，由一些简单图形组成,这里假设该复杂图形由一个圆两条线组成的复杂图形
            /// </summary>
            public class ComplexGraphics : Graphics
            {
                private List<Graphics> complexGraphicsList = new List<Graphics>();

                public ComplexGraphics(string name)
                    : base(name)
                { }

                /// <summary>
                /// 复杂图形的画法
                /// </summary>
                public override void Draw()
                {
                    foreach (Graphics g in complexGraphicsList)
                    {
                        g.Draw();
                    }
                }

                public override void Add(Graphics g)
                {
                    complexGraphicsList.Add(g);
                }
                public override void Remove(Graphics g)
                {
                    complexGraphicsList.Remove(g);
                }
            }
        }
        #endregion

        #region  外观模式
        public class FacadePattern
        {
            // 子系统A
            public class SubSystemA
            {
                public void MethodA()
                {
                    Console.WriteLine("执行子系统A中的方法A");
                }
            }

            // 子系统B
            public class SubSystemB
            {
                public void MethodB()
                {
                    Console.WriteLine("执行子系统B中的方法B");
                }
            }

            // 子系统C
            public class SubSystemC
            {
                public void MethodC()
                {
                    Console.WriteLine("执行子系统C中的方法C");
                }
            }

            // 外观类
            public class RegistrationFacade
            {
                private RegisterCourse registerCourse;
                private NotifyStudent notifyStu;
                public RegistrationFacade()
                {
                    registerCourse = new RegisterCourse();
                    notifyStu = new NotifyStudent();
                }

                public bool RegisterCourse(string courseName, string studentName)
                {
                    if (!registerCourse.CheckAvailable(courseName))
                    {
                        return false;
                    }

                    return notifyStu.Notify(studentName);
                }
            }

            #region 子系统
            // 相当于子系统A
            public class RegisterCourse
            {
                public bool CheckAvailable(string courseName)
                {
                    Console.WriteLine("正在验证课程 {0}是否人数已满", courseName);
                    return true;
                }
            }

            // 相当于子系统B
            public class NotifyStudent
            {
                public bool Notify(string studentName)
                {
                    Console.WriteLine("正在向{0}发生通知", studentName);
                    return true;
                }
            }
            #endregion
        }
        #endregion

        #region  享元模式
        public class FlyweightPattern
        {
            /// <summary>
            ///  抽象享元类，提供具体享元类具有的方法
            /// </summary>
            public abstract class Flyweight
            {
                public abstract void Operation(int extrinsicstate);
            }

            public class ConcreteFlyweight : Flyweight
            {
                // 内部状态
                private string intrinsicstate;
                // 构造函数
                public ConcreteFlyweight(string innerState)
                {
                    this.intrinsicstate = innerState;
                }
                /// <summary>
                /// 享元类的实例方法
                /// </summary>
                /// <param name="extrinsicstate">外部状态</param>
                public override void Operation(int extrinsicstate)
                {
                    Console.WriteLine("具体实现类: intrinsicstate {0}, extrinsicstate {1}", intrinsicstate, extrinsicstate);
                }
            }
            public class FlyweightFactory
            {
                // 最好使用泛型Dictionary<string,Flyweighy>
                //public Dictionary<string, Flyweight> flyweights = new Dictionary<string, Flyweight>();
                public Hashtable flyweights = new Hashtable();
                public FlyweightFactory()
                {
                    flyweights.Add("A", new ConcreteFlyweight("A"));
                    flyweights.Add("B", new ConcreteFlyweight("B"));
                    flyweights.Add("C", new ConcreteFlyweight("C"));
                }
                public Flyweight GetFlyweight(string key)
                {
                    // 更好的实现如下
                    //Flyweight flyweight = flyweights[key] as Flyweight;
                    //if (flyweight == null)
                    //{
                    // Console.WriteLine("驻留池中不存在字符串" + key);
                    // flyweight = new ConcreteFlyweight(key);
                    //}
                    //return flyweight;

                    return flyweights[key] as Flyweight;
                }
            }
        }
        #endregion

        #region  代理模式
        public class ProxyPattern
        {
            // 抽象主题角色
            public abstract class Person
            {
                public abstract void BuyProduct();
            }

            //真实主题角色
            public class RealBuyPerson : Person
            {
                public override void BuyProduct()
                {
                    Console.WriteLine("帮我买一个IPhone和一台苹果电脑");
                }
            }

            public class Friend : Person
            {

                // 引用真实主题实例
                RealBuyPerson realSubject;

                public override void BuyProduct()
                {
                    Console.WriteLine("通过代理类访问真实实体对象的方法");
                    if (realSubject == null)
                    {
                        realSubject = new RealBuyPerson();
                    }

                    this.PreBuyProduct();
                    // 调用真实主题方法
                    realSubject.BuyProduct();
                    this.PostBuyProduct();
                }
                // 代理角色执行的一些操作
                public void PreBuyProduct()
                {
                    // 可能不知一个朋友叫这位朋友带东西，首先这位出国的朋友要对每一位朋友要带的东西列一个清单等
                    Console.WriteLine("我怕弄糊涂了，需要列一张清单，张三：要带相机，李四：要带Iphone...........");
                }
                // 买完东西之后，代理角色需要针对每位朋友需要的对买来的东西进行分类
                public void PostBuyProduct()
                {
                    Console.WriteLine("终于买完了，现在要对东西分一下，相机是张三的；Iphone是李四的..........");
                }
            }
        }
        #endregion

        #region  模板方法模式
        public class TemplateMethodPattern
        {
            public abstract class Vegetabel
            {
                // 模板方法，不要把模版方法定义为Virtual或abstract方法，避免被子类重写，防止更改流程的执行顺序
                public void CookVegetabel()
                {
                    Console.WriteLine("抄蔬菜的一般做法");
                    this.pourOil();
                    this.heatOil();
                    this.pourVegetable();
                    this.stir_fry();
                }
                // 第一步倒油
                public void pourOil()
                {
                    Console.WriteLine("倒油");
                }
                // 把油烧热
                public void heatOil()
                {
                    Console.WriteLine("把油烧热");
                }
                // 油热了之后倒蔬菜下去，具体哪种蔬菜由子类决定
                public abstract void pourVegetable();

                // 开发翻炒蔬菜
                public void stir_fry()
                {
                    Console.WriteLine("翻炒");
                }
            }

            public class Spinach : Vegetabel
            {
                public override void pourVegetable()
                {
                    Console.WriteLine("倒菠菜进锅中");
                }
            }
            public class ChineseCabbage : Vegetabel
            {
                public override void pourVegetable()
                {
                    Console.WriteLine("倒大白菜进锅中");
                }
            }
        }
        #endregion

        #region  命令模式
        public class CommandPattern
        {
            // 命令接收者——学生
            public class Receiver
            {
                public void Run1000Meters()
                {
                    Console.WriteLine("跑1000米");
                }
            }
            public abstract class Command
            {
                // 命令应该知道接收者是谁，所以有Receiver这个成员变量
                protected Receiver _receiver;
                public Command(Receiver receiver)
                {
                    this._receiver = receiver;
                }
                public abstract void Action();
            }

            public class Invoke
            {
                public Command _command;
                public Invoke(Command command)
                {
                    this._command = command;
                }
                public void ExecuteCommand()
                {
                    _command.Action();
                }
            }
            public class ConcreteCommand : Command
            {
                public ConcreteCommand(Receiver receiver) : base(receiver) { }

                public override void Action()
                {
                    // 调用接收的方法，因为执行命令的是学生
                    _receiver.Run1000Meters();
                }
            }

        }
        #endregion

        #region  迭代器模式
        public class IteratorPattern
        {
            public interface Iterator
            {
                bool MoveNext();
                Object GetCurrent();
                void Next();
                void Reset();
            }
            // 抽象聚合类
            public interface IListCollection
            {
                Iterator GetIterator();
            }
            // 具体迭代器类
            public class ConcreteIterator : Iterator
            {
                // 迭代器要集合对象进行遍历操作，自然就需要引用集合对象
                private ConcreteList _list;
                private int _index;

                public ConcreteIterator(ConcreteList list)
                {
                    _list = list;
                    _index = 0;
                }


                public bool MoveNext()
                {
                    if (_index < _list.Length)
                    {
                        return true;
                    }
                    return false;
                }

                public Object GetCurrent()
                {
                    return _list.GetElement(_index);
                }

                public void Reset()
                {
                    _index = 0;
                }

                public void Next()
                {
                    if (_index < _list.Length)
                    {
                        _index++;
                    }

                }
            }
            // 具体聚合类
            public class ConcreteList : IListCollection
            {
                int[] collection;
                public ConcreteList()
                {
                    collection = new int[] { 2, 4, 6, 8 };
                }

                public Iterator GetIterator()
                {
                    return new ConcreteIterator(this);
                }

                public int Length
                {
                    get { return collection.Length; }
                }

                public int GetElement(int index)
                {
                    return collection[index];
                }
            }

        }
        #endregion

        #region  观察者模式
        public class ObserverPattern
        {
            public class Subscriber
            {
                public string Name { get; set; }
                public Subscriber(string name)
                {
                    this.Name = name;
                }

                public void ReceiveAndPrintData(TenxunGame txGame)
                {
                    Console.WriteLine("Notified {0} of {1}'s" + " Info is: {2}", Name, txGame.Symbol, txGame.Info);
                }
            }
            public class TenxunGame
            {
                // 订阅者对象
                public Subscriber Subscriber { get; set; }

                public String Symbol { get; set; }

                public string Info { get; set; }

                public void Update()
                {
                    if (Subscriber != null)
                    {
                        // 调用订阅者对象来通知订阅者
                        Subscriber.ReceiveAndPrintData(this);
                    }
                }
            }
        }
        #endregion

        #region  中介者模式

        public class MediatorPattern
        {
            // 抽象牌友类
            public abstract class AbstractCardPartner
            {
                public int MoneyCount { get; set; }

                public AbstractCardPartner()
                {
                    MoneyCount = 0;
                }

                public abstract void ChangeCount(int Count, AbstractCardPartner other);
            }

            // 牌友A类
            public class ParterA : AbstractCardPartner
            {
                public override void ChangeCount(int Count, AbstractCardPartner other)
                {
                    this.MoneyCount += Count;
                    other.MoneyCount -= Count;
                }
            }

            // 牌友B类
            public class ParterB : AbstractCardPartner
            {
                public override void ChangeCount(int Count, AbstractCardPartner other)
                {
                    this.MoneyCount += Count;
                    other.MoneyCount -= Count;
                }
            }
        }
        #endregion

        #region  状态者模式
        public class StatePattern
        {
            // 抽象状态类
            public abstract class State
            {
                // Properties
                public Account Account { get; set; }
                public double Balance { get; set; } // 余额
                public double Interest { get; set; } // 利率
                public double LowerLimit { get; set; } // 下限
                public double UpperLimit { get; set; } // 上限

                public abstract void Deposit(double amount); // 存款
                public abstract void Withdraw(double amount); // 取钱
                public abstract void PayInterest(); // 获得的利息
            }

            public class RedState : State
            {
                public RedState(State state)
                {
                    // Initialize
                    this.Balance = state.Balance;
                    this.Account = state.Account;
                    Interest = 0.00;
                    LowerLimit = -100.00;
                    UpperLimit = 0.00;
                }

                // 存款
                public override void Deposit(double amount)
                {
                    Balance += amount;
                    StateChangeCheck();
                }
                // 取钱
                public override void Withdraw(double amount)
                {
                    Console.WriteLine("没有钱可以取了！");
                }

                public override void PayInterest()
                {
                    // 没有利息
                }

                private void StateChangeCheck()
                {
                    if (Balance > UpperLimit)
                    {
                        Account.State = new SilverState(this);
                    }
                }
            }

            // Silver State意味着没有利息得
            public class SilverState : State
            {
                public SilverState(State state)
                    : this(state.Balance, state.Account)
                {
                }

                public SilverState(double balance, Account account)
                {
                    this.Balance = balance;
                    this.Account = account;
                    Interest = 0.00;
                    LowerLimit = 0.00;
                    UpperLimit = 1000.00;
                }

                public override void Deposit(double amount)
                {
                    Balance += amount;
                    StateChangeCheck();
                }
                public override void Withdraw(double amount)
                {
                    Balance -= amount;
                    StateChangeCheck();
                }

                public override void PayInterest()
                {
                    Balance += Interest * Balance;
                    StateChangeCheck();
                }

                private void StateChangeCheck()
                {
                    if (Balance < LowerLimit)
                    {
                        Account.State = new RedState(this);
                    }
                    else if (Balance > UpperLimit)
                    {
                        Account.State = new GoldState(this);
                    }
                }
            }

            // Gold State意味着有利息状态
            public class GoldState : State
            {
                public GoldState(State state)
                {
                    this.Balance = state.Balance;
                    this.Account = state.Account;
                    Interest = 0.05;
                    LowerLimit = 1000.00;
                    UpperLimit = 1000000.00;
                }
                // 存钱
                public override void Deposit(double amount)
                {
                    Balance += amount;
                    StateChangeCheck();
                }
                // 取钱
                public override void Withdraw(double amount)
                {
                    Balance -= amount;
                    StateChangeCheck();
                }
                public override void PayInterest()
                {
                    Balance += Interest * Balance;
                    StateChangeCheck();
                }

                private void StateChangeCheck()
                {
                    if (Balance < 0.0)
                    {
                        Account.State = new RedState(this);
                    }
                    else if (Balance < LowerLimit)
                    {
                        Account.State = new SilverState(this);
                    }
                }
            }

            public class Account
            {
                public State State { get; set; }
                public string Owner { get; set; }
                public Account(string owner)
                {
                    this.Owner = owner;
                    this.State = new SilverState(0.0, this);
                }


                public double Balance { get { return State.Balance; } } // 余额
                                                                        // 存钱
                public void Deposit(double amount)
                {
                    State.Deposit(amount);
                    Console.WriteLine("存款金额为 {0:C}——", amount);
                    Console.WriteLine("账户余额为 =:{0:C}", this.Balance);
                    Console.WriteLine("账户状态为: {0}", this.State.GetType().Name);
                    Console.WriteLine();
                }

                // 取钱
                public void Withdraw(double amount)
                {
                    State.Withdraw(amount);
                    Console.WriteLine("取款金额为 {0:C}——", amount);
                    Console.WriteLine("账户余额为 =:{0:C}", this.Balance);
                    Console.WriteLine("账户状态为: {0}", this.State.GetType().Name);
                    Console.WriteLine();
                }

                // 获得利息
                public void PayInterest()
                {
                    State.PayInterest();
                    Console.WriteLine("Interest Paid --- ");
                    Console.WriteLine("账户余额为 =:{0:C}", this.Balance);
                    Console.WriteLine("账户状态为: {0}", this.State.GetType().Name);
                    Console.WriteLine();
                }
            }
        }
        #endregion

        #region  策略模式
        public class StragetyPattern
        {
            // 所得税计算策略
            public interface ITaxStragety
            {
                double CalculateTax(double income);
            }

            // 个人所得税
            public class PersonalTaxStrategy : ITaxStragety
            {
                public double CalculateTax(double income)
                {
                    return income * 0.12;
                }
            }

            // 企业所得税
            public class EnterpriseTaxStrategy : ITaxStragety
            {
                public double CalculateTax(double income)
                {
                    return (income - 3500) > 0 ? (income - 3500) * 0.045 : 0.0;
                }
            }

            public class InterestOperation
            {
                private ITaxStragety m_strategy;
                public InterestOperation(ITaxStragety strategy)
                {
                    this.m_strategy = strategy;
                }

                public double GetTax(double income)
                {
                    return m_strategy.CalculateTax(income);
                }
            }
        }
        #endregion

        #region  责任链模式
        public class ChainOfResponsibity
        {
            // 采购请求
            public class PurchaseRequest
            {
                // 金额
                public double Amount { get; set; }
                // 产品名字
                public string ProductName { get; set; }
                public PurchaseRequest(double amount, string productName)
                {
                    Amount = amount;
                    ProductName = productName;
                }
            }
            // 审批人,Handler
            public abstract class Approver
            {
                public Approver NextApprover { get; set; }
                public string Name { get; set; }
                public Approver(string name)
                {
                    this.Name = name;
                }
                public abstract void ProcessRequest(PurchaseRequest request);
            }
            // ConcreteHandler
            public class Manager : Approver
            {
                public Manager(string name)
                    : base(name)
                { }

                public override void ProcessRequest(PurchaseRequest request)
                {
                    if (request.Amount < 10000.0)
                    {
                        Console.WriteLine("{0}-{1} approved the request of purshing {2}", this, Name, request.ProductName);
                    }
                    else if (NextApprover != null)
                    {
                        NextApprover.ProcessRequest(request);
                    }
                }
            }
            // ConcreteHandler,副总
            public class VicePresident : Approver
            {
                public VicePresident(string name)
                    : base(name)
                {
                }
                public override void ProcessRequest(PurchaseRequest request)
                {
                    if (request.Amount < 25000.0)
                    {
                        Console.WriteLine("{0}-{1} approved the request of purshing {2}", this, Name, request.ProductName);
                    }
                    else if (NextApprover != null)
                    {
                        NextApprover.ProcessRequest(request);
                    }
                }
            }
            // ConcreteHandler，总经理
            public class President : Approver
            {
                public President(string name)
                    : base(name)
                { }
                public override void ProcessRequest(PurchaseRequest request)
                {
                    if (request.Amount < 100000.0)
                    {
                        Console.WriteLine("{0}-{1} approved the request of purshing {2}", this, Name, request.ProductName);
                    }
                    else
                    {
                        Console.WriteLine("Request需要组织一个会议讨论");
                    }
                }
            }
        }
        #endregion

        #region  访问者模式
        public class VistorPattern
        {
            // 抽象元素角色
            public abstract class Element
            {
                public abstract void Print();
            }

            // 具体元素A
            public class ElementA : Element
            {
                public override void Print()
                {
                    Console.WriteLine("我是元素A");
                }
            }

            // 具体元素B
            public class ElementB : Element
            {
                public override void Print()
                {
                    Console.WriteLine("我是元素B");
                }
            }

            // 对象结构
            public class ObjectStructure
            {
                private ArrayList elements = new ArrayList();

                public ArrayList Elements
                {
                    get { return elements; }
                }

                public ObjectStructure()
                {
                    Random ran = new Random();
                    for (int i = 0; i < 6; i++)
                    {
                        int ranNum = ran.Next(10);
                        if (ranNum > 5)
                        {
                            elements.Add(new ElementA());
                        }
                        else
                        {
                            elements.Add(new ElementB());
                        }
                    }
                }
            }
        }
        #endregion

        #region  备忘录模式
        public class MementoPattern
        {
            // 联系人
            public class ContactPerson
            {
                public string Name { get; set; }
                public string MobileNum { get; set; }
            }

            // 发起人
            public class MobileOwner
            {
                // 发起人需要保存的内部状态
                public List<ContactPerson> ContactPersons { get; set; }

                public MobileOwner(List<ContactPerson> persons)
                {
                    ContactPersons = persons;
                }

                // 创建备忘录，将当期要保存的联系人列表导入到备忘录中 
                public ContactMemento CreateMemento()
                {
                    // 这里也应该传递深拷贝，new List方式传递的是浅拷贝，
                    // 因为ContactPerson类中都是string类型,所以这里new list方式对ContactPerson对象执行了深拷贝
                    // 如果ContactPerson包括非string的引用类型就会有问题，所以这里也应该用序列化传递深拷贝
                    return new ContactMemento(new List<ContactPerson>(this.ContactPersons));
                }

                // 将备忘录中的数据备份导入到联系人列表中
                public void RestoreMemento(ContactMemento memento)
                {
                    // 下面这种方式是错误的，因为这样传递的是引用，
                    // 则删除一次可以恢复，但恢复之后再删除的话就恢复不了.
                    // 所以应该传递contactPersonBack的深拷贝，深拷贝可以使用序列化来完成
                    this.ContactPersons = memento.contactPersonBack;
                }

                public void Show()
                {
                    Console.WriteLine("联系人列表中有{0}个人，他们是:", ContactPersons.Count);
                    foreach (ContactPerson p in ContactPersons)
                    {
                        Console.WriteLine("姓名: {0} 号码为: {1}", p.Name, p.MobileNum);
                    }
                }
            }

            // 备忘录
            public class ContactMemento
            {
                // 保存发起人的内部状态
                public List<ContactPerson> contactPersonBack;

                public ContactMemento(List<ContactPerson> persons)
                {
                    contactPersonBack = persons;
                }
            }

            // 管理角色
            public class Caretaker
            {
                public ContactMemento ContactM { get; set; }
            }
        }
        #endregion
    }
}
