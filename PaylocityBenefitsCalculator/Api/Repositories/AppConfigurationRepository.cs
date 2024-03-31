using Api.Data;
using Api.Models;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories
{
    public class AppConfigurationRepository : IAppConfigurationRepository
    {
        private readonly PaylocityDbContext _paylocityDbContext;

        public AppConfigurationRepository(PaylocityDbContext context)
        {
            _paylocityDbContext = context;
        }

        public async Task<AppConfiguration?> GetAppConfigurationAsync()
        {
            var appConfig = await _paylocityDbContext.AppConfigs.FirstOrDefaultAsync();
            return appConfig;
        }
    }
}
