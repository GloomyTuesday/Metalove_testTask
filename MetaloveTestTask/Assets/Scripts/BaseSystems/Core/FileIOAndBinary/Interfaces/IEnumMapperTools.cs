using System;

namespace Scripts.BaseSystems.FileIOAndBinary
{
    public interface IEnumMapperTools 
    {
        public int? GetEnumValue<T>(string str) where T : Enum;
    }
}
