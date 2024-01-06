using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ThiefHackUI
{
    class Host
    {
        public static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Регистрация сервисов
            services.AddSingleton<IEngineService, EngineService>();

            services.AddPooledDbContextFactory<AppDbContext>(options =>
            {
                options.UseSqlite(@"Data Source=App.db");
            });

            services.AddSingleton<IDatabaseService, DatabaseService>();
            services.AddSingleton<MainWindow>();

            return services.BuildServiceProvider();
        }
    }
}
