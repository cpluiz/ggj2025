using UnityEngine;
using System.Collections;

namespace CTBW.TowerSystem
{
    public class Soldier : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField, Range(0.1f, 2f)] protected float _soldierSpeed;
        [SerializeField] protected Vector3 _angularVelocityWhenDestroying;
        [Header("Resources")]
        [SerializeField] protected Sprite[] _soldierSprites;
        [Header("Reference")]
        [SerializeField] protected SpriteRenderer _spriteRenderer;
        [SerializeField] protected Rigidbody _rb;

        private string targetTag;
        protected void Awake()
        {
            SetSoldierImage();
        }

        protected void Update()
        {
            if (_rb.useGravity) return;
            transform.position += transform.forward * _soldierSpeed * Time.deltaTime;
        }
        protected void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.CompareTag(targetTag))
            {
                Tower targetTower = collision.gameObject.GetComponentInParent<Tower>();
                if(targetTower != null)
                {
                    SoldierReachedTower(targetTower);
                }                
            }
        }

        [ContextMenu("SetSoldierImage")]
        public void SetSoldierImage()
        {
            _spriteRenderer.sprite = _soldierSprites[Random.Range(0, _soldierSprites.Length)];
        }
        public void SetLayer(LayerMask layer)
        {
            gameObject.layer = 1 << layer;
        }
        public void SetTargetTag(string targetTag)
        {
            this.targetTag = targetTag;
        }
        public void SoldierReachedTower(Tower targetTower)
        {
            targetTower.TakeHit();
            StartCoroutine(Co_DestroySoldier());
        }
        public void BubbleReachedSoldier()
        {
            if (_rb.useGravity) return;
            _rb.useGravity = true;
            _rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            _rb.AddRelativeTorque(_angularVelocityWhenDestroying, ForceMode.Impulse);
            StartCoroutine(Co_DestroySoldier(1));
        }
        protected void DestroySoldier()
        {
            StartCoroutine(Co_DestroySoldier());
        }

        protected IEnumerator Co_DestroySoldier(float destroyAfterSeconds = 0)
        {
            yield return new WaitForSeconds(destroyAfterSeconds);
            Destroy(gameObject);
        }
    }
}