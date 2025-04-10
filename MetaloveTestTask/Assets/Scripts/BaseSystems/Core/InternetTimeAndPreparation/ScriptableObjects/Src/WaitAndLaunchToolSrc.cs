using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace Scripts.BaseSystems.Core
{
    [CreateAssetMenu(menuName = "Scriptable Obj/Base systems/Core/Internet, time and preparation/Wait and launch tool")]
    public class WaitAndLaunchToolSrc : ScriptableObject, IWaitAndLaunchTool
    {
        private IWaitAndLaunchTool _iWaitAndLaunchTool; 
        private IWaitAndLaunchTool IWaitAndLaunchTool
        {
            get
            {
                if (_iWaitAndLaunchTool == null)
                    _iWaitAndLaunchTool = this;

                return _iWaitAndLaunchTool;
            }
        }

        async Task<bool> IWaitAndLaunchTool.WaitAndLaunch(object[] objectsToWait, object requestSender, float  seconds)
        {
            if (objectsToWait == null) return true;

            var result = ExtractComponentsIReady(objectsToWait);
            List<IReady> iReadyList = new List<IReady>(result);

            var endTime = Time.realtimeSinceStartup + seconds;

            Debug.Log("\t - " + requestSender.ToString() + " - is waiting for: ");

            for (int i = 0; i < objectsToWait.Length; i++)
            {
                Debug.Log("\t\t [ " + i + " ] " + objectsToWait[i].ToString());
            }

            while (Time.realtimeSinceStartup <= endTime || iReadyList.Count > 0)
            {
                for (int i = iReadyList.Count - 1; i >= 0; i--)
                {
                    if (iReadyList[i].Ready)
                    { 
                        iReadyList.RemoveAt(iReadyList.Count - 1);
                    }
                }

                if (iReadyList.Count < 1)
                {
                    Debug.Log("\t\t - Wait request sender: " + requestSender.ToString() + " - result: " + true);
                    return true;
                }
                await Task.Yield();
            }

            Debug.Log("\t\t -  Wait request sender: " + requestSender.ToString() + " - result: " + false);
            return false;
        }

        /// <summary>
        ///     Game object should contain IReady interface. Return true if all objects are rady and fals wjen at least one of the is not
        /// </summary>
        /// <returns></returns>
        async Task<bool> IWaitAndLaunchTool.WaitAndLaunch(object objectToWait , object requestSender, float seconds)
        {
            if (objectToWait == null) return true;

            var awaitable = IWaitAndLaunchTool.WaitAndLaunch(new object[] { objectToWait }, requestSender, seconds);
            
            return await awaitable.ConfigureAwaitAuto();
        }

        private List<IReady>  ExtractComponentsIReady(object[] objectsToWait)
        {
            List<IReady> iReadyList = new List<IReady>();
            List<WeakReference<object>> objectsThatAreNotReadyList = new List<WeakReference<object>>();

            if (objectsToWait == null) return iReadyList;

            for (int i = 0; i < objectsToWait.Length; i++)
            {
                var iReady = objectsToWait[i] as IReady;

                if (iReady != null)
                {
                    iReadyList.Add(iReady);
                    objectsThatAreNotReadyList.Add(objectsToWait[i] as WeakReference<object>); 
                }
            }

            return iReadyList;
        }

    }
}
