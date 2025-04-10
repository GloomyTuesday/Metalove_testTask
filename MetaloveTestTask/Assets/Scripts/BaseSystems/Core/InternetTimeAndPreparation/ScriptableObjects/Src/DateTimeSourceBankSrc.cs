using UnityEngine;
using System.Collections.Generic; 

namespace Scripts.BaseSystems.Core
{
    [CreateAssetMenu(menuName = "Scriptable Obj/Base systems/Core/Internet, time and preparation/Date time source bank")]
    public class DateTimeSourceBankSrc : ScriptableObject
    {

        [SerializeField]
        private DateTimeSourceContainer[] _dateTimeSource;

        [SerializeField,HideInInspector]
        private int _dateTimeSourceHash;

        private int _socketArrayVerificationHash;
        private SocketContainer[] _socketContainer;

        private int _dateTimeSourceIdAddressVerificationHash; 
        private Dictionary<DateTimeSourceId, string> _dateTimeSourceIdAddress = new Dictionary<DateTimeSourceId, string>();

        private int _dateTimeSourceIdPortVerificationHash;
        private Dictionary<DateTimeSourceId, int> _dateTimeSourceIdPort = new Dictionary<DateTimeSourceId, int>();

        public DateTimeSourceContainer[] DateTimeSourceContainer => _dateTimeSource;

        public void UpdateVerificationCach()
        {
            _dateTimeSourceHash = _dateTimeSource.GetHashCode();
        }

        public Dictionary<DateTimeSourceId, string> DateTimeSourceIdAddress
        {
            get
            {
                if (_dateTimeSourceIdAddressVerificationHash != _dateTimeSourceHash)
                {
                    _dateTimeSourceIdAddress.Clear(); 

                    for (int i = 0; i < _dateTimeSource.Length; i++)
                    {
                        _dateTimeSourceIdAddress.Add(_dateTimeSource[i]._dateTimeSourceId, _dateTimeSource[i]._socket._url);
                    }

                    _dateTimeSourceIdAddressVerificationHash = _dateTimeSourceHash; 
                }
                return _dateTimeSourceIdAddress;
            }
        }

        public SocketContainer[] SocketContainer
        {
            get
            {
                if (_socketArrayVerificationHash != _dateTimeSourceHash)
                {
                    _socketContainer = new SocketContainer[_dateTimeSource.Length ]; 
                    for (int i = 0; i < _socketContainer.Length; i++)
                    {
                        _socketContainer[i] = _dateTimeSource[i]._socket; 
                    }

                    _socketArrayVerificationHash = _dateTimeSourceHash;
                }
                return _socketContainer;
            }
        }

        public Dictionary<DateTimeSourceId, int> DateTimeSourceIdPort
        {
            get
            {
                if (_dateTimeSourceIdPortVerificationHash != _dateTimeSourceHash)
                {
                    _dateTimeSourceIdPort.Clear();

                    for (int i = 0; i < _dateTimeSource.Length; i++)
                    {
                        _dateTimeSourceIdPort.Add(_dateTimeSource[i]._dateTimeSourceId, _dateTimeSource[i]._socket._portNumber);
                    }

                    _dateTimeSourceIdPortVerificationHash = _dateTimeSourceHash;
                }
                return _dateTimeSourceIdPort;
            }
        }

        public DateTimeSourceContainer[] DateTimeSource
        {
            get => _dateTimeSource;
            set
            {
                _dateTimeSource = value;
                UpdateVerificationCach();
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_dateTimeSource == null) return; 

            Dictionary<DateTimeSourceId, bool> approvedContainers = new Dictionary<DateTimeSourceId, bool>();

            for (int i = 0; i < _dateTimeSource.Length; i++)
            {

                if (approvedContainers.ContainsKey(_dateTimeSource[i]._dateTimeSourceId))
                    Debug.LogError("\t " + name + " contains element: " + i + " which id: " + _dateTimeSource[i]._dateTimeSourceId + " is already registered ");
                else
                    approvedContainers.Add(_dateTimeSource[i]._dateTimeSourceId, true);
            }

            UpdateVerificationCach();
        }
#endif



        
    }
}
