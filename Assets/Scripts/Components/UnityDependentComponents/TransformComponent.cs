using UnityEngine;

namespace Components.UnityDependentComponents
{
    /// <summary>
    ///     This component holds the link to a transform.
    ///     It is used to connect Unity objects with ecsworld and should not be used on server
    /// </summary>
    public struct TransformComponent
    {
        public Transform Transform;
    }
}