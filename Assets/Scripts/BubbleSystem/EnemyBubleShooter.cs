using UnityEngine;

namespace CTBW.BubbleSystem
{
    public class EnemyBubleShooter : BubbleShooter
    {
        [Header("Turret Config")]
        [SerializeField, Range(0f, 180f)] private float _horizontalAngleThreshold;
        [SerializeField, Range(0f, 180f)] private float _verticalAngleThreshold;

        private Quaternion startAngle;
        private Vector3 targetEulerAngle;

        private bool enemyIsShooting = false;
        private float targetBubbleSize;

        private void Awake()
        {
            startAngle = transform.rotation;
        }
        new private void Update()
        {
            if(canShoot && !enemyIsShooting)
            {
                StartEnemyShoot();
            }

            base.Update();

            if (enemyIsShooting && currentProjectile.CurrentBubbleSize >= targetBubbleSize)
            {
                ShootBubble();
                enemyIsShooting = false;
            }
        }
        private void StartEnemyShoot() {
            if (enemyIsShooting) return;

            targetEulerAngle = startAngle.eulerAngles;

            targetEulerAngle.y += Random.Range(-_horizontalAngleThreshold, _horizontalAngleThreshold);
            targetEulerAngle.x -= Random.Range(0, _verticalAngleThreshold);

            transform.eulerAngles = targetEulerAngle;

            enemyIsShooting = true;
            StartShooting();
            targetBubbleSize = currentProjectile.RandomTargetBubbleSize;
        }
    }
}
