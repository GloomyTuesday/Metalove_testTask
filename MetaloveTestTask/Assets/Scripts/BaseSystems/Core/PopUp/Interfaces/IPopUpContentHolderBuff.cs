using System;
using UnityEngine;

namespace Scripts.BaseSystems.Core
{
    public interface IPopUpContentHolderBuff
    {
        public Transform PopUpBackgroundHolder { get; set; }
        public event Action<Transform> OnPopUpBackgroundHolderUpdated;

        public Transform PopUpContentHolder { get; set; }
        public event Action<Transform> OnPopUpContentHolderUpdated;

        public AnimationClip PopUpAnimClipOpen { get; set; }
        public event Action<AnimationClip> OnPopUpAnimClipOpenUpdated;

        public AnimationClip PopUpAnimClipClose { get; set; }
        public event Action<AnimationClip> OnPopUpAnimClipCloseUpdated;


        public AnimationClip PopUpAnimClipBgOpen { get; set; }
        public event Action<AnimationClip> OnPopUpAnimClipBgOpenUpdated;

        public AnimationClip PopUpAnimClipBgClose { get; set; }
        public event Action<AnimationClip> OnPopUpAnimClipBgCloseUpdated;

        public AnimationClip PopUpAnimClipBgPointerDown { get; set; }
        public event Action<AnimationClip> OnPopUpAnimClipBgPointerDownUpdated;

        public AnimationClip PopUpAnimClipBgCloseCanceled { get; set; }
        public event Action<AnimationClip> OnPopUpAnimClipBgCloseCanceledUpdated;

        public GameObject PopUpBgPrefab { get; set; }
        public event Action<GameObject> OnPopUpBgPrefabUpdated;
    }
}
