using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using ServiceAdapter.ExternalAccess.Service;

namespace ServiceAdapter.ExternalAccess.Default
{
    public class ExternalStartup : IExternalStartup
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<ICategoryService, CategoryService>();
        }
    }
}
