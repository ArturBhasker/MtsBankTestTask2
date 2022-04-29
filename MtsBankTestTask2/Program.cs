using Autofac;
using MtsBankTestTask2;
using MtsBankTestTask2.AutofacModules;

var services = new ContainerBuilder();

services.RegisterModule<ServicesModule>();
services.RegisterModule<ConfigurationModule>();

services.RegisterType<Application>();

var container = services.Build();

var application = container.Resolve<Application>();

using var cts = new CancellationTokenSource();

await application.RunAsync(cts.Token);

Console.ReadLine();