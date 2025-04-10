using System.Collections;
using UnityEngine;

namespace Scripts.BaseSystems.UiRelated
{
    public class SafeAreaAdapter : MonoBehaviour
    {
        [SerializeField, Space(10)]
        private RectTransform _rectTransformToAdapt;

        public RectTransform SafeAreaRRectTransform => _rectTransformToAdapt;

        private void OnEnable()
        {
            AdaptRectTransformToSafeArea(_rectTransformToAdapt);
        }

        private IEnumerator Updater(float delay, RectTransform rectTransform)
        {
            while (true)
            {
                AdaptRectTransformToSafeArea(rectTransform); 
                yield return new WaitForSeconds(delay); 
            }
        }

        public void AdaptRectTransformToSafeArea(RectTransform rectToAdapt)
        {
            if (rectToAdapt == null) return;

            var safeArea = Screen.safeArea;

            if (safeArea.width <= 0)
                Debug.LogWarning("\t safeArea width is zero "); 

            if (safeArea.width <= 0 || safeArea.height <= 0 || Screen.width <= 0 || Screen.height <= 0)
            {
                Debug.LogWarning("\t Safe area can't be resized because one of the values may cause deviding by zero: \t" +
                    "\n safeArea : " + safeArea.width + " " + safeArea.height +
                    "\n Screen : " + Screen.width + " " + Screen.height);

                return; 
            }

            var anchorMin = safeArea.position;
            var anchorMax = safeArea.position + safeArea.size;

            if (anchorMax.x > Screen.width)
                anchorMax.x = Screen.width;

            if (anchorMax.y > Screen.height)
                anchorMax.y = Screen.height;

            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            rectToAdapt.anchorMin = anchorMin;
            rectToAdapt.anchorMax = anchorMax;
        }
    }
}
