using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.BaseSystems.UiRelated
{
    [RequireComponent(typeof(RectTransformAligner))]
    public class UnfoldingList : MonoBehaviour, IUnfoldingList
    {
        [SerializeField]
        private UnfoldingDirectionId _unfoldingDirectionId = UnfoldingDirectionId.Vertical;
        [SerializeField]
        private RectTransform _listRectTransform;
        [SerializeField, Space(10), Header("Object that has IRectTransformAligner component")]
        private GameObject _rectTransformAlignerObj;

        private List<RectTransform> ChildList { get; set; } = new List<RectTransform>();

        //  Rect transform of the element tht is inside the list
        private RectTransform BiggestRectTransformChild { get; set; }   

        private Vector2 FoldedRectSize { get; set; }
        private Vector2 UnfoldedRectSize { get; set; }

        public RectTransform ListRectTrans => _listRectTransform;

        public bool DeactivateGameObjectWhenFolded { get; set; } = true; 
        public bool DestroyAllChildrenWhenFolded { get; set; } = true;

        public UnfoldingDirectionId UnfoldingDirectionId { get => _unfoldingDirectionId; set => _unfoldingDirectionId = value; }

        private IRectTransformAligner _rectTransformAligner;
        private IRectTransformAligner IRectTransformAligner
        {
            get
            {
                if (_rectTransformAligner == null)
                    _rectTransformAligner = _rectTransformAlignerObj.GetComponent<RectTransformAligner>();

                return _rectTransformAligner;
            }
        }

        private RectTransform _rectTransformMain;
        private RectTransform RectTransformMain
        {
            get
            {
                if (_rectTransformMain == null)
                    _rectTransformMain = GetComponent<RectTransform>();

                return _rectTransformMain;
            }
        }

        private void OnEnable()
        {
            var center = RectTransformMain.TransformPoint(RectTransformMain.rect.center);
        }

        public void Unfold()
        {
            IRectTransformAligner.Align(); 
            SetUpInitData();
            LaunchAnimation();
        }

        public void Fold()
        {
            Action endAction = () => {

                if (DestroyAllChildrenWhenFolded)
                    DestroyAllChildren(RectTransformMain);

                if(DeactivateGameObjectWhenFolded)
                    RectTransformMain.gameObject.SetActive(false);
            };
            LaunchAnimation(false, endAction);
        }

        private void SetUpInitData()
        {
            var rectArray = GetChildrenRectTRansformArray(); 

            ChildList.AddRange(rectArray);
            float maxArea = rectArray[0].rect.width * rectArray[0].rect.height;
            BiggestRectTransformChild = rectArray[0];
            float targetRectSizeY = BiggestRectTransformChild.rect.size.y;
            float targetRectSizeX = BiggestRectTransformChild.rect.size.x;

            for (int i = 1; i < rectArray.Length; i++)
            {
                var areaBuff = rectArray[i].rect.width * rectArray[i].rect.height;

                if (maxArea < areaBuff)
                {
                    maxArea = areaBuff;
                    BiggestRectTransformChild = rectArray[i];
                }

                switch (UnfoldingDirectionId)
                {
                    case UnfoldingDirectionId.Vertical:
                        targetRectSizeX = BiggestRectTransformChild.rect.x;
                        targetRectSizeY += rectArray[i].rect.size.y;
                        break;

                    case UnfoldingDirectionId.Horizontal:
                        targetRectSizeX += rectArray[i].rect.size.x;
                        targetRectSizeY = BiggestRectTransformChild.rect.y;
                        break;
                }
            }

            FoldedRectSize = BiggestRectTransformChild.rect.size;
            UnfoldedRectSize = new Vector2(targetRectSizeX, targetRectSizeY);
        }

        private RectTransform[] GetChildrenRectTRansformArray()
        {
            var rectTransformArray = new RectTransform[RectTransformMain.childCount];
            for (int i = 0; i < rectTransformArray.Length; i++)
                rectTransformArray[i] = RectTransformMain.GetChild(i).GetComponent<RectTransform>();

            return rectTransformArray; 
        }

        private void LaunchAnimation(bool unfold = true, Action endAction = null)
        {
            var targetRectSize = unfold == true ? UnfoldedRectSize : FoldedRectSize;

            Animation(
                targetRectSize,
                RectTransformMain,
                IRectTransformAligner,
                endAction
                ); 
        }

        private void Animation(
            Vector2 targetRectSize,
            RectTransform rectTransform,
            IRectTransformAligner rectTransformAligner,
            Action endAction = null
            )
        {
            rectTransform.sizeDelta = targetRectSize;
            rectTransformAligner.Align();
            var center = RectTransformMain.TransformPoint(RectTransformMain.rect.center);

            if (endAction != null)
                endAction.Invoke(); 
        }

        private void DestroyAllChildren(Transform parent)
        {
            for (int i = parent.childCount - 1; i >= 0; i--)
                Destroy(parent.GetChild(i).gameObject);
        }
    }
}
