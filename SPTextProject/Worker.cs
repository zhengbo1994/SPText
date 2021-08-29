using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SPTextProject.IBLL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SPTextProject
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IOrderTransferService _tran;

        public Worker(ILogger<Worker> logger, IOrderTransferService orderTransferService)
        {
            _logger = logger;
            _tran = orderTransferService;
        }

        public override async Task StartAsync(CancellationToken stoppingToken)
        {
            await Task.Run(() =>
            {
                _tran.Transfer();
            });
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
