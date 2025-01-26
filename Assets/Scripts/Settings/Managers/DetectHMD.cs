using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace CTBW.Settings
{
    public class DetectHMD : MonoBehaviour
    {
        [SerializeField] private HeadsetInfo _headsetInfo;
        [SerializeField] private bool _userPresence;
        [SerializeField] private bool _hadUserPresence = false;

        [SerializeField] private ScenePicker _vrMenuScene;
        [SerializeField] private ScenePicker _PCMenuScene;

        private XRDisplaySubsystem xRDisplaySubsystem;
        private InputDevice hmdDevice;

        private void Start()
        {
            CheckForDisplaySubsystem();
        }
        private void CheckForDisplaySubsystem()
        {
            if(
                XRGeneralSettings.Instance == null ||
                XRGeneralSettings.Instance.Manager == null ||
                XRGeneralSettings.Instance.Manager.activeLoader == null
            ){
                _headsetInfo.HMD_DETECTED = false;
                return;
            }

            xRDisplaySubsystem = XRGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<XRDisplaySubsystem>();

            if(xRDisplaySubsystem != null)
            {
                var desiredCharacteristics = InputDeviceCharacteristics.HeadMounted;
                var locatedDevices = new List<InputDevice>();
                InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, locatedDevices);
                if (locatedDevices.Count > 0)
                {
                    hmdDevice = locatedDevices[0];
                    hmdDevice.TryGetFeatureValue(CommonUsages.userPresence, out _userPresence);
                    _hadUserPresence |= _userPresence;
                }
            }
            _headsetInfo.HMD_DETECTED = _hadUserPresence;
        }
    }
}
