using SPCoreTextLK.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace SPCoreTextLK.Service
{
    public class TestServiceA : ITestServiceA
    {
        public TestServiceA()
        {
            Console.WriteLine($"{this.GetType().Name}被构造。。。");
        }

        public void Show()
        {
            Console.WriteLine("A123456");
        }
    }
}
