using System.Collections.Generic;
using UnityEngine;

namespace Scripts.BaseSystems.CameraSetup
{
    public interface ICameraBank
    {
        public Dictionary<CameraId, Camera> CameraDictionary { get; }

        public void RegisterCamera(Camera camera, CameraId cameraId);
        public void UnRegisterCamera(Camera camera);
        public void UnRegisterCamera(CameraId cameraId);

        /// <summary>
        ///     Will return main camera in case of missing the requested one
        /// </summary>
        /// <param name="cameraId"></param>
        /// <returns></returns>
        public Camera GetCamera(CameraId cameraId);
    }
}
