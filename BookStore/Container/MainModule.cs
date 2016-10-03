using Ninject.Extensions.Conventions;
using Ninject.Modules;

namespace BookStore.Container
{
    public class MainModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind(x =>
            {
                x.FromThisAssembly()
                    .SelectAllClasses()
                    .BindDefaultInterfaces();
            });
        }
    }
}