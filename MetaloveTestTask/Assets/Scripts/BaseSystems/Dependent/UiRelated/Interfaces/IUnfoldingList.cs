using UnityEngine;

namespace Scripts.BaseSystems.UiRelated
{
    public interface IUnfoldingList 
    {
        public UnfoldingDirectionId UnfoldingDirectionId { get; set; }
        public void Unfold();
        public bool DeactivateGameObjectWhenFolded { get; set; }
        public bool DestroyAllChildrenWhenFolded { get; set; }
        public void Fold();
        public RectTransform ListRectTrans { get; }
    }
}
