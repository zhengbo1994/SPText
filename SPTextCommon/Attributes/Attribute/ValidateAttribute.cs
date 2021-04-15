using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SPTextCommon.Attributes.Attribute
{
    #region  验证有关
    /// <summary>
    /// 非空验证
    /// </summary>
    public class RequiredAttribute : AbstractValidateAttribute
    {
        public override bool Validate(object oValue)
        {
            if (oValue == null || string.IsNullOrWhiteSpace(oValue.ToString()))
                return false;
            else
                return true;
        }
    }
    /// <summary>
    /// 长度验证（最小值、最大值）
    /// </summary>
    public class LengthAttribute : AbstractValidateAttribute
    {
        public int _Min = 0;
        public int _Max = 0;
        public LengthAttribute(int Min, int Max)
        {
            _Min = Min;
            _Max = Max;
        }

        public override bool Validate(object oValue)
        {
            if (oValue == null || oValue.ToString().Length < this._Min || oValue.ToString().Length > this._Max)
                return false;
            else
                return true;

        }
    }
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public abstract class AbstractValidateAttribute : System.Attribute
    {
        public abstract bool Validate(object oValue);
    }

    public static class AttributcMapping
    {
        public static bool ValiData<T>(T t)
        {
            Type type = typeof(T);

            foreach (var prop in type.GetProperties())
            {
                if (prop.IsDefined(typeof(AbstractValidateAttribute), true))
                {
                    object oValue = prop.GetValue(t);
                    AbstractValidateAttribute attribute = (AbstractValidateAttribute)prop.GetCustomAttribute(typeof(AbstractValidateAttribute), true);
                    attribute.Validate(oValue);
                }
                else
                {
                    continue;
                }
            }
            return true;
        }
        //获取特性上面标记内容
        public static string GetName(this MemberInfo member)
        {
            if (member.IsDefined(typeof(AbstractBaseAttribute), true))
            {
                AbstractBaseAttribute attribute = member.GetCustomAttribute<AbstractBaseAttribute>();
                return attribute.GetRcalName();
            }
            else
            {
                return member.Name;
            }
        }
    }
    #endregion

    #region  标记有关
    public class ClassAttribute : AbstractBaseAttribute
    {
        public ClassAttribute(string name) : base(name) { }
    }
    public class PropertyAttribute : AbstractBaseAttribute
    {
        public PropertyAttribute(string name) : base(name) { }
    }
    public class AbstractBaseAttribute : System.Attribute
    {
        private string _RcalName = null;

        public AbstractBaseAttribute(string RcalName)
        {
            this._RcalName = RcalName;
        }

        public string GetRcalName()
        {
            return this._RcalName;
        }
    }
    #endregion
}
