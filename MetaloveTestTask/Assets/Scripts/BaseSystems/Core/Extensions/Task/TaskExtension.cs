using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Scripts.BaseSystems
{
    public static class TaskExtension
    {

        private static bool IsMultiThreadSupported
        {
            get
            {
#if UNITY_WEBGL
            return false;
#else
            return true;
#endif
            }
        }

        public static ConfiguredTaskAwaitable<T> ConfigureAwaitAuto<T>(this Task<T> task)
        {
            return task.ConfigureAwait(!IsMultiThreadSupported);
        }

        public static ConfiguredTaskAwaitable ConfigureAwaitAuto(this Task task)
        {
            return task.ConfigureAwait(!IsMultiThreadSupported);
        }
    }
}
