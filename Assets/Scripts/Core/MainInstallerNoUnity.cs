using System.Numerics;
using Leopotam.EcsLite;
using Zenject;

namespace Core
{
    namespace Installers
    {
        public class MainInstaller : Installer
        {
            private readonly StartingPositionData _mapData = new()
            {
                Button1 = new Vector3(-3.43f, 0, -4.56f),

                Button2 = new Vector3(1.2f, 0, -4.79f),

                Door1 = new Vector3(-1.66f, 0, -1.14f),

                Door2 = new Vector3(5.18f, 0, -0.42f),

                Player = Vector3.Zero
            };

            public override void InstallBindings()
            {
                Container.BindInstance(_mapData);
                Container.Bind<EcsWorld>().FromFactory<WorldBuilderNoUnity>().AsSingle();
                Container.BindInterfacesTo<SystemBuilder>().AsSingle().NonLazy();
            }
        }
    }
}