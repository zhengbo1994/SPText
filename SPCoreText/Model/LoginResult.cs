using SPCoreText.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPCoreText.Model
{
    public class LoginResult : RetResponse<string>
    {
        public string ReturnUrl;
        public string Token;
    }
}
