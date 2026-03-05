using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using carGooBackend.Data;
using Microsoft.EntityFrameworkCore;

namespace carGooBackend.Services
{
    public class VehicleOfferCleanupService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly TimeSpan _checkInterval = TimeSpan.FromHours(1);

        public VehicleOfferCleanupService(IServiceProvider services)
        {
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _services.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<CarGooDataContext>();
                    var todayUtc = DateTime.UtcNow.Date;
                    var expiredVehicleOffers = await dbContext.PonudaVozila
                        .Where(p => p.Utovar < todayUtc)
                        .ToListAsync(stoppingToken);
                    if (expiredVehicleOffers.Any())
                    {
                        dbContext.PonudaVozila.RemoveRange(expiredVehicleOffers);
                        await dbContext.SaveChangesAsync(stoppingToken);
                    }
                }

                await Task.Delay(_checkInterval, stoppingToken);
            }
        }
    }
}