using UnityEngine;

namespace Scripts.BaseSystems
{
    public static class UnityEngineObjectExtensions 
    {
        public static T GetComponent<T>(this Object obj) where T : class
        {
            T expectedType; 

            var gameObj = obj as GameObject; 
            if(gameObj!=null)
            {
                expectedType = gameObj.GetComponent<T>(); 
                return expectedType; 
            }

            var scriptObj = obj as ScriptableObject;
            if (scriptObj != null)
            {
                expectedType = scriptObj as T;
                return expectedType;
            }
            return null; 
        }
    }
}
