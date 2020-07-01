using EF_CodeDB;
using SPTextLK.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace SPCoreTextLK.Interface
{
    public interface IUserService : IBaseService
    {
        //void Query();
        //void Update();
        //void Delete();
        //void Add();

        void UpdateLastLogin(User user);
    }
}
