using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPCoreApiText.Utiltiy
{
    public class JWTTokenOptions
    {
        public string Audience
        {
            get;
            set;
        }
        public string SecurityKey
        {
            get;
            set;
        }
        //public SigningCredentials Credentials
        //{
        //    get;
        //    set;
        //}
        public string Issuer
        {
            get;
            set;
        }
    }

    public class CurrentUserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Account { get; set; }
        public string Mobile { get; set; }
        public string EMail { get; set; }
        //public string Password { get; set; }
        public string Role { get; set; }
        public int Age { get; set; }
        /// <summary>
        /// 0女 1男
        /// </summary>
        public byte Sex { get; set; }
    }
}
