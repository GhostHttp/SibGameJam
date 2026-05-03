using UnityEngine;
using UnityEngine.AI;

public class Units : MonoBehaviour
{

    [SerializeField] private float _radiusSearchEnemy;
    [SerializeField] private float _cooldownSearchEnemy;
    [SerializeField] private float _radiusBlast;
    [SerializeField] private float _distanceEnemy;
    [SerializeField] private float _damage;
    [SerializeField] private PlayerForm4 _pF4;

    private NavMeshAgent _navMA;
    private GameObject _enemy;
    private bool _searchEnemy = true;
    private float _timeCooldownSearchEnemy = 0;


    private void Start()
    {
        _navMA = GetComponent<NavMeshAgent>();

        _navMA.updateUpAxis = false;
        _navMA.updateRotation = false;
    }
    private void OnEnable()
    {
        _searchEnemy = true;
        _timeCooldownSearchEnemy = 0;
    }
    private void Update()
    {
        if (_searchEnemy && Time.time > _timeCooldownSearchEnemy + _cooldownSearchEnemy) SearchEnemy();
        else if (_enemy != null) Move();
        if (_enemy != null && !_enemy.activeInHierarchy)
        {
            _enemy = null;
            _searchEnemy = true;
        }
    }
    private void SearchEnemy()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(this.transform.position, _radiusSearchEnemy);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("enemy"))
            {
                _searchEnemy = false;
                _enemy = hit.gameObject;
                return;
            }
        }
        _timeCooldownSearchEnemy = Time.time;
    }
    private void Blast()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(this.transform.position, _radiusBlast);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("enemy"))
            {
                hit.GetComponent<Enemy>().TakeDamage(_damage);
                _pF4.RemoveUnit(this.gameObject);

            }
        }
        this.gameObject.SetActive(false);
    }
    private void Move()
    {
        if (Vector2.Distance(this.transform.position, _enemy.transform.position) <= _distanceEnemy)
        {
            Blast();
            _enemy = null;
            return;
        }
        transform.SetParent(null);
        _navMA.SetDestination(_enemy.transform.position);
    }
    public void TakeDamage(float damage)
    {
        if (damage <= 0) return;
        this.gameObject.SetActive(false);
    }
}
