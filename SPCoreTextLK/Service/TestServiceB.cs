using SPCoreTextLK.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace SPCoreTextLK.Service
{
    public class TestServiceB : ITestServiceB
    {
        public TestServiceB(ITestServiceA iTestServiceA)
        {
            Console.WriteLine($"{this.GetType().Name}被构造。。。");
        }


        public void Show()
        {
            Console.WriteLine($"This is TestServiceB B123456");
        }
    }
}
