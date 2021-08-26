using System;
using System.Collections.Generic;
using System.Text;

namespace SPTextProject.Models.Core
{
    public abstract class BaseEntity
    {

    }


    //
    // 摘要:
    //     Marks a type as keyless entity.
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class KeylessAttribute : Attribute
    {
        public KeylessAttribute() { 
        
        }
    }
}
