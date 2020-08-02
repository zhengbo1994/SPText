using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPText.Unity.Aop
{
    public interface IUserProcessor
    {
        void RegUser(User user);
        User GetUser(User user);
    }
}
