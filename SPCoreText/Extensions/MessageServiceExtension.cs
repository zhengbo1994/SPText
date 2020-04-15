using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SPCoreText.Services;
using Microsoft.Extensions.DependencyInjection;

namespace SPCoreText.Extensions
{
    public static class MessageServiceExtension
    {
        public static void AddMessage(this IServiceCollection services, Action<MessageServiceBuilder> configure)
        {
//            services.AddSingleton<IMessageService, EmailService>();

            var builder = new MessageServiceBuilder(services);
            configure(builder);
        }

    }
}
