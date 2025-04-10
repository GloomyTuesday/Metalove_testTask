using System.Collections.Generic;
using UnityEngine;
using System;

/*
 
  In order to make this tool work, all camera object should have same name as their Id, because this tool will seaerch those objects by those names.  

*/

namespace Scripts.BaseSystems.CameraSetup
{
    [CreateAssetMenu(menuName = "Scriptable Obj/Base systems/Core/Camera setup/Camera description bank")]
    internal class CameraBankSrc : ScriptableObject, ICameraBank
    {
        [SerializeField,Header("Registered cameras")]
        private List<CameraDataDescription> _cameraList = new List<CameraDataDescription>(); 

        [Serializable]
        private class CameraDataDescription
        {
            public CameraId _cameraId;
            public Camera _camera; 
        }

        private Dictionary<CameraId, Camera> _cameraDictionary = new Dictionary<CameraId, Camera>();
        public Dictionary<CameraId, Camera> CameraDictionary
        {
            get
            {
                if (MainCamera == null)
                {
                    MainCamera = Camera.main;
                    RegisterCamera(MainCamera, CameraId.Main); 
                }

                return _cameraDictionary; 
            }
        }
        private Camera MainCamera { get; set; }

        public void RegisterCamera( Camera camera, CameraId cameraId )
        {
            Debug.Log("\t Request to register camera: "+camera.name+"\t id: "+cameraId); 
            if(cameraId==CameraId.Non)
            {
                Debug.LogError("You can't register camera with Id Non"); 
                return; 
            }

            CleareFromNulls();

            if (!_cameraDictionary.ContainsKey(cameraId))
            {
                _cameraDictionary.Add(cameraId, camera);
                return; 
            }

            if (_cameraDictionary[cameraId] == camera) return; 

            Debug.Log("\t Camera with id: "+ cameraId + " was substituted."); 
            _cameraDictionary[cameraId] = camera; 
        }

        public void UnRegisterCamera(Camera camera)
        {
            var cameraIdToRemove = CameraId.Non;

            foreach (var item in CameraDictionary)
            {
                if(item.Value == camera)
                {
                    cameraIdToRemove = item.Key;
                    break; 
                }
            }

            if (cameraIdToRemove == CameraId.Non) return;

            CameraDictionary.Remove(cameraIdToRemove); 
        }

        public void UnRegisterCamera(CameraId cameraId)
        {
            if (CameraDictionary.ContainsKey(cameraId))
                CameraDictionary.Remove(cameraId); 
        }

        private void CleareFromNulls()
        {
            var idToRemove = new List<CameraId>();

            foreach (var item in CameraDictionary)
            {
                if (item.Value == null)
                    idToRemove.Add(item.Key); 
            }

            for (int i = 0; i < idToRemove.Count; i++)
                CameraDictionary.Remove(idToRemove[i]); 
        }

        public Camera GetCamera(CameraId cameraId)
        {
            if (!CameraDictionary.ContainsKey(cameraId))
            {
                return Camera.main;
            }

            return CameraDictionary[cameraId]; 
        }
    }
}

