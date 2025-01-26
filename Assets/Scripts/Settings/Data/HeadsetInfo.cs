using UnityEngine;

namespace CTBW.Settings
{
    [CreateAssetMenu(fileName = "HeadsetInfo", menuName = "Scriptable Objects/HeadsetInfo")]
    public class HeadsetInfo : ScriptableObject
    {
        public bool HMD_DETECTED;
    }
}
