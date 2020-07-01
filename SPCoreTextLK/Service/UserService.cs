using EF_CodeDB;
using Microsoft.EntityFrameworkCore;
using SPCoreTextLK.Interface;
using SPTextLK.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace SPCoreTextLK.Service
{
    public class UserService : BaseService, IUserService
    {
        public UserService(DbContext context) : base(context)
        {
        }

        public void UpdateLastLogin(User user)
        {
            User userDB = base.Find<User>(user.Id);
            userDB.LastLoginTime = DateTime.Now;
            this.Commit();
        }
    }
}
