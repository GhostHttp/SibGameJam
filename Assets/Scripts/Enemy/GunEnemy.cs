using ObjectPool;
using UnityEngine;
using UnityEngine.AI;

public class GunEnemy : MonoBehaviour
{
    [SerializeField] private int _poolBulletSize;
    [SerializeField] private float _attackCooldown;
    //[SerializeField] private float _radiusWandering;
    [SerializeField] private float _distanceToEnemy;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private Transform _parentBullet;
    [SerializeField] private GameObject _prefabBullet;
    [SerializeField] private float _radiusSearchEnemy;
    [SerializeField] private float _cooldownSearchEnemy;
    [SerializeField] private float _minDistanceStrike;
    //[SerializeField] private float _cooldownSearchPointWandering;

    private bool _isGun;
    //private NavMeshPath path;
    private GameObject _enemy;
    private NavMeshAgent _navMA;
    //private Vector3 _targetPosition;
    //private bool _isNewPoint = false;
    private float _timeAttackCooldown;
    private Pool _bulletPool = new Pool();
    private float _timeCooldownSeartchEnemy = 0;
    //private float _timeCooldownSeartchWandering = 0;

    private void Start()
    {
        //path = new NavMeshPath();
        _navMA = GetComponent<NavMeshAgent>();
        _navMA.updateUpAxis = false;
        _navMA.updateRotation = false;
        _bulletPool.CreatePool(_prefabBullet, _poolBulletSize, _parentBullet);
    }
    private void Update()
    {
        if (_isGun)
        {
            Strike();
            if (_enemy == null)
            {
                _isGun = false;
            }
        }
        if (_enemy == null && Time.time >= _timeCooldownSeartchEnemy + _cooldownSearchEnemy) SeartchEnemy();
        else if (_enemy != null)
        {
            //_isNewPoint = true;
            bool point = Movement(_enemy.transform.position, _minDistanceStrike);
            if (point)
            {
                _isGun = true;
            }
        }
        //if (!_isNewPoint && Time.time >= _timeCooldownSeartchWandering + _cooldownSearchPointWandering)
        //{
        //    Vector3 targetPosition = SearchNewPosition();
        //    bool isPath = NavMesh.CalculatePath(this.transform.position, targetPosition, NavMesh.AllAreas, path);
        //    if (isPath && path.status == NavMeshPathStatus.PathComplete)
        //    {
        //        _timeCooldownSeartchWandering = Time.time;
        //        _isNewPoint = true;
        //        _targetPosition = targetPosition;
        //    }
        //    else _isNewPoint = false;
        //}
        //else if (_isNewPoint && _enemy == null && !_isGun)
        //{
        //    bool move = Movement(_targetPosition, _distanceToEnemy);
        //   if (move)
        //    {
        //        _timeCooldownSeartchWandering = Time.time;
        //    }
        //}
    }
    private void SeartchEnemy()
    {
        _timeCooldownSeartchEnemy = Time.time;
        Collider2D[] hits = Physics2D.OverlapCircleAll(this.transform.position, _radiusSearchEnemy);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                _enemy = hit.gameObject;
                return;
            }
        }
        _enemy = null;
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
    //private Vector3 SearchNewPosition()
    //{
    //    Vector2 direction = Random.insideUnitCircle;
    //   Vector2 ofser = direction * _radiusWandering;
    //
    //    Vector3 targetPosition = this.transform.position + (Vector3)ofser;
    //    return targetPosition;
    //}
    private void Strike()
    {
        Vector2 direction = (_enemy.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        if (Time.time < _timeAttackCooldown + _attackCooldown) return;
        Debug.Log("3");
        GameObject obj = _bulletPool.GetObject();
        if (!obj) return;
        obj.transform.position = _attackPoint.position;
        obj.transform.rotation = this.transform.rotation;
        obj.SetActive(true);
        _timeAttackCooldown = Time.time;
    }
}
