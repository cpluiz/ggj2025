using UnityEngine;

namespace CTBW.BubbleSystem
{
    [RequireComponent(typeof(Rigidbody))]
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

        [Header("Refferences")]
        [SerializeField] protected Transform _bubbleVisual;
        [SerializeField] protected Rigidbody _rb;

        protected float _currentBubbleSize;
        protected Vector3 _newPosition;
        protected Vector3 _currentBubbleScale;
        protected Vector3 _forwardVector;
        
        [SerializeField]
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
            //TODO change "weight" of the bubble with inverse proportion to the bubble size
        }

        public void IncreaseBuble(float deltaTime)
        {
            if (_isShooting) return;

            _currentBubbleSize += _bubbleIncrement * deltaTime;
            _currentBubbleScale.x = _currentBubbleScale.y = _currentBubbleScale.z = _currentBubbleSize;

            if (_currentBubbleSize >= _maxBubbleSize)
            {
                PopBubble();
            }
        }
        public void ShootBubble()
        {
            if(_currentBubbleSize <= _minBubbleSize)
            {
                PopBubble();
                return;
            }

            _forwardVector = transform.forward;

            transform.parent = null;
            _isShooting = true;
            _rb.mass = Mathf.Lerp(_minBubbleSize, _maxBubbleSize, _currentBubbleSize) * _projectileWeight;
            _rb.AddForce(_forwardVector * ( Mathf.InverseLerp(_minBubbleSize, _maxBubbleSize, _currentBubbleSize) * _projectileSpeed), ForceMode.Impulse);
        }

        protected void PopBubble()
        {
            //TODO apply sound effect of bubble exploding
            Destroy(gameObject);
        }
    }
}
