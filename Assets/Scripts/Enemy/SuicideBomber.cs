using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;

public class SuicideBomber : MonoBehaviour
{
    [Header("===Переменные взрывного монстра===")]
    //[SerializeField] private float _radiusWandering;
    [SerializeField] private float _distanceToEnemy;
    [SerializeField] private float _radiusSearchEnemy;
    [SerializeField] private float _cooldownSearchEnemy;
    //[SerializeField] private float _cooldownSearchPointWandering;
    [SerializeField] private float _damageBomb;
    [SerializeField] private float _radiusBlast;

    private GameObject _enemy;
    private NavMeshAgent _navMA;
    //private bool _isNewPoint = false;
    //private Vector3 _targetPosition;
    private float _timeCooldownSeartchEnemy = 0;
    //private float _timeCooldownSeartchWandering = 0;
    //private Vector3 _lastTargetPosition;

    private void OnEnable()
    {
    //    _isNewPoint = false;
        _timeCooldownSeartchEnemy = 0;
    //    _timeCooldownSeartchWandering = 0;
        _enemy = null;
    }
    private void Start()
    {
        _navMA = GetComponent<NavMeshAgent>();
        _navMA.updateUpAxis = false;
        _navMA.updateRotation = false;
    }
    //private Vector3 SearchNewPosition()
    //{
    //    float randomX = Random.Range(this.transform.position.x - _radiusWandering, this.transform.position.x + _radiusWandering);
    //    float randomY = Random.Range(this.transform.position.y - _radiusWandering, this.transform.position.y + _radiusWandering);

    //    Vector3 targetPosition = new Vector3(randomX, randomY, 0);
    //    return targetPosition;
    //}
    private void Update()
    {
        if (_enemy == null && Time.time >= _timeCooldownSeartchEnemy + _cooldownSearchEnemy) SeartchEnemy();
        else if (_enemy != null)
        {
            //_isNewPoint = true;
            bool point = Movement(_enemy.transform.position, _distanceToEnemy);
            if (point)
            {
                Blast();
                return;
            }
        }
        //if (!_isNewPoint && Time.time >= _timeCooldownSeartchWandering + _cooldownSearchPointWandering)
        //{
        //    Debug.Log("1");
        //   Vector3 targetPosition = SearchNewPosition();
        //
        //    NavMeshPath path = new NavMeshPath();
        //    bool isPath = NavMesh.CalculatePath(this.transform.position, targetPosition, NavMesh.AllAreas, path);
        //    Debug.Log(path.status);
        //    if (isPath && path.status == NavMeshPathStatus.PathComplete)
        //    {
        //        Debug.Log("12");
        //        _timeCooldownSeartchWandering = Time.time;
        //        _isNewPoint = true;
        //        Debug.Log(targetPosition);
        //       _targetPosition = targetPosition;
        //    }
        //    else 
        //    {
        //        Debug.Log("13");
        //        _isNewPoint = false;
        //    }
        //}
        //else if (_isNewPoint && _enemy == null && _targetPosition != _lastTargetPosition)
        //{
        //    Debug.Log("2");
        //    bool move = Movement(_targetPosition, _distanceToEnemy);
        //    if (move)
        //    {
        //        Debug.Log("21");
        //        _lastTargetPosition = _targetPosition;
        //        _isNewPoint = false;
        //        _timeCooldownSeartchWandering = Time.time;
        //    }
        //}
    }
    private bool Movement(Vector3 targetPosition, float distanceToTargetPosition)
    {
        if (Vector2.Distance(this.transform.position, targetPosition) <= distanceToTargetPosition)
        {
            return true;
        }
        Debug.Log("31");
        _navMA.SetDestination(targetPosition);
        return false;
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
        _enemy = null;
        this.gameObject.SetActive(false);
    }
}
