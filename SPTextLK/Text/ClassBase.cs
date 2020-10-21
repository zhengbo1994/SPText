using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Lifetime;

namespace SPTextLK.Text
{
    public class ClassBase : IClassBase
    {
        public void Show()
        {
            Console.WriteLine($"{this.GetType().Name},这是一个无参无返回值的方法！");
        }

        public T Show<T>(T t)
        {
            Console.WriteLine($"{this.GetType().Name},这是一个泛型参数且有返回值的方法！");
            return t;
        }
    }
}
