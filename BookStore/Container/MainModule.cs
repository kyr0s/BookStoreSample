using System.Web.Mvc;
using Ninject.Extensions.Conventions;
using Ninject.Modules;

namespace BookStore.Container
{
    public class MainModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind(x => x
                            .FromThisAssembly()
                            .SelectAllClasses()
                            .BindAllInterfaces()
                            .Configure(c => c.InSingletonScope())
                            );

            Kernel.Bind(x => x
                            .FromThisAssembly()
                            .SelectAllClasses()
                            .InheritedFrom<Controller>()
                            .BindAllInterfaces()
                            .Configure(c => c.InTransientScope()));
        }
    }
}