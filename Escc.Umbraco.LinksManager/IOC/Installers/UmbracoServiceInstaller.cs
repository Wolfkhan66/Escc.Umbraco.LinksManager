using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Escc.Umbraco.LinksManager.Services;
using Escc.Umbraco.LinksManager.Services.Interfaces;

namespace Escc.Umbraco.LinksManager.IOC.Installers
{
    public class UmbracoServiceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IUmbracoService>()
                                        .ImplementedBy<UmbracoService>()
                                        .LifestyleTransient());
        }
    }
}