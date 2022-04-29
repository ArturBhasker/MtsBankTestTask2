using Autofac;
using MtsBankTestTask2.CheckServices;
using MtsBankTestTask2.CounterServices;
using MtsBankTestTask2.Listeners;

namespace MtsBankTestTask2.AutofacModules
{
    internal class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder services)
        {
            services
                .RegisterType<CommonCheckService>()
                .As<ICommonCheckService>();

            services
                .RegisterType<HtmlCounterService>()
                .As<IHtmlCounterService>();


            services
                .RegisterType<CommonCounterService>()
                .As<ICommonCounterService>();

            services
                .RegisterAssemblyTypes(ThisAssembly)
                .As<IListener>();
        }
    }
}