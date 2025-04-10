using System;
using UnityEngine;

namespace Scripts.BaseSystems
{
    public class FilterByType : PropertyAttribute
    {
        private Type _filterType;
        public Type FilterType => _filterType;

        public FilterByType(Type typeToSerialize)
        {
            _filterType = typeToSerialize;
        }
    }
}
