using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerForm2 : MonoBehaviour
{
    [Header("===Переменные лазерной формы===")]
    [SerializeField] private LineRenderer _lR;
    [SerializeField] private Transform _endpointLaser;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackCooldown;
    [SerializeField] private float _attackDamage;

    private bool _isEnabled = false;
    private bool _attack = false;
    private float _timeCooldown;

    public void Enable(bool enable)
    {
        _isEnabled = enable;
        if (!enable)
        {
            _attack = false;
            _lR.enabled = false;
        }
    }
    public void Attack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _attack = true;
        }
        if(context.canceled)
        {
            _attack = false;
            _lR.enabled = false;
        }
    }
    private void Start()
    {
        _lR.positionCount = 2;
        _lR.startWidth = 0.3f;
        _lR.endWidth = 0.3f;
    }
    private void Update()
    {
        if (!_isEnabled || !_attack) return;
        Attack();
    }
    private void Attack()
    {
        RaycastHit2D hit = Physics2D.Raycast(_attackPoint.position, this.transform.right, 23f);

        _lR.enabled = true;
        _lR.SetPosition(0, _attackPoint.position);

        if (hit.collider != null) _lR.SetPosition(1, hit.point);
        else _lR.SetPosition(1, _endpointLaser.position);

        if (hit.collider != null && Time.time >= _timeCooldown + _attackCooldown || Time.time <= _attackCooldown)
        {
            //Обработка нанесения урона
            Debug.Log($"Лазер попал в {hit.collider.gameObject.name}.");
            _timeCooldown = Time.time;
        }
    }
}
