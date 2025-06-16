using Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure;

public class TokenCleanupService(IServiceScopeFactory scopeFactory, IRefreshTokenStore refreshTokenStore, IUnitOfWork unitOfWork) : BackgroundService
{

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var expiredTokens = await refreshTokenStore.GetAllExpired();

                if (expiredTokens.Any())
                {
                    await refreshTokenStore.DeleteRange(expiredTokens);
                    await unitOfWork.SaveChangesAsync();
                }
            }

            await Task.Delay(TimeSpan.FromHours(1), stoppingToken); // Запуск каждые 1 час
        }
    }
}
