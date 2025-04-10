using UnityEngine;
using System;
using System.Collections.Generic;

namespace Scripts.BaseSystems.FileIOAndBinary
{
    [CreateAssetMenu(menuName = "Scriptable Obj/Base systems/Core/File IO and binary/Enum mapper tools")]
    public class EnumMapperToolsSrc : ScriptableObject, IEnumMapperTools
    {

        private Dictionary<Type, Dictionary<string, int?>> EnumTypeDataDictionary { get; set; } = new Dictionary<Type, Dictionary<string, int?>>(); 

        public int? GetEnumValue<T>(string str) where T : Enum
        {
            var type = typeof(T);

            if (!EnumTypeDataDictionary.ContainsKey(type))
                CompleteDictionary<T>();

            if (!EnumTypeDataDictionary[type].ContainsKey(str)) return null; 

            return EnumTypeDataDictionary[type][str]; 
        }

        private void CompleteDictionary<T>() where T : Enum, IConvertible
        {
            var type = typeof(T);
            EnumTypeDataDictionary.Add(type, new Dictionary<string, int?>());

            var enumValues = (T[])Enum.GetValues(typeof(T));

            foreach (var item in enumValues)
                EnumTypeDataDictionary[type].Add(item.ToString(), Convert.ToInt32(item));
        }

        
    }
}
