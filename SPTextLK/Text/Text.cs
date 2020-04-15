using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPTextLK.Text
{
    public class Text : IText
    {
        public Text()
        {
            Console.WriteLine("这是一个构造函数！");
        }

        public Text(string name)
        {
            Console.WriteLine($"{name}这是一个构造函数！");
        }
        public void One()
        {
            Console.WriteLine("这是我的第一个方法！");
        }

        public void one()
        {
            throw new NotImplementedException();
        }
    }
}
