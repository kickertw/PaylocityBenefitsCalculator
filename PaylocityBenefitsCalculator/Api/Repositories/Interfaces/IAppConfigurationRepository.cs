using Api.Models;

namespace Api.Repositories.Interfaces
{
    public interface IAppConfigurationRepository
    {
        Task<AppConfiguration?> GetAppConfigurationAsync();
    }
}
