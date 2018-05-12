using System.Linq;
using Castle.Core;
using Castle.Windsor;

namespace SharpSentinel.UI.Extensions
{
    public static class WindsorContainerExtensions
    {
        /// <summary>
        /// Disables lazy loading of singleton services - important when you have dependencies between services
        /// </summary>
        /// <param name="container">The windsorcontainer</param>
        public static void ForceCreationOfSingletons(this IWindsorContainer container)
        {
            var singletons = container
                .Kernel
                .GetAssignableHandlers(typeof(object))
                .Where(h => h.ComponentModel.LifestyleType == LifestyleType.Singleton)
                .SelectMany(f => f.ComponentModel.Services);
            
            foreach (var singleton in singletons)
            {
                container.Resolve(singleton);
            }
        }
    }
}