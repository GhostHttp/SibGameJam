using UnityEngine;

public class PlayerStatistic : MonoBehaviour
{
    [Header("===Характеристики игрока===")]
    [SerializeField] private float _health;
    [SerializeField] private float _armor;
    [SerializeField] private float _speed;

    [SerializeField] private float _bonusHealth = 0;
    [SerializeField] private float _bonusArmor = 0;
    [SerializeField] private float _bonusSpeed = 0;

    private float _factorHealth;
    private float _factorArmor;
    private float _factorSpeed;

    public float Health { get; private set; }
    public float Armor { get; private set; }
    public float Speed { get; private set; }

    private void Start()
    {
        Health = _health;
        Armor = _armor;
        Speed = _speed;
    }

    public void AddBonusHealth(float bonusHealth)
    {
        if(_bonusHealth + bonusHealth < 0) return;
        _bonusHealth += bonusHealth;

        if(_factorHealth == 0)
        {
            Health += _bonusHealth;
        }
        else
        {
            float setHealth = Health / _factorHealth;
            setHealth += bonusHealth;
            Health = setHealth * _factorHealth;
        }
    }
    public void AddBonusArmor(float bonusArmor)
    {
        if(_bonusArmor + bonusArmor < 0) return;
        _bonusArmor += bonusArmor;

        if(_factorArmor == 0)
        {
            Armor += _bonusArmor;
        }
        else
        {
            float setArmor = Armor / _factorArmor;
            setArmor += bonusArmor;
            Armor = setArmor * _factorArmor;
        }
    }
    public void AddBonusSpeed(float bonusSpeed)
    {
        if (_bonusSpeed + bonusSpeed < 0) return;
        _bonusSpeed += bonusSpeed;

        if(_factorSpeed == 0)
        {
           Speed += _bonusSpeed;
        }
        else
        {
            float setSpeed = Speed / _factorSpeed;
            setSpeed += bonusSpeed;
            Speed = setSpeed * _factorSpeed;
        }
    }
    
    public void SetFactorHealth(float factorHealth)
    {
        if(factorHealth <= 0) return;
        if (factorHealth != 0) RemoveFactorHealth();
        _factorHealth = factorHealth;
    }
    public void SetFactorArmor(float factorArmor)
    {
        if (factorArmor <= 0) return;
        if (factorArmor != 0) RemoveFactorArmor();
        _factorArmor = factorArmor;
    }
    public void SetFactorSpeed(float factorSpeed)
    {
        if (factorSpeed <= 0) return;
        if (factorSpeed != 0) RemoveFactorSpeed();
        _factorSpeed = factorSpeed;
    }
    
    public void RemoveFactorHealth()
    {
        Health /= _factorHealth;
    }
    public void RemoveFactorArmor()
    {
        Armor /= _factorArmor;
    }
    public void RemoveFactorSpeed()
    {
        Speed /= _factorSpeed;
    }

    public void RemoveBonusArmor()
    {
        if (_factorArmor == 0)
        {
            Armor -= _bonusArmor;
        }
        else
        {
            float setArmor = Armor / _factorArmor;
            setArmor -= _bonusArmor;
            Armor = setArmor * _factorArmor;
        }
    }
    public void RemoveBonusSpeed()
    {
        if (_factorArmor == 0)
        {
            Armor -= _bonusArmor;
        }
        else
        {
            float setArmor = Armor / _factorArmor;
            setArmor -= _bonusArmor;
            Armor = setArmor * _factorArmor;
        }
    }

    public void TakeDamage(float damage)
    {
        if(damage <= 0) return;
        if(damage - Armor <= 0) return;
        Health -= damage - Armor;

        if(Health <= 0)
        {
            Debug.Log($"Вам нанесли {damage} урона и вы погибли!");
            this.transform.gameObject.SetActive(false);
        }
    }
}
