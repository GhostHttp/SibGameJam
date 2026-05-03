using System.Collections.Generic;
using UnityEngine;

public class Occurrenct : MonoBehaviour
{
    [SerializeField] private List<OccurrenctInfo> _listOccurrenctInfo;
    [SerializeField] private float _cooldownOccurrenctSearch;
    [SerializeField] private PlayerStatistic _player;

    private OccurrenctInfo _thisEvent;
    private bool _isEvent = false;
    private float _timeStartEvent = 0;
    private float _timeCooldownOccurrenctSearch = 0;

    private void Update()
    {
        if(Time.time >= _timeCooldownOccurrenctSearch+_cooldownOccurrenctSearch && !_isEvent)
        {
            var test = Test();
            _timeCooldownOccurrenctSearch = Time.time;

            if(test != null)
            {
                _isEvent = true;
                _timeStartEvent = Time.time;
                _thisEvent = test;
                AddBaff();
                AddDebaff();
            }
        }
        if (_isEvent)
        {
            if(Time.time >= _timeStartEvent + _thisEvent.duration)
            {
                RemoveBaff();
                _isEvent = false;
                _timeCooldownOccurrenctSearch = Time.time;
            }
        }
    }
    private OccurrenctInfo Test()
    {
        foreach(var ocure in _listOccurrenctInfo)
        {
            foreach(var obj  in ocure.royalEnemy)
            {
                if (!obj.activeInHierarchy)
                {
                    ocure.royalEnemy.Remove(obj);
                    if (ocure.royalEnemy.Count == 0) return ocure;
                }
            }
        }
        return null;
    }
    private void AddBaff()
    {
        _player.AddBonusHealth(_thisEvent.buffHealth);
        _player.AddBonusArmor(_thisEvent.buffArmor);
        _player.AddBonusSpeed(_thisEvent.buffSpeed);
    }
    private void AddDebaff()
    {
        _player.AddBonusHealth(-_thisEvent.debuffHealth);
        _player.AddBonusArmor(-_thisEvent.debuffArmor);
        _player.AddBonusHealth(-_thisEvent.debuffSpeed);
    }
    private void RemoveBaff()
    {
        _player.RemoveBonusArmor();
        _player.RemoveBonusSpeed();
    }
}
[System.Serializable]
class OccurrenctInfo
{
    public string name;
    public float duration;
    public List<GameObject> royalEnemy = new List<GameObject>();
    public float buffHealth;
    public float buffArmor;
    public float buffSpeed;
    public float debuffHealth;
    public float debuffArmor;
    public float debuffSpeed;
}
