using UnityEngine;
using System.Collections;
namespace CTBW.TowerSystem
{
    public class Tower : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField, Range(3f, 10f)] protected float _minSpawnInteraval;
        [SerializeField, Range(5f, 20f)] protected float _maxSpawnInteraval;
        [SerializeField, Range(0, 0.7f)] protected float _lateralOffset;

        [SerializeField] protected Soldier _soldierPrefab;

        [Header("References")]
        [SerializeField] protected Transform _spawnPoint;

        protected bool CanSpawn = true;

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
            Vector3 spawnPosition = _spawnPoint.localPosition;

            spawnPosition.x += Random.Range(-_lateralOffset, _lateralOffset);

            Soldier newSoldier = Instantiate<Soldier>(_soldierPrefab, transform.TransformPoint(spawnPosition), _spawnPoint.rotation);
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
    }
}