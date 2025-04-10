using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using System;

namespace Scripts.BaseSystems.FileIOAndBinary
{
    [CreateAssetMenu(menuName = "Scriptable Obj/Base systems/Core/File IO and binary/Serialization tools")]
    public class SerializationToolsSrc : ScriptableObject, ISerializationTools
    {
        byte[] ISerializationTools.Serialize(object objToSerialize)
        {
            var binaryFormatter = new BinaryFormatter();
            var memoryStream = new MemoryStream();

            binaryFormatter.Serialize(memoryStream, objToSerialize);

            return memoryStream.ToArray();
        }

        T ISerializationTools.Deserialize<T>(byte[] contentToDeserialize)
        {
            var binaryFormatter = new BinaryFormatter();
            var memoryStream = new MemoryStream(contentToDeserialize);
            T t = default;
            try
            {
                t = (T)binaryFormatter.Deserialize(memoryStream);
            }
            catch (Exception e)
            {
                Debug.LogError("\t Deserializing exception: " + e);
            }
            finally
            {
                memoryStream.Close();
            }

            return t;
        }

    }
}
