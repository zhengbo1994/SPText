using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPText.Common.DataHelper.Data
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : Attribute
    {
        private readonly string _name;

        public ColumnAttribute(string name)
        {
            _name = name;
        }

        public string GetColumn => _name;
    }
}
