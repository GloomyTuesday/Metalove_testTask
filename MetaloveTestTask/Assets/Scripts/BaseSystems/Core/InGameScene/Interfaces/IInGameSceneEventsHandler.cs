using System;
using UnityEngine;

namespace Scripts.BaseSystems.Core
{
    internal interface IInGameSceneEventsHandler
    {
        public event Action<GameObject> OnLoadInGameScene;
        public event Action OnLoadPreviousInGameScene;
    }
}
