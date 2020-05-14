using System;
using System.Linq;
using System.Runtime.Loader;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceAdapter.ExternalAccess;

namespace ServiceAdapter.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddOptions();
            services.AddHttpClient();

            var targetStartup = FindTargetStartup(services);
            // Hedef dll deki RegisterServices fonksiyonunu çalıştırarak ExternalAccess library'sindeki interfaceleri config e göre IoC ye eklemiş olduk.
            targetStartup.RegisterServices(services);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }

        private IExternalStartup FindTargetStartup(IServiceCollection services)
        {
            var adapterConfig = Configuration.GetSection("AdapterConfig").Get<AdapterConfig>();
            var externalStartupType = typeof(IExternalStartup);
            // appsettings.json üzerinde tanımlı olan dll'i run time'da yüklüyoruz.
            var serviceAssembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(string.Format($@"{AppContext.BaseDirectory}\{adapterConfig.ServiceAssembly}"));
            /* Geliştirme ve Deploy aşamasında çalışacağımız hedef dll'in ServiceAdapter.API çıktısı ile aynı klasörde olması appsettings.json'da yaptığımız config tanımının daha basit olmasını sağlar.
               Bu yüzden tüm implementasyon librarylerini (ServiceAdapeter.ExternalAccess.BlumeCorp, ServiceAdapter.ExternalAccess.Default) Build outputlarını Debug için ..\ServiceAdapter.API\bin\Debug\ Release için ..\ServiceAdapter.API\bin\Release\ olarak set ediyoruz. 
               Yüklediğimiz dll'de IExternalStartup interface'ini implemnt etmiş class'ı buluyoruz. (Single kullanmamızın sebebi birden fazla implementasyon yoksa ya da birden fazla implemtasyon varsa  exception fırlatmak.)
               https://docs.microsoft.com/en-us/visualstudio/ide/how-to-change-the-build-output-directory?view=vs-2019 
            */

            var startupType = serviceAssembly.GetTypes().Single(x => externalStartupType.IsAssignableFrom(x));
            // Bulduğumuz startup class'ının instance'ını alıyoruz.
            var startup = Activator.CreateInstance(startupType) as IExternalStartup;
            return startup;
        }
    }
}
