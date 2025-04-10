using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts.BaseSystems.InternetTools
{
    [CreateAssetMenu(fileName = "InternetTools", menuName = "Scriptable Obj/Base systems/Core/Internet Tools/Internet tools")]
    internal class InternetToolsSrc : ScriptableObject, IInternetTools
    {
        StreamReader streamReader = null;

        async Task<string> IInternetTools.TryToGetRequestAnswerTcp( string url , int port )
        {
            try
            {
                var client = new TcpClient();
                await client.ConnectAsync(url,port).ConfigureAwait(true);

                using (streamReader = new StreamReader(client.GetStream()))
                {
                    var answer = await streamReader.ReadToEndAsync().ConfigureAwait(true);
                    return answer;
                }
            }
            catch(Exception exception)
            {
                Debug.LogWarning(" " + name + "\t Try to get request answer Tcp FAILED \t url: " + url + "\n message: " + exception);
            }
            return null;
        }
    }
}
