using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPText.Common.DataHelper.Data
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : Attribute
    {
        private readonly string _name;

        public TableAttribute(string name)
        {
            _name = name;
        }

        //public string TableName {
        //    get
        //    {
        //        return _name;
        //    }
        //}
        //public string TableName
        //{
        //    get => _name;
        //}

        public string TableName => _name;
    }
}
