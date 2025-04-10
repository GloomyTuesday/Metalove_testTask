using System.Threading.Tasks;

namespace Scripts.BaseSystems.Core
{
    public interface IWaitAndLaunchTool
    {
        /// <summary>
        /// waitMilliseconds - How long to wait untill all objects will become ready
        /// </summary>
        /// <param name="objectsToWait"></param>
        /// <param name="requestSender"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public Task<bool> WaitAndLaunch(object[] objectsToWait, object requestSender, float seconds = 0);

        /// <summary>
        /// waitMilliseconds - How long to wait untill all objects will become ready
        /// </summary>
        /// <param name="objectToWait"></param>
        /// <param name="requestSender"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public Task<bool> WaitAndLaunch(object objectToWait, object requestSender, float seconds = 0);
    }
}
