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
            if(LayerMask.LayerToName(collision.gameObject.layer) == "Bubble")
            {
                //TODO give player some gold/points/wathever
                DestroySoldier();
            }
            if (LayerMask.LayerToName(collision.gameObject.layer) == "Tower")
            {
                //TODO remove health from tower
                DestroySoldier();
            }
        }

        [ContextMenu("SetSoldierImage")]
        public void SetSoldierImage()
        {
            _spriteRenderer.sprite = _soldierSprites[Random.Range(0, _soldierSprites.Length)];
        }
        public void DestroySoldier()
        {
            if (_rb.useGravity) return;
            _rb.useGravity = true;
            _rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            _rb.AddRelativeTorque(_angularVelocityWhenDestroying, ForceMode.Impulse);
            StartCoroutine(Co_DestroySoldier());
        }

        protected IEnumerator Co_DestroySoldier()
        {
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
        }
    }
}