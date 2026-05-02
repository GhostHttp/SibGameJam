using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("===Характеристики взрывного монстра===")]
    [SerializeField] private float _health;
    [SerializeField] private float _armor;
    [SerializeField] private float _speed;

    public float Health { get; private set; }
    public float Armor { get; private set; }
    public float Speed { get; private set; }

    private void OnEnable()
    {
        Health = _health;
        Armor = _armor;
        Speed = _speed;
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0) return;
        if (damage - Armor <= 0) return;
        Health -= damage - Armor;

        if (Health <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }
}
