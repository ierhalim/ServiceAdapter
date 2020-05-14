using Microsoft.Extensions.DependencyInjection;


namespace ServiceAdapter.ExternalAccess
{
    public interface IExternalStartup
    {
        void RegisterServices(IServiceCollection services);
    }
}
