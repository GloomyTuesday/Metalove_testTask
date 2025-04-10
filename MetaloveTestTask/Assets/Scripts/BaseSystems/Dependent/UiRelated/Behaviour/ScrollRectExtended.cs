using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

namespace Scripts.BaseSystems.UiRelated
{
    public class ScrollRectExtended : ScrollRect
    {
        [SerializeField] private bool _cacheParentEventReceivers = true;

        private IEventSystemHandler[] _parentEventReceivers = default;
        private bool routeToParent = false;

        /// <summary>
        ///     Apply action to all parents
        /// </summary>
        private void DoForParents<T>(Action<T> action) where T : IEventSystemHandler
        {

            if (_cacheParentEventReceivers)
            {
                ApplyActionToCachedParents(action); 
                return;
            }

            ApplyActionToAllParents(action); 
        }

        private void ApplyActionToCachedParents<T>(Action<T> action) where T : IEventSystemHandler
        {
            if (_parentEventReceivers == default)
                _parentEventReceivers = transform.parent.GetComponentsInParent<IEventSystemHandler>();

            for (int i = 0; i < _parentEventReceivers.Length; i++)
            {
                if (_parentEventReceivers[i] is T)
                    action((T)_parentEventReceivers[i]);
            }
        }

        private void ApplyActionToAllParents<T>(Action<T> action) where T : IEventSystemHandler
        {
            Transform parent = transform.parent;

            while (parent != null)
            {
                foreach (var component in parent.GetComponents<Component>())
                {
                    if (component is T)
                        action((T)(IEventSystemHandler)component);
                }
                parent = parent.parent;
            }
        }

        public override void OnDrag(PointerEventData eventData)
        {
            if (routeToParent)
                DoForParents<IDragHandler>((parent) => { parent.OnDrag(eventData); });
            else
                base.OnDrag(eventData);
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            if (!horizontal && Math.Abs(eventData.delta.x) > Math.Abs(eventData.delta.y))
                routeToParent = true;
            else if (!vertical && Math.Abs(eventData.delta.x) < Math.Abs(eventData.delta.y))
                routeToParent = true;
            else
                routeToParent = false;

            if (routeToParent)
                DoForParents<IBeginDragHandler>((parent) => { parent.OnBeginDrag(eventData); });
            else
                base.OnBeginDrag(eventData);
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            if (routeToParent)
                DoForParents<IEndDragHandler>((parent) => { parent.OnEndDrag(eventData); });
            else
                base.OnEndDrag(eventData);
            routeToParent = false;
        }
    }
}
