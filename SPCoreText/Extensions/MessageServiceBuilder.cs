using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SPCoreText.Services;

namespace SPCoreText.Extensions
{
    public class MessageServiceBuilder
    {
        public IServiceCollection ServiceCollection { get; set; }

        public MessageServiceBuilder(IServiceCollection services)
        {
            ServiceCollection = services;
        }

        public void UseEmail()
        {
            ServiceCollection.AddSingleton<IMessageService, EmailService>();
        }

        public void UseSms()
        {
            ServiceCollection.AddSingleton<IMessageService, SmsService>();
        }
    }
}
