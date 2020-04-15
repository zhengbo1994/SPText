using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPMVCText.Models
{
    public class Text:Attribute
    {
        public int Id { get; set; }
        public string Nmae { get; set; }
        public int Age { get; set; }
        public string Area { get; set; }
    }
}