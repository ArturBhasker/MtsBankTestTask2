using Autofac;
using Microsoft.Extensions.Configuration;

namespace MtsBankTestTask2.AutofacModules
{
    internal class ConfigurationModule : Module
    {
        protected override void Load(ContainerBuilder services)
        {
            var configurationBuilder = new ConfigurationBuilder();

            configurationBuilder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var configuration = configurationBuilder.Build();

            services
                .Register<IConfiguration>(_ => configuration);
        }
    }
}