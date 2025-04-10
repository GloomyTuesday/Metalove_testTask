using UnityEngine;

namespace Scripts.BaseSystems.Core
{
    public interface IInGameScene
    {
        public GameObject InGameScenePrefab { get; }
        Transform CanvasContentHolder { get; }
        Transform SpaceContentHolder { get; }
        Transform RootContentHolder { get; }

        string InGameSceneName { get; }

        void DistributeContent(
            Transform canvasContentHolder,
            Transform worldContentHolder,
            Transform rootContentHolder
            );

        void DestroyScene();
    }
}
