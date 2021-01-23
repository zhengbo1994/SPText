using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SPTextCommon
{
    /// <summary>
    /// Ctor委托
    /// </summary>
    /// <returns></returns>
    internal delegate object CtorDelegate();
    /// <summary>
    /// 方法委托
    /// </summary>
    /// <param name="target"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    internal delegate object MethodDelegate(object target, object[] args);
    /// <summary>
    /// 获取值委托
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    internal delegate object GetValueDelegate(object target);
    /// <summary>
    /// 设置值委托
    /// </summary>
    /// <param name="target"></param>
    /// <param name="arg"></param>
    internal delegate void SetValueDelegate(object target, object arg);

    /// <summary>
    /// 动态方法工厂
    /// </summary>
    internal static class DynamicMethodFactory
    {
        /// <summary>
        /// 创建构造函数
        /// </summary>
        /// <param name="constructor"></param>
        /// <returns></returns>
        public static CtorDelegate CreateConstructor(ConstructorInfo constructor)
        {
            if (constructor == null)
                throw new ArgumentNullException("constructor");
            if (constructor.GetParameters().Length > 0)
                throw new NotSupportedException("不支持有参数的构造函数。");

            DynamicMethod dm = new DynamicMethod(
                "ctor",
                constructor.DeclaringType,
                Type.EmptyTypes,
                true);

            ILGenerator il = dm.GetILGenerator();
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Newobj, constructor);
            il.Emit(OpCodes.Ret);

            return (CtorDelegate)dm.CreateDelegate(typeof(CtorDelegate));
        }
        /// <summary>
        /// 创建方法
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static MethodDelegate CreateMethod(MethodInfo method)
        {
            ParameterInfo[] pi = method.GetParameters();

            DynamicMethod dm = new DynamicMethod("DynamicMethod", typeof(object),
                new Type[] { typeof(object), typeof(object[]) },
                typeof(DynamicMethodFactory), true);

            ILGenerator il = dm.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);

            for (int index = 0; index < pi.Length; index++)
            {
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Ldc_I4, index);

                Type parameterType = pi[index].ParameterType;
                if (parameterType.IsByRef)
                {
                    parameterType = parameterType.GetElementType();
                    if (parameterType.IsValueType)
                    {
                        il.Emit(OpCodes.Ldelem_Ref);
                        il.Emit(OpCodes.Unbox, parameterType);
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldelema, parameterType);
                    }
                }
                else
                {
                    il.Emit(OpCodes.Ldelem_Ref);

                    if (parameterType.IsValueType)
                    {
                        il.Emit(OpCodes.Unbox, parameterType);
                        il.Emit(OpCodes.Ldobj, parameterType);
                    }
                }
            }

            if ((method.IsAbstract || method.IsVirtual)
                && !method.IsFinal && !method.DeclaringType.IsSealed)
            {
                il.Emit(OpCodes.Callvirt, method);
            }
            else
            {
                il.Emit(OpCodes.Call, method);
            }

            if (method.ReturnType == typeof(void))
            {
                il.Emit(OpCodes.Ldnull);
            }
            else if (method.ReturnType.IsValueType)
            {
                il.Emit(OpCodes.Box, method.ReturnType);
            }
            il.Emit(OpCodes.Ret);

            return (MethodDelegate)dm.CreateDelegate(typeof(MethodDelegate));
        }
        /// <summary>
        /// 创建属性生成器
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static GetValueDelegate CreatePropertyGetter(PropertyInfo property)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            if (!property.CanRead)
                return null;

            MethodInfo getMethod = property.GetGetMethod(true);

            DynamicMethod dm = new DynamicMethod("PropertyGetter", typeof(object),
                new Type[] { typeof(object) },
                property.DeclaringType, true);

            ILGenerator il = dm.GetILGenerator();

            if (!getMethod.IsStatic)
            {
                il.Emit(OpCodes.Ldarg_0);
                il.EmitCall(OpCodes.Callvirt, getMethod, null);
            }
            else
                il.EmitCall(OpCodes.Call, getMethod, null);

            if (property.PropertyType.IsValueType)
                il.Emit(OpCodes.Box, property.PropertyType);

            il.Emit(OpCodes.Ret);

            return (GetValueDelegate)dm.CreateDelegate(typeof(GetValueDelegate));
        }
        /// <summary>
        /// 创建属性设置器
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static SetValueDelegate CreatePropertySetter(PropertyInfo property)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            if (!property.CanWrite)
                return null;

            MethodInfo setMethod = property.GetSetMethod(true);

            DynamicMethod dm = new DynamicMethod("PropertySetter", null,
                new Type[] { typeof(object), typeof(object) },
                property.DeclaringType, true);

            ILGenerator il = dm.GetILGenerator();

            if (!setMethod.IsStatic)
            {
                il.Emit(OpCodes.Ldarg_0);
            }
            il.Emit(OpCodes.Ldarg_1);

            EmitCastToReference(il, property.PropertyType);
            if (!setMethod.IsStatic && !property.DeclaringType.IsValueType)
            {
                il.EmitCall(OpCodes.Callvirt, setMethod, null);
            }
            else
                il.EmitCall(OpCodes.Call, setMethod, null);

            il.Emit(OpCodes.Ret);

            return (SetValueDelegate)dm.CreateDelegate(typeof(SetValueDelegate));
        }
        /// <summary>
        /// 创建字段生成器
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static GetValueDelegate CreateFieldGetter(FieldInfo field)
        {
            if (field == null)
                throw new ArgumentNullException("field");

            DynamicMethod dm = new DynamicMethod("FieldGetter", typeof(object),
                new Type[] { typeof(object) },
                field.DeclaringType, true);

            ILGenerator il = dm.GetILGenerator();

            if (!field.IsStatic)
            {
                il.Emit(OpCodes.Ldarg_0);

                EmitCastToReference(il, field.DeclaringType);  //to handle struct object

                il.Emit(OpCodes.Ldfld, field);
            }
            else
                il.Emit(OpCodes.Ldsfld, field);

            if (field.FieldType.IsValueType)
                il.Emit(OpCodes.Box, field.FieldType);

            il.Emit(OpCodes.Ret);

            return (GetValueDelegate)dm.CreateDelegate(typeof(GetValueDelegate));
        }
        /// <summary>
        /// 创建字段设置器
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static SetValueDelegate CreateFieldSetter(FieldInfo field)
        {
            if (field == null)
                throw new ArgumentNullException("field");

            DynamicMethod dm = new DynamicMethod("FieldSetter", null,
                new Type[] { typeof(object), typeof(object) },
                field.DeclaringType, true);

            ILGenerator il = dm.GetILGenerator();

            if (!field.IsStatic)
            {
                il.Emit(OpCodes.Ldarg_0);
            }
            il.Emit(OpCodes.Ldarg_1);

            EmitCastToReference(il, field.FieldType);

            if (!field.IsStatic)
                il.Emit(OpCodes.Stfld, field);
            else
                il.Emit(OpCodes.Stsfld, field);
            il.Emit(OpCodes.Ret);

            return (SetValueDelegate)dm.CreateDelegate(typeof(SetValueDelegate));
        }
        /// <summary>
        /// 投影到引用
        /// </summary>
        /// <param name="il"></param>
        /// <param name="type"></param>
        private static void EmitCastToReference(ILGenerator il, Type type)
        {
            if (type.IsValueType)
                il.Emit(OpCodes.Unbox_Any, type);
            else
                il.Emit(OpCodes.Castclass, type);
        }
    }
}
