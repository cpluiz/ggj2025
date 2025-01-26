using UnityEngine;
using System.Collections;
using CTBW.Settings;

namespace CTBW.TowerSystem
{
    public class Tower : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField, Range(3f, 10f)] protected float _minSpawnInteraval;
        [SerializeField, Range(5f, 20f)] protected float _maxSpawnInteraval;
        [SerializeField, Range(0, 0.7f)] protected float _lateralOffset;
        [SerializeField, Range(1, 100)] protected int _maxHits;

        [Header("Soldier Config")]
        [SerializeField] protected Soldier _soldierPrefab;
        [SerializeField] protected LayerMask _soldierLayer;
        [SerializeField, TagSelector] protected string _targetTag;

        [Header("References")]
        [SerializeField] protected Transform _spawnPoint;

        protected bool CanSpawn = true;
        protected int currentHits = 0;

        public int MaxHP { get { return _maxHits; } }
        public int CurrentDamage { get { return currentHits; } }

        protected void Awake()
        {
            StartCoroutine(Co_SpawnSoldier());
        }
        protected void OnDestroy()
        {
            StopAllCoroutines();
        }

        protected void SpawnSoldier()
        {
            Vector3 spawnPosition = _spawnPoint.position;

            spawnPosition.z += Random.Range(-_lateralOffset, _lateralOffset);

            Soldier newSoldier = Instantiate<Soldier>(_soldierPrefab, spawnPosition, _spawnPoint.rotation);
            newSoldier.SetLayer(_soldierLayer);
            newSoldier.SetTargetTag(_targetTag);
        }
        protected IEnumerator Co_SpawnSoldier()
        {
            float nextSpawn = 1.5f;
            while (CanSpawn)
            {
                yield return new WaitForSeconds(nextSpawn);
                SpawnSoldier();
                nextSpawn = Random.Range(_minSpawnInteraval, _maxSpawnInteraval);
            }
        }

        public void TakeHit()
        {
            currentHits++;
        }
    }
}