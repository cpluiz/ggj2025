using UnityEngine;
using UnityEngine.UI;

namespace CTBW.TowerSystem
{
    public class TowerUI : MonoBehaviour
    {
        [SerializeField] private Tower _tower;
        [SerializeField] private Slider _healthSlider;

        private void LateUpdate()
        {
            _healthSlider.value = Mathf.InverseLerp(_tower.MaxHP, 0, _tower.CurrentDamage);
        }
    }
}
