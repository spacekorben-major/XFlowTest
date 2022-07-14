using Leopotam.EcsLite;
using Systems.UnityDependentSystems;
using UnityEngine;
using Zenject;

namespace Core.UnityDependent
{
    public class MainInstaller : MonoInstaller
    {
        [SerializeField] 
        private SceneData SceneData;

        public override void InstallBindings()
        {
            Container.BindInstances(SceneData);

            Container.Bind<EcsWorld>().FromFactory<WorldBuilder>().AsSingle();

            Container.BindInterfacesTo<SystemBuilder>().AsSingle().NonLazy();

            Container.BindInterfacesTo<PlayerInputSystem>().AsSingle().NonLazy();
            Container.BindInterfacesTo<PositionTranslationSystem>().AsSingle().NonLazy();
            Container.BindInterfacesTo<RotationTranslationSystem>().AsSingle().NonLazy();
            Container.BindInterfacesTo<PlayerAnimationSystem>().AsSingle().NonLazy();
        }
    }
}