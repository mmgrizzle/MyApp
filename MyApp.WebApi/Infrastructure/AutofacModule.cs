using Autofac;
using MyApp.Common;
using MyApp.Infrastructure;

namespace MyApp.WebApi.Infrastructure
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        { 
            builder.RegisterType<MachineDateTime>().As<IDateTime>();
        }
    }

}
