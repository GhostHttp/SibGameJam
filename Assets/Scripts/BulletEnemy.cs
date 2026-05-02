using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _damage;
    [SerializeField] private float _lifetime;

    private float _timeLifetime;

    private void OnEnable()
    {
        _timeLifetime = Time.time;
    }
    private void Update()
    {
        if (Time.time >= _timeLifetime + _lifetime) this.gameObject.SetActive(false);
        transform.Translate(Vector2.right * _speed * Time.deltaTime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null)
        {
            this.gameObject.SetActive(false);
            if (collision.gameObject.CompareTag("Player")) collision.gameObject.GetComponent<PlayerStatistic>().TakeDamage(_damage);
            else if(collision.gameObject.GetComponent("UnitsPlayer")) collision.gameObject.GetComponent<Units>().TakeDamage(_damage);
        }
    }
}
