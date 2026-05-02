using UnityEngine;
using UnityEngine.AI;

public class SuicideBomber : MonoBehaviour
{
    [Header("===Переменные взрывного монстра===")]
    [SerializeField] private Enemy _this;
    [SerializeField] private float _radiusWandering;
    [SerializeField] private float _minDistanceToEnemy;
    [SerializeField] private float _radiusSearchEnemy;
    [SerializeField] private float _cooldownSearchEnemy;
    [SerializeField] private float _minDistanceActivateBomb;
    [SerializeField] private float _damageBomb;
    [SerializeField] private float _radiusBlast;
    [SerializeField] private float _cooldownSearchPointWandering;

    private GameObject _enemy;
    private NavMeshAgent _navMA;
    private NavMeshPath path = new NavMeshPath();
    private bool _isNewPoint = false;
    private Vector3 _targetPosition;
    private float _timeCooldownSeartchEnemy = 0;
    private float _timeCooldownSeartchWandering = 0;


    private void Start()
    {
        _navMA = GetComponent<NavMeshAgent>();
    }
    private Vector3 SearchNewPosition()
    {
        Vector2 direction = Random.insideUnitCircle;
        Vector2 ofser = direction * _radiusWandering;

        Vector3 targetPosition = this.transform.position + (Vector3)ofser;
        return targetPosition;
    }
    private void Update()
    {
        if (_enemy == null && Time.time >= _timeCooldownSeartchEnemy+_cooldownSearchEnemy) SeartchEnemy();
        else if(_enemy != null)
        {
            _isNewPoint = true;
            bool point = Movement(_enemy.transform.position, _minDistanceActivateBomb);
            if (point)
            {
                Blast();
                return;
            }
        }
        if (!_isNewPoint && Time.time >= _timeCooldownSeartchWandering+_cooldownSearchPointWandering)
        {
            Vector3 targetPosition = SearchNewPosition();
            NavMeshPath path = new NavMeshPath();
            bool isPath = NavMesh.CalculatePath(this.transform.position, targetPosition, NavMesh.AllAreas, path);
            if (isPath && path.status == NavMeshPathStatus.PathComplete)
            {
                _timeCooldownSeartchWandering = Time.time;
                _isNewPoint = true;
                _targetPosition = targetPosition;
            }
            else _isNewPoint = false;
        }
        else if (_isNewPoint && _enemy == null) Movement(_targetPosition, 0.5f);
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
    private void SeartchEnemy()
    {
        _timeCooldownSeartchEnemy = Time.time;
        Collider2D[] hits = Physics2D.OverlapCircleAll(this.transform.position, _radiusSearchEnemy);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("player"))
            {
                _enemy = hit.gameObject;
                return;
            }
        }
        _enemy = null;
    }
    private void Blast()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(this.transform.position, _radiusBlast);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                hit.GetComponent<PlayerStatistic>().TakeDamage(_damageBomb);
            }
            else if (hit.CompareTag("UnitsPlayer"))
            {
                hit.GetComponent<Units>().TakeDamage(_damageBomb);
            }
        }
        this.gameObject.SetActive(false);
    }
}
