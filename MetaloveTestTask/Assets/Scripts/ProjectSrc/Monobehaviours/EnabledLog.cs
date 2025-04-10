using UnityEngine;

namespace Scripts.ProjectSrc
{
    public class EnabledLog : MonoBehaviour
    {
        [SerializeField]
        private string _message = ""; 

        private void OnEnable()
        {
            Debug.Log("\t Enabled: "+ _message);
        }
    }
}
