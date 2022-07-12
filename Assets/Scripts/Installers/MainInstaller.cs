using Core;
using Zenject;

namespace Installers
{
    public class MainInstaller : Installer<MainInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<WorldBuilder>().NonLazy();
        }
    }
}