using SPCoreText.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPCoreText.Model
{
    public class LoginResult : Response<string>
    {
        public string ReturnUrl;
        public string Token;
    }
}
