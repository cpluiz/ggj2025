using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

namespace CTBW.BubbleSystem
{
    public class BubbleShooter : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private float _cooldown;
        [SerializeField] InputActionReference _shootAction;

        [Header("Refferences")]
        [SerializeField] private BubbleProjectile _projectilePrefab;

        private bool canShoot = true;
        private bool isShooting = false;
        private BubbleProjectile currentProjectile;

        private Coroutine cooldownCoroutine;

        private void OnEnable()
        {
            _shootAction.action.performed += ShootAction_performed;
            _shootAction.action.canceled += ShootAction_canceled;
        }
        private void OnDisable()
        {
            _shootAction.action.performed -= ShootAction_performed;
            _shootAction.action.canceled -= ShootAction_canceled;

            if (cooldownCoroutine != null && isShooting)
            {
                cooldownCoroutine = StartCoroutine(Co_RunCooldown());
            }
        }
        private void ShootAction_performed(InputAction.CallbackContext obj)
        {
            if (!canShoot) return;

            isShooting = true;
            canShoot = false;
            currentProjectile = Instantiate<BubbleProjectile>(_projectilePrefab, this.transform);
        }
        private void ShootAction_canceled(InputAction.CallbackContext obj)
        {
            isShooting = false;
            if (currentProjectile != null)
            {
                currentProjectile.ShootBubble();
            }
            if(cooldownCoroutine == null)
            {
                cooldownCoroutine = StartCoroutine(Co_RunCooldown());
            }
        }

        private void Update()
        {
            if (isShooting && currentProjectile != null)
            {
                currentProjectile.IncreaseBuble(Time.deltaTime);
            }
        }

        private IEnumerator Co_RunCooldown()
        {
            Debug.Log("Waiting for cooldown");
            yield return new WaitForSecondsRealtime(_cooldown);
            canShoot = true;
            cooldownCoroutine = null;
            Debug.Log("Can shoot again");
        }
    }
}
