﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPTextLK.Text
{
    public interface IClassBase
    {
        void Show();
        T Show<T>(T t);
    }
}
