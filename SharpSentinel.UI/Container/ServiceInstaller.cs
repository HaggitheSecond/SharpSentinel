using Caliburn.Micro;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using SharpSentinel.UI.Extensions;
using SharpSentinel.UI.Services;

namespace SharpSentinel.UI.Container
{
    public class ServiceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IWindowManager, Services.Window.WindowManager>());
            container.Register(Component.For<IEventAggregator, EventAggregator>());

            container.Register(Classes
                .FromThisAssembly()
                .BasedOn<IService>()
                .WithService
                .FromInterface()
                .LifestyleSingleton());

            container.ForceCreationOfSingletons();
        }
    }
}