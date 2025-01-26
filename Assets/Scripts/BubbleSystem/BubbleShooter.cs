using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

namespace CTBW.BubbleSystem
{
    public class BubbleShooter : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] protected float _cooldown;
        [SerializeField] private InputActionReference _shootAction;
        [SerializeField] protected LayerMask _targetLayer;

        [Header("Refferences")]
        [SerializeField] protected BubbleProjectile _projectilePrefab;

        protected bool canShoot = true;
        protected bool isShooting = false;
        protected BubbleProjectile currentProjectile;

        protected Coroutine cooldownCoroutine;

        private void OnEnable()
        {
            if (_shootAction == null) return;
            _shootAction.action.performed += ShootAction_performed;
            _shootAction.action.canceled += ShootAction_canceled;
        }
        private void OnDisable()
        {
            if (_shootAction == null) return;
            _shootAction.action.performed -= ShootAction_performed;
            _shootAction.action.canceled -= ShootAction_canceled;

            if (cooldownCoroutine != null && isShooting)
            {
                cooldownCoroutine = StartCoroutine(Co_RunCooldown());
            }
        }
        protected void Update()
        {
            if (isShooting && currentProjectile != null)
            {
                currentProjectile.IncreaseBuble(Time.deltaTime);
            }
        }
        private void ShootAction_performed(InputAction.CallbackContext obj)
        {
            StartShooting();
        }
        private void ShootAction_canceled(InputAction.CallbackContext obj)
        {
            ShootBubble();
        }
        protected void StartShooting()
        {
            if (!canShoot) return;

            isShooting = true;
            canShoot = false;
            currentProjectile = Instantiate<BubbleProjectile>(_projectilePrefab, this.transform);
            currentProjectile.SetTargetLayer(_targetLayer);
        }
        protected void ShootBubble()
        {
            if (!isShooting) return;

            isShooting = false;
            if (currentProjectile != null)
            {
                currentProjectile.ShootBubble();
            }
            if (cooldownCoroutine == null)
            {
                cooldownCoroutine = StartCoroutine(Co_RunCooldown());
            }
        }

        protected IEnumerator Co_RunCooldown()
        {
            yield return new WaitForSecondsRealtime(_cooldown);
            canShoot = true;
            cooldownCoroutine = null;
        }
    }
}
