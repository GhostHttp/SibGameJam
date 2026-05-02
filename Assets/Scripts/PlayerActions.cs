using ObjectPool;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;  

public class PlayerActions : MonoBehaviour
{
    [Header("===Основные пременные===")]
    [SerializeField] private List<FormPlayer> _form = new List<FormPlayer>();
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _cooldownTransformation;

    [Header("===Переменные для стрельбы===")]
    [SerializeField] private GameObject _prefabBullet;
    [SerializeField] private Transform _parentBullet;
    [SerializeField] private int _poolBulletSize;

    [Header("===Переменные для лазера===")]
    [SerializeField] private LineRenderer _lR;
    [SerializeField] private Transform _endpointLaser;

    [Header("===Переменные для барьера===")]
    [SerializeField] private PlayerStatistic _player;
    [SerializeField] private GameObject _prefabUnits;
    [SerializeField] private Transform _parentUnits;
    [SerializeField] private float _cooldownBarrier;
    [SerializeField] private float _factorArmor = 2;
    [SerializeField] private int _poolUnitsSize;
    [SerializeField] private float _timeBarrier;

    // 0-Пушка
    // 1-Лазер
    // 2-Рот
    // 3-БарьерМатка

    private bool _barrier = true;
    private Pool _poolBullet = new Pool();
    private Pool _poolUnits = new Pool();
    private SpriteRenderer _sr;
    private int _numberCurrentForm = 0;
    private bool _attack = false;
    private bool _isFactorArmor = false;
    private float _timeTransformationCooldown = 0;
    private float _timeAttackCooldown = 0;
    private float _timeSpawnUnitsCooldown = 0;
    private float _timeBarrierCooldown = 0;
    private float _timeBarrierTime = 0;

    private void Start()
    {
        _sr = GetComponent<SpriteRenderer>();

        _poolUnits.CreatePool(_prefabUnits, _poolUnitsSize, _parentUnits);
        _poolBullet.CreatePool(_prefabBullet, _poolBulletSize, _parentBullet);

        _lR.positionCount = 2;
        _lR.startWidth = 0.3f;
        _lR.endWidth = 0.3f;
    }

    public void Transformation1(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (Time.time < _timeTransformationCooldown + _cooldownTransformation) return;
            if (_form.Count == 0) return;
            if (_numberCurrentForm == 3) _player.RemoveBonusArmor();

            _sr.sprite = _form[0].formSprite;
            _timeAttackCooldown = 0;
            _numberCurrentForm = 0;

            _timeTransformationCooldown = Time.time;
        }
    }
    public void Transformation2(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (Time.time < _timeTransformationCooldown + _cooldownTransformation) return;
            if (_form.Count == 0) return;
            if (_numberCurrentForm == 3) _player.RemoveBonusArmor();

            _sr.sprite = _form[1].formSprite;
            _timeAttackCooldown = 0;
            _numberCurrentForm = 1;

            _timeTransformationCooldown = Time.time;
        }
    }
    public void Transformation3(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (Time.time < _timeTransformationCooldown + _cooldownTransformation) return;
            if (_form.Count == 0) return;
            if (_numberCurrentForm == 3) _player.RemoveBonusArmor();

            _sr.sprite = _form[2].formSprite;
            _timeAttackCooldown = 0;
            _numberCurrentForm = 2;

            _timeBarrierTime = Time.time;
            _timeTransformationCooldown = Time.time;
        }
    }
    public void Transformation4(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (Time.time < _timeTransformationCooldown + _cooldownTransformation) return;
            if (_form.Count == 0) return;

            _player.AddBonusArmor(_factorArmor);
            _isFactorArmor = true;
            _sr.sprite = _form[3].formSprite;
            _timeAttackCooldown = 0;
            _numberCurrentForm = 3;

            _timeTransformationCooldown = Time.time;
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (_numberCurrentForm == 2) MouthAttack();
            _attack = true;
        }
        else if (context.canceled)
        {
            _attack = false;
            _lR.enabled = false;
        }
    }

    private void Update()
    {
        if (_numberCurrentForm == 3)
        {
            if(Time.time >= _timeSpawnUnitsCooldown + _form[_numberCurrentForm].attackCooldown) SpawnUnits();
        }
        else if (_attack && _numberCurrentForm == 1)
        {
            LaserAttack();
        }
        else if (_attack && _numberCurrentForm == 0)
        {
            if (Time.time >= _timeAttackCooldown + _form[_numberCurrentForm].attackCooldown) CannonAttack();
        }
    }

    private void MouthAttack()
    {
        RaycastHit2D hit = Physics2D.Raycast(_attackPoint.position, this.transform.right, 0.5f);

        if (hit.collider != null)
        {
            //Обработка нанесения урона
            Debug.Log($"Укусил {hit.collider.gameObject.name}.");
        }
    } //Моментальная атака с укусами
    private void CannonAttack()
    {
        GameObject obj = _poolBullet.GetObject();
        obj.transform.position = _attackPoint.position;
        obj.transform.rotation = this.transform.rotation;
        _timeAttackCooldown = Time.time;
    } //Атака стреляющая
    private void LaserAttack()
    {
        RaycastHit2D hit = Physics2D.Raycast(_attackPoint.position, this.transform.right, 23f);
        _lR.enabled = true;
        _lR.SetPosition(0, _attackPoint.position);
        if(hit.collider != null) _lR.SetPosition(1, hit.point);
        else _lR.SetPosition(1, _endpointLaser.position);

        if (hit.collider != null)
        {
            //Обработка нанесения урона
            Debug.Log($"Лазер попал в {hit.collider.gameObject.name}.");
        }
    } //Атака лазером
    private void SpawnUnits()
    {
        //if (!_barrier && Time.time >= _cooldownBarrier + _timeBarrierCooldown)
        //{
         //   _timeBarrierTime = Time.time;
        //    _barrier = true;
        //}
        //if (_barrier && Time.time >= _timeBarrierTime + _timeBarrier)
        //{
        //    _timeBarrierCooldown = Time.time;
        //    _barrier = false;
        //}
        GameObject obj = _poolUnits.GetObject();
        obj.transform.position = _attackPoint.position;
        obj.SetActive(true);
    } //Спавн юнитов маткой
}
[System.Serializable]
public class FormPlayer
{
    public Sprite formSprite;
    public float formAttackDamage;
    public float attackCooldown;
}

