using NUnit.Framework;
using ObjectPool;
using System.Collections.Generic;
using UnityEngine;

public class PlayerForm4 : MonoBehaviour
{
    [Header("===Переменные формы барьера===")]
    [SerializeField] private PlayerStatistic _player;
    [SerializeField] private GameObject _prefabUnits;
    [SerializeField] private Transform _parentUnits;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _cooldownSpawnUnits;
    [SerializeField] private float _cooldownBarrier;
    [SerializeField] private float _bonusArmor = 2;
    [SerializeField] private int _poolUnitsSize;
    [SerializeField] private float _healthLimited;
    [SerializeField] private int _maxCountUnits;
    [SerializeField] private int _radiusCircleUnits;
    [SerializeField] private Transform _parenCircle;

    private bool _isEnabled = false;
    private bool _barrier = true;
    private Pool _unitsPool = new Pool();
    private float _playerHealth;
    private float _timeCooldown;
    private float _timeCooldownBarrier;
    private List<Vector3> _circleUnitsPosition = new List<Vector3>();
    private List<GameObject> _units = new List<GameObject>();

    public void Enable(bool enable)
    {
        _isEnabled = enable;
        if (!_isEnabled)
        {
            DestroyUnit();
            _player.RemoveBonusArmor();
            _barrier = false;
        }
        else
        {
            if(_units.Count > 0) LoadUnits();
            _player.AddBonusArmor(_bonusArmor);
            _playerHealth = _player.Health;
            _barrier = true;
        }
    }
    private void Start()
    {
        _unitsPool.CreatePool(_prefabUnits, _poolUnitsSize, _parentUnits);
        CreateCircle();
    }
    private void LoadUnits()
    {
        int i = 0;
        foreach(var obj in _units)
        {
            obj.transform.SetParent(_parenCircle);
            obj.transform.localPosition = _circleUnitsPosition[i];
            obj.SetActive(true);
            i++;
        }
    }
    private void CreateCircle()
    {
        _circleUnitsPosition.Clear();
        for (int i = 0; i < _maxCountUnits; i++)
        {
            float angle = i * (360f / _maxCountUnits) * Mathf.Deg2Rad;
            Vector3 position = new Vector3(0, 0, 0) + new Vector3(
                Mathf.Cos(angle) * _radiusCircleUnits,
                Mathf.Sin(angle) * _radiusCircleUnits,
                0
            );
            _circleUnitsPosition.Add(position);
        }
    }
    private void Update()
    {
        if (!_isEnabled) return;
        if (_playerHealth - _player.Health >= _healthLimited && _barrier)
        {
            _barrier = false;
            _player.RemoveBonusArmor();
            _timeCooldownBarrier = Time.time;
        }
        else if (Time.time >= _timeCooldownBarrier + _cooldownBarrier && !_barrier) Barrier();
        if (Time.time >= _timeCooldown + _cooldownSpawnUnits && _isEnabled && _units.Count < _maxCountUnits) SpawnUnits();
    }
    private void SpawnUnits()
    {
        Debug.Log("Вызывается метод спавна юнитов");
        if (_circleUnitsPosition.Count == _units.Count) return;
        GameObject obj = _unitsPool.GetObject();
        Debug.Log(obj.name);
        obj.transform.SetParent(_parenCircle);
        obj.transform.localPosition = _circleUnitsPosition[_units.Count];
        _units.Add(obj);
        _timeCooldown = Time.time;
    }
    private void Barrier()
    {
        _player.AddBonusArmor(_bonusArmor);
        _playerHealth = _player.Health;
        _barrier = true;;
    }
    public void RemoveUnit(GameObject removeUnit)
    {
        if (_units.Count == 0) return;
        _units.Remove(removeUnit);
    }
    private void DestroyUnit()
    {
        foreach (var obj in _units)
        {
            obj.SetActive(false);
        }
    }
}
