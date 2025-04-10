using Scripts.BaseSystems;

namespace Scripts.ProjectSrc
{
    public interface ICameraEventsInvoker 
    {
        public void ApplyCameraSetup(CameraSetupModel cameraSetup);

        public void ResetCamSetup();
        public CameraSetupModel GetInitCamSetup();

        public void RollbackCameraSetup(int steps);
        public CameraSetupModel PeekCameraSetup(int steps);

        public void ApplyPreviousCameraSetup();
        public CameraSetupModel GetPreviousCamSetup();
    }
}
