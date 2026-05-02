using ObjectPool;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerForm1 : MonoBehaviour
{
    [Header("===Переменные стреляющей формы===")]
    [SerializeField] private GameObject _prefabBullet;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private Transform _parentBullet;
    [SerializeField] private int _poolBulletSize;
    [SerializeField] private float _attackCooldown;

    private bool _isEnabled;
    private bool _attack;
    private float _timeCooldown = 0;
    private Pool _bulletPool = new Pool();

    public void Enable(bool enable)
    {
        _isEnabled = enable;
        if (!enable)
        {
            _attack = false;
        }
    }
    public void Attack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _attack = true;
        }
        if (context.canceled)
        {
            _attack = false;
        }
    }
    private void Start()
    {
        _bulletPool.CreatePool(_prefabBullet, _poolBulletSize, _parentBullet);
    }
    private void Update()
    {
        if (!_isEnabled || !_attack) return;
        if (Time.time > _timeCooldown + _attackCooldown) Strike();
    }
    private void Strike()
    {
        Debug.Log("3");
        GameObject obj = _bulletPool.GetObject();
        if (!obj) return;
        obj.transform.position = _attackPoint.position;
        obj.transform.rotation = this.transform.rotation;
        obj.SetActive(true);
        _timeCooldown = Time.time;
    }
}
