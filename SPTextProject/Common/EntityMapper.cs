using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SPTextProject.Common
{
    public class EntityMapper<inT, outT>
    {
        private readonly static Func<inT, outT> _func = null;

        static EntityMapper()
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(inT), "p");
            List<MemberBinding> memberBindings = new List<MemberBinding>();
            foreach (var item in typeof(outT).GetProperties())
            {
                MemberExpression memberExpression = Expression.Property(parameterExpression, typeof(inT).GetProperty(item.Name));//p.XXX
                MemberBinding memberBinding = Expression.Bind(item, memberExpression);//XXX = p.XXX
                memberBindings.Add(memberBinding);
            }
            MemberInitExpression memberInitExpression = Expression.MemberInit(Expression.New(typeof(outT)), memberBindings.ToArray());//new outT(){outT.XXX = p.XXX,outT.XXX = p.XXX,...}
            Expression<Func<inT, outT>> lambda = Expression.Lambda<Func<inT, outT>>(memberInitExpression, new ParameterExpression[]
            {
                    parameterExpression
            });//p => new outT(){outT.XXX = p.XXX,outT.XXX = p.XXX,...}

            _func = lambda.Compile();
        }

        public static outT Transition(inT t)
        {
            var result = _func(t);
            return result;
        }
    }
}
