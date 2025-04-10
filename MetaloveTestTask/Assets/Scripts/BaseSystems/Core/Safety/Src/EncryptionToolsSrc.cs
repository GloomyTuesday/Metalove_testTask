using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Scripts.BaseSystems.Safety
{
    [CreateAssetMenu(menuName = "Scriptable Obj/Base systems/Core/Safety/Encryption tools")]
    public class EncryptionToolsSrc : ScriptableObject, IEncryptionTools
    {
        private IEncryptionTools _iEncryptionTools;
        private IEncryptionTools IEncryptionTools
        {
            get
            {
                if (_iEncryptionTools == null)
                    _iEncryptionTools = this;

                return _iEncryptionTools; 
            }
        }

        byte[] IEncryptionTools.EncryptSha(string input)
        {
            byte[] hash;
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                hash = sha256.ComputeHash(inputBytes);
            }
            return hash;
        }

        string IEncryptionTools.EncryptShaAsString(string input, byte groupSize)
        {
            var maxGroupSize = 4;
            var bytes = IEncryptionTools.EncryptSha(input);
            groupSize = (byte)(maxGroupSize % 1000);   //  Be cause groupd size can't be bigger than 4

            List<byte> bytesArrayTransformedList = new List<byte>();

            Func<int, int> GetMultiplier = delegate (int size)
            {
                if (size == 0) return 1;
                byte localDivResult = 1;
                for (int n = 0; n < size; n++)
                    localDivResult *= 10;
                return localDivResult;
            };

            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)Mathf.Abs(bytes[i]); 
                if (bytes[i] >= 100) bytesArrayTransformedList.Add((byte)(bytes[i] / 100));
                if (bytes[i] >= 10) bytesArrayTransformedList.Add((byte)(bytes[i] % 100 / 10));
                bytesArrayTransformedList.Add((byte)(bytes[i] % 10));
            }

            int count = 2;
            int buffer = 0;
            string resultStr = "";

            for (int i = 0; i < bytesArrayTransformedList.Count; i++)
            {
                if (count < groupSize)
                    buffer = buffer + bytesArrayTransformedList[i] * GetMultiplier(count);

                if (count <= 0)
                {
                    resultStr += Convert.ToChar(buffer);
                    count = 2;
                    buffer = 0;
                }
                else
                    count--;
            }

            return resultStr;
        }
    }
}
