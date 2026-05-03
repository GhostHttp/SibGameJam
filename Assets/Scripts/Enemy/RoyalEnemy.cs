using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class RoyalEnemy : MonoBehaviour
{
    //[SerializeField] private List<Transform> _spawnEnemy = new List<Transform>();
    [SerializeField] private List<GameObject> _prefab = new List<GameObject>();
    [SerializeField] private float _radiusSpawnEnemy;
    [SerializeField] private float _cooldownSpawnEnemy;
    [SerializeField] private int _maxSpawnEnemy;
    [SerializeField] private int _healthCountToMovement;
    [SerializeField] private GameObject _player;
    [SerializeField] private float _cooldownMovement;
    [SerializeField] private float _minPlayerDistance;
    [SerializeField] private float _damage;

    private float _timeCooldownMovement;
    private NavMeshAgent _navMA;
    private Enemy _this;
    private float _timeCooldownSpawnEnemy = 0;
    private int _countSpawnEnemy = 0;

    private void Start()
    {
        _navMA = GetComponent<NavMeshAgent>();
        _navMA.updateUpAxis = false;
        _navMA.updateRotation = false;
        _this = GetComponent<Enemy>();
    }

    private void Update()
    {
        Vector2 direction = (_player.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        if (Time.time >= _timeCooldownSpawnEnemy + _cooldownSpawnEnemy && _countSpawnEnemy < _maxSpawnEnemy && _this.Health > _healthCountToMovement)
        {
            SpawnEnemy();
        }
        if (_this.Health <= _healthCountToMovement && Time.time >= _timeCooldownMovement + _cooldownMovement)
        {
            if (Movement(_player.transform.position, _minPlayerDistance)) { _timeCooldownMovement = Time.time; }
        }
    }
    private void SpawnEnemy()
    {
        GameObject obj = GameObject.Instantiate(_prefab[Random.Range(0, _prefab.Count)]);
        if (obj != null) _timeCooldownSpawnEnemy = Time.time;
        else return;
        obj.transform.position = RandomPosition();
        obj.SetActive(true);
        _countSpawnEnemy++;
    }
    private Vector2 RandomPosition()
    {
        Vector2 direction = Random.insideUnitCircle;
        Vector2 offser = direction * _radiusSpawnEnemy;
        Vector3 targetPosition = this.transform.position + (Vector3)offser;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPosition, out hit, 5f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else return transform.position;
    }
    private bool Movement(Vector3 targetPosition, float distanceToTargetPosition)
    {
        if (Vector2.Distance(this.transform.position, targetPosition) <= distanceToTargetPosition)
        {
            return true;
        }
        _navMA.SetDestination(targetPosition);
        return false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<PlayerStatistic>().TakeDamage(_damage);
            }
        }
    }
}
