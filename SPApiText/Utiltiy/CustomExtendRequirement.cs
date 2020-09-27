using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPCoreApiText.Utiltiy
{
    public class CustomExtendRequirement : IAuthorizationRequirement
    {
    }
    public class CustomExtendRequirementHandler : AuthorizationHandler<CustomExtendRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomExtendRequirement requirement)
        {
            //context.User.Identities.First().Claims

            //var jti = context.User.FindFirst("jti")?.Value;// 检查 Jti 是否存在

            bool tokenExists = false;
            if (tokenExists)
            {
                context.Fail();
            }
            else
            {
                context.Succeed(requirement); // 显式的声明验证成功
            }
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// 两种邮箱都能支持 
    /// 
    /// </summary>
    public class DoubleEmailRequirement : IAuthorizationRequirement
    {
    }

    public class QQMailHandler : AuthorizationHandler<DoubleEmailRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DoubleEmailRequirement requirement)
        {
            //if (context.User != null && context.User.HasClaim(c => c.Type == ClaimTypes.Email))
            //{
            //    var email = context.User.FindFirst(c => c.Type == ClaimTypes.Email).Value;
            if (context.User != null && context.User.HasClaim(c => c.Type == "EMail"))
            {
                var email = context.User.FindFirst(c => c.Type == "EMail").Value;
                Console.WriteLine(email);
                if (email.EndsWith("@qq.com", StringComparison.OrdinalIgnoreCase))
                {
                    context.Succeed(requirement);
                }
                else
                {
                    //context.Fail();//不设置失败
                }
            }
            return Task.CompletedTask;
        }
    }
    public class ZhaoxiMailHandler : AuthorizationHandler<DoubleEmailRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DoubleEmailRequirement requirement)
        {
            //if (context.User != null && context.User.HasClaim(c => c.Type == ClaimTypes.Email))
            //{
            //    var email = context.User.FindFirst(c => c.Type == ClaimTypes.Email).Value;

            if (context.User != null && context.User.HasClaim(c => c.Type == "EMail"))
            {
                var email = context.User.FindFirst(c => c.Type == "EMail").Value;
                Console.WriteLine(email);
                if (email.EndsWith("@ZhaoxiEdu.Net", StringComparison.OrdinalIgnoreCase))
                {
                    context.Succeed(requirement);
                }
                else
                {
                    //context.Fail();
                }
            }
            return Task.CompletedTask;
        }
    }


}
