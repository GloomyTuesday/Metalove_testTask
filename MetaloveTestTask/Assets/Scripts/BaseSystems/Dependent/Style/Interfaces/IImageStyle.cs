using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.BaseSystems.Style
{
    public interface IImageStyle 
    {
        public ImageStyleId[] AvailableStyleId { get; }

        public Sprite GetSprite(ImageStyleId styleId);
        public Color GetColor(ImageStyleId styleId);
        public Material GetMaterial(ImageStyleId styleId);
        public Image.Type GetType(ImageStyleId styleId);
        public bool GetRaycastTarget(ImageStyleId styleId);

        /// <summary>
        ///     first item is offset min
        ///     second item is offset max
        /// </summary>
        /// <param name="styleId"></param>
        /// <returns></returns>
        public (Vector2,Vector2) GetOffset(ImageStyleId styleId);
    }

}
