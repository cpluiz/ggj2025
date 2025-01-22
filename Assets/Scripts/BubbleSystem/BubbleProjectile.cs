using UnityEngine;
using System.Collections;

namespace CTBW.BubbleSystem
{
    [RequireComponent(typeof(Rigidbody), typeof(AudioSource))]
    public class BubbleProjectile : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField, Range(0, 0.1f)] protected float _startBubbleSize;
        [SerializeField, Range(0, 0.1f)] protected float _minBubbleSize;
        [SerializeField, Range(0.1f, 0.5f)] protected float _maxBubbleSize;
        [SerializeField, Range(0, 0.1f)] protected float _bubbleIncrement;

        [Header("Projectile Config")]
        [SerializeField, Range(0.001f, 100f)] protected float _projectileSpeed;
        [SerializeField, Range(0.1f, 10f)] protected float _projectileWeight;
        [SerializeField, Range(0.5f, 30f)] protected float _minLifeTime;
        [SerializeField, Range(0.5f, 30f)] protected float _maxLifeTime;

        [Header("SFX")]
        [SerializeField] protected AudioClip[] _popEffects;
        [SerializeField] protected AudioClip[] _shotEffects;

        [Header("Refferences")]
        [SerializeField] protected Transform _bubbleVisual;
        [SerializeField] protected Rigidbody _rb;
        [SerializeField] protected AudioSource _audioSource;

        protected float _currentBubbleSize;
        protected Vector3 _newPosition;
        protected Vector3 _currentBubbleScale;
        protected Vector3 _forwardVector;

        protected bool _isShooting = false;

        public float CurrentBubbleSize { get { return _currentBubbleSize; } }

        protected void Start()
        {
            _currentBubbleScale = _bubbleVisual.localScale = Vector3.one * _startBubbleSize;
            _currentBubbleSize = _startBubbleSize;
        }
        protected void LateUpdate()
        {
            _bubbleVisual.localScale = _currentBubbleScale;
        }
        protected void FixedUpdate()
        {
            if (!_isShooting) return;

            _rb.useGravity = false;
            _rb.AddForce(Physics.gravity * (_rb.mass * _rb.mass));

            //TODO apply erratic behaviour to the bubble movement
        }

        protected void OnDestroy()
        {
            StopAllCoroutines();
        }

        protected void OnCollisionEnter(Collision collision)
        {
            if (!_isShooting) return;
            //TODO diferentiate between environment and enemies
            StartCoroutine(Co_PopBubble());
        }

        public void IncreaseBuble(float deltaTime)
        {
            if (_isShooting) return;

            _currentBubbleSize += _bubbleIncrement * deltaTime;
            _currentBubbleScale.x = _currentBubbleScale.y = _currentBubbleScale.z = _currentBubbleSize;

            if (_currentBubbleSize >= _maxBubbleSize)
            {
                StartCoroutine(Co_PopBubble(true));
            }
        }
        public void ShootBubble()
        {
            if(_currentBubbleSize <= _minBubbleSize)
            {
                StartCoroutine(Co_PopBubble());
                return;
            }

            _forwardVector = transform.forward;

            transform.parent = null;
            _isShooting = true;
            _rb.mass = Mathf.Lerp(_minBubbleSize, _maxBubbleSize, _currentBubbleSize) * _projectileWeight;
            _rb.AddForce(_forwardVector * ( Mathf.InverseLerp(_minBubbleSize, _maxBubbleSize, _currentBubbleSize) * _projectileSpeed), ForceMode.Impulse);

            if (_shotEffects.Length > 0)
            {
                _audioSource.PlayOneShot(_shotEffects[Random.Range(0, _shotEffects.Length)]);
            }
            StartCoroutine(Co_BubbleExpire());
        }

        protected IEnumerator Co_PopBubble(bool playPopSound = false)
        {
            if(_popEffects.Length > 0 && playPopSound)
            {
                AudioClip selectedClip = _popEffects[Random.Range(0, _popEffects.Length)];
                _audioSource.PlayOneShot(selectedClip);
                yield return new WaitForSeconds(selectedClip.length);
            }
            yield return null;
            Destroy(gameObject);
            //TODO Apply bubble exploding VFX
        }

        protected IEnumerator Co_BubbleExpire()
        {
            yield return new WaitForSeconds(Random.Range(_minLifeTime, _maxLifeTime));
            StartCoroutine(Co_PopBubble());
        }
    }
}
