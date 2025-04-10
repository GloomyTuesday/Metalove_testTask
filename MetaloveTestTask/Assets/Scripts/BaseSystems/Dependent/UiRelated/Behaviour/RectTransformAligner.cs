using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Scripts.BaseSystems.UiRelated
{
    public class RectTransformAligner : MonoBehaviour, IRectTransformAligner, IEventDataUpdated
    {
        [SerializeField]
        private bool _align;
        [SerializeField]
        private RectTransformAlignmentId _rectTransformAlignmentId;
        [SerializeField, Space(10), Header("Align when OnEnable called")]
        private bool _alignOnLaunch = false; 
        [SerializeField, FormerlySerializedAs("_offset")]
        private float _offsetOnArrangingAxis;
        [SerializeField]
        private float _offsetOnNonArrangingAxis;

        [SerializeField, Space(10), Header(" Mnimnal size for main rect")]
        private Vector2 _minSizeConstaint = Vector2.zero;

        [SerializeField, Space(15), Header("Objects with IEventDataUpdated component")]
        [Header("Align is invoked when OnDataUpdated callback occures")]
        private GameObject[] _eventDataUpdatedObjects;

        [SerializeField, Space(10)]
        private RectTransform[] _rectTransformToIgnore;

        [SerializeField, HideInInspector]
        private RectTransform _mainRectTransform = null;

        public int InstanceId { get; private set; } 

        public event Action OnDataUpdated;

        public bool AlignOnLaunch { get=> _alignOnLaunch; set=> _alignOnLaunch= value; }

        private HashSet<RectTransform> _rectTransformToIgnoreHasSet; 
        private HashSet<RectTransform> RectTRansformToIgnore {
            get
            {
                if (_rectTransformToIgnoreHasSet == null)
                {
                    _rectTransformToIgnoreHasSet = new HashSet<RectTransform>();
                    for (int i = 0; i < _rectTransformToIgnore.Length; i++)
                        if (!_rectTransformToIgnoreHasSet.Contains(_rectTransformToIgnore[i]))
                            _rectTransformToIgnoreHasSet.Add(_rectTransformToIgnore[i] ); 
                }

                return _rectTransformToIgnoreHasSet;
            }
        }
        private IEventDataUpdated[] _iEventDataUpdatedArray; 
        private IEventDataUpdated[] IEventDataUpdatedArray
        {
            get
            {
                if(_iEventDataUpdatedArray == null)
                {
                    var list = new List<IEventDataUpdated>();
                    for (int i = 0; i < _eventDataUpdatedObjects.Length; i++)
                    {
                        var iEventDataUpdated = _eventDataUpdatedObjects[i].GetComponent<IEventDataUpdated>();
                        
                        if (iEventDataUpdated != null)
                            list.Add(iEventDataUpdated);
                    }

                    _iEventDataUpdatedArray = list.ToArray();
                }

                return _iEventDataUpdatedArray; 
            }
        }

        private class RectTransformAlignProperties
        {
            public Vector2 _anchorMax;
            public Vector2 _anchorMin;
            public Vector2 _pivot;

            public RectTransformAlignProperties(RectTransform rectTransform)
            {
                _anchorMax = rectTransform.anchorMax;
                _anchorMin = rectTransform.anchorMin;
                _pivot = rectTransform.pivot;
            }
        }

        public float OffsetOnArrangingAxis
        {
            get => _offsetOnArrangingAxis;

            set
            {
                _offsetOnArrangingAxis = value;
                if (_mainRectTransform == null)
                    _mainRectTransform = GetComponent<RectTransform>();

                Align(_mainRectTransform, _rectTransformAlignmentId, _offsetOnArrangingAxis, _offsetOnNonArrangingAxis);
            }
        }

        public RectTransformAlignmentId RectTransformAlignmentId
        {
            get => _rectTransformAlignmentId;

            set
            {
                _rectTransformAlignmentId = value;
                if (_mainRectTransform == null)
                    _mainRectTransform = GetComponent<RectTransform>();

                Align(_mainRectTransform, _rectTransformAlignmentId, _offsetOnArrangingAxis, _offsetOnNonArrangingAxis);
            }
        }

        private void OnValidate()
        {
            if (!_align) return;

            _align = false;

            if (_mainRectTransform == null)
                _mainRectTransform = GetComponent<RectTransform>();

            Align(_mainRectTransform, _rectTransformAlignmentId, _offsetOnArrangingAxis, _offsetOnNonArrangingAxis);
        }

        private void OnEnable()
        {
            InstanceId = GetInstanceID(); 

            Subscribe();

            if (!AlignOnLaunch) return;
            Align();
        }

        private void OnDisable()
        {
            Unsubscribe(); 
        }

        private void Subscribe()
        {
            for (int i = 0; i < IEventDataUpdatedArray.Length; i++)
                IEventDataUpdatedArray[i].OnDataUpdated += Align;
        }

        private void Unsubscribe()
        {
            for (int i = 0; i < IEventDataUpdatedArray.Length; i++)
                if (IEventDataUpdatedArray[i]!=null)
                    IEventDataUpdatedArray[i].OnDataUpdated -= Align;
                
        }

        public void Align()
        {
            if (_mainRectTransform == null)
                _mainRectTransform = GetComponent<RectTransform>();

            Align(_mainRectTransform, _rectTransformAlignmentId, _offsetOnArrangingAxis, _offsetOnNonArrangingAxis);
        }

        public async void AlignNextFrame()
        {
            await Task.Yield();

            if (_mainRectTransform == null)
                _mainRectTransform = GetComponent<RectTransform>();

            Align();
            //  Align(_mainRectTransform, _rectTransformAlignmentId, _offsetOnArrangingAxis);
        }

        public async void AlignWithDelay(int frameAmountToDelay)
        {
            for(int i = 0;i < frameAmountToDelay; i++)
                await Task.Yield();

            Align();
        }

        private void Align(
            RectTransform mainRectTransform,
            RectTransformAlignmentId rectTransformAlignmentId,
            float offsetOnArrangingAxis,
            float offsetOnNonArrangingAxis=0
            )
        {
            var childCount = mainRectTransform.childCount;
            var rectTransformList = new List<RectTransform>();
            var rectTransformPropertiesBuffer = new List<RectTransformAlignProperties>();

            var scrollRect = mainRectTransform.GetComponentInParent<ScrollRect>();
            bool scrollHorizontalFlagBuff = false;
            bool scrollVerticalFlagBuff = false;

            if (scrollRect != null)
            {
                scrollHorizontalFlagBuff = scrollRect.horizontal;
                scrollVerticalFlagBuff = scrollRect.vertical;
                scrollRect.horizontal = false;
                scrollRect.vertical = false;
            }

            float accumulatedValue = offsetOnArrangingAxis;

            mainRectTransform.sizeDelta = new Vector2(0, 0);

            //  Calc content size
            for (int i = 0; i < childCount; i++)
            {
                RectTransform rectTransform ;
                rectTransform = mainRectTransform.GetChild(i).GetComponent<RectTransform>();
                
                if (RectTRansformToIgnore.Contains(rectTransform))
                    continue;

                rectTransformList.Add(rectTransform); 
                rectTransformPropertiesBuffer.Add(new RectTransformAlignProperties(rectTransformList[^1]));

                if (!rectTransformList[^1].gameObject.activeInHierarchy) continue;

                CalcAndApplySizeForMainRectTransform(
                    mainRectTransform,
                    rectTransformList[^1],
                    ref accumulatedValue,
                    rectTransformAlignmentId
                    );
            }

            accumulatedValue = offsetOnArrangingAxis;

            //  Calc and apply position for elements inside rect
            for (int i = 0; i < rectTransformList.Count; i++)
            {
                if (!rectTransformList[i].gameObject.activeInHierarchy) continue;

                CalcAndApplyPositionForChildRectTransform
                    (
                    mainRectTransform,
                    rectTransformList[i],
                    ref accumulatedValue,
                    offsetOnArrangingAxis,
                    rectTransformAlignmentId
                    );
            }

            if (scrollRect != null)
            {
                scrollRect.horizontal = scrollHorizontalFlagBuff;
                scrollRect.vertical = scrollVerticalFlagBuff;
            }

            for (int i = 0; i < rectTransformList.Count; i++)
            {
                var positionBufferX = rectTransformList[i].localPosition.x - rectTransformList[i].sizeDelta.x * rectTransformList[i].pivot.x;
                var positionBufferY = rectTransformList[i].localPosition.y - rectTransformList[i].sizeDelta.y * rectTransformList[i].pivot.y;

                rectTransformList[i].anchorMin = rectTransformPropertiesBuffer[i]._anchorMin;
                rectTransformList[i].anchorMax = rectTransformPropertiesBuffer[i]._anchorMax;
                rectTransformList[i].pivot = rectTransformPropertiesBuffer[i]._pivot;

                var newPosition = new Vector2(
                    positionBufferX + rectTransformList[i].sizeDelta.x * rectTransformList[i].pivot.x,
                    positionBufferY + rectTransformList[i].sizeDelta.y * rectTransformList[i].pivot.y
                    );

                rectTransformList[i].localPosition = newPosition;
                rectTransformList[i].ForceUpdateRectTransforms();
            }

            if (mainRectTransform.rect.size.x < _minSizeConstaint.x)
                mainRectTransform.sizeDelta = new Vector2( _minSizeConstaint.x, mainRectTransform.sizeDelta.y);

            if (mainRectTransform.rect.size.y < _minSizeConstaint.y)
                mainRectTransform.sizeDelta = new Vector2( mainRectTransform.sizeDelta.x , _minSizeConstaint.y);

            OnDataUpdated?.Invoke(); 
        }

        private void CalcAndApplySizeForMainRectTransform(
            RectTransform mainRect,
            RectTransform childRect,
            ref float accumulatedValue,
            RectTransformAlignmentId alignmentId
            )
        {
            switch (alignmentId)
            {
                case RectTransformAlignmentId.VerticalTopToBottom:
                    CalcContentSizeVertical(
                                            mainRect,
                                            childRect,
                                            ref accumulatedValue,
                                            _offsetOnArrangingAxis,
                                            _offsetOnNonArrangingAxis
                                            ); 
                    break;
                case RectTransformAlignmentId.VerticalBottomToTop: 
                    CalcContentSizeVertical(
                                            mainRect,
                                            childRect,
                                            ref accumulatedValue,
                                            _offsetOnArrangingAxis,
                                            _offsetOnNonArrangingAxis
                                            );
                    break;
                case RectTransformAlignmentId.HorizontalLeftToRight: CalcContentSizeHorizontal(
                                            mainRect,
                                            childRect,
                                            ref accumulatedValue,
                                            _offsetOnArrangingAxis,
                                            _offsetOnNonArrangingAxis
                                            );
                    break;
                case RectTransformAlignmentId.HorizontalRightToLeft: CalcContentSizeHorizontal(
                                            mainRect,
                                            childRect,
                                            ref accumulatedValue,
                                            _offsetOnArrangingAxis,
                                            _offsetOnNonArrangingAxis
                                            );
                    break;
            }
        }

        private void CalcAndApplyPositionForChildRectTransform(
            RectTransform mainRect,
            RectTransform childRect,
            ref float accumulatedValue,
            float offsetOnArrangingAxis,
            RectTransformAlignmentId alignmentId
            )
        {
            switch (alignmentId)
            {
                case RectTransformAlignmentId.VerticalTopToBottom: CalcAndApplyVerticalTopToBottom(
                    mainRect,
                    childRect,
                    ref accumulatedValue,
                    offsetOnArrangingAxis); 
                    break;
                case RectTransformAlignmentId.VerticalBottomToTop: CalcAndApplyVerticalBottomToTop(
                    mainRect,
                    childRect,
                    ref accumulatedValue,
                    offsetOnArrangingAxis);
                    break;
                case RectTransformAlignmentId.HorizontalLeftToRight: CalcAndApplyHorizontalLeftToRight(
                    mainRect,
                    childRect,
                    ref accumulatedValue,
                    offsetOnArrangingAxis);
                    break;
                case RectTransformAlignmentId.HorizontalRightToLeft: CalcAndApplyHorizontalRightToLeft(
                    mainRect,
                    childRect,
                    ref accumulatedValue,
                    offsetOnArrangingAxis);
                    break;
            }
        }

        private void CalcAndApplyVerticalTopToBottom(
            RectTransform mainRect,
            RectTransform childRect,
            ref float accumulatedValue,
            float offsetOnArrangingAxis
            )
        {
            childRect.pivot = new Vector2(.5f, 1);
            childRect.anchorMin = new Vector2(.5f, 1);
            childRect.anchorMax = new Vector2(.5f, 1);
            childRect.localPosition = Vector2.zero;

            Vector2 localPosition = Vector2.zero;
            localPosition.y = mainRect.sizeDelta.y - mainRect.sizeDelta.y * mainRect.pivot.y - accumulatedValue;
            localPosition.x = mainRect.sizeDelta.x / 2 - mainRect.sizeDelta.x * mainRect.pivot.x;
            childRect.localPosition = localPosition;
            accumulatedValue += childRect.sizeDelta.y + offsetOnArrangingAxis;
            childRect.ForceUpdateRectTransforms();
        }

        private void CalcAndApplyVerticalBottomToTop(
            RectTransform mainRect,
            RectTransform childRect,
            ref float accumulatedValue,
            float offsetOnArrangingAxis
            )
        {
            childRect.pivot = new Vector2(.5f, 0);
            childRect.anchorMin = new Vector2(.5f, 0);
            childRect.anchorMax = new Vector2(.5f, 0);

            Vector2 localPosition = Vector2.zero;
            localPosition.y = -mainRect.sizeDelta.y * mainRect.pivot.y + accumulatedValue;
            localPosition.x = mainRect.sizeDelta.x / 2 - mainRect.sizeDelta.x * mainRect.pivot.x;
            childRect.localPosition = localPosition;
            accumulatedValue += childRect.sizeDelta.y + offsetOnArrangingAxis;
            childRect.ForceUpdateRectTransforms();
        }

        private void CalcAndApplyHorizontalLeftToRight(
            RectTransform mainRect,
            RectTransform childRect,
            ref float accumulatedValue,
            float offsetOnArrangingAxis
            )
        {
            childRect.pivot = new Vector2(0, .5f);
            childRect.anchorMin = new Vector2(.5f, .5f);
            childRect.anchorMax = new Vector2(.5f, .5f);

            Vector2 localPosition = Vector2.zero;
            localPosition.x = -mainRect.sizeDelta.x + mainRect.sizeDelta.x * mainRect.pivot.x + accumulatedValue;
            localPosition.y = mainRect.sizeDelta.y / 2 - mainRect.sizeDelta.y * mainRect.pivot.y;
            childRect.localPosition = localPosition;
            accumulatedValue += childRect.sizeDelta.x + offsetOnArrangingAxis;
        }

        private void CalcAndApplyHorizontalRightToLeft(
            RectTransform mainRect,
            RectTransform childRect,
            ref float accumulatedValue,
            float offsetOnArrangingAxis
            )
        {
            childRect.pivot = new Vector2(1, .5f);

            Vector2 localPosition = Vector2.zero;
            localPosition.x = mainRect.sizeDelta.x - mainRect.sizeDelta.x * mainRect.pivot.x - accumulatedValue;
            localPosition.y = mainRect.sizeDelta.y / 2 - mainRect.sizeDelta.y * mainRect.pivot.y;
            childRect.localPosition = localPosition;
            accumulatedValue += childRect.sizeDelta.x + offsetOnArrangingAxis;
        }

        private void CalcContentSizeVertical(
            RectTransform mainRect, 
            RectTransform childRect,
            ref float accumulatedValue,
            float offsetOnArrangingAxis,
            float offsetOnNonArrangingAxis)
        {
            Vector2 mainRectSize = mainRect.sizeDelta;

            var value = childRect.sizeDelta.x + offsetOnNonArrangingAxis;

            if (mainRectSize.x  < value)
                mainRectSize.x = value;
            

            accumulatedValue += childRect.sizeDelta.y + offsetOnArrangingAxis;
            mainRectSize.y = accumulatedValue;
            mainRect.sizeDelta = mainRectSize;
        }

        private void CalcContentSizeHorizontal(
            RectTransform mainRect,
            RectTransform childRect,
            ref float accumulatedValue,
            float offsetOnArrangingAxis,
            float offsetOnNonArrangingAxis)
        {
            Vector2 mainRectSize = mainRect.sizeDelta;

            var value = childRect.sizeDelta.y + offsetOnNonArrangingAxis;

            if (mainRectSize.y < value)
                mainRectSize.y = value;

            accumulatedValue += childRect.sizeDelta.x + offsetOnArrangingAxis;
            mainRectSize.x = accumulatedValue;
            mainRect.sizeDelta = mainRectSize;
        }
    }
}
