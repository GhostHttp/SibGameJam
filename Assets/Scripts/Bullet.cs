using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed;
    private void Update()
    {
        transform.Translate(Vector2.right * _speed * Time.deltaTime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null)
        {
            this.gameObject.SetActive(false);
            //Обработка нанесения урона
        }
    }
}
