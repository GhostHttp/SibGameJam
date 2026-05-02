using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerForm3 : MonoBehaviour
{
    [Header("===Переменные формы ближнего боя===")]
    [SerializeField] private Sprite _sprite;
    [SerializeField] private float _attackCooldown;
    [SerializeField] private float _attackDamage;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackDistance;

    private bool _isEnabled = false;
    private float _timeCooldown = 0;

    public void Enable(bool enable)
    {
        _isEnabled = enable;
        if(_isEnabled) this.gameObject.GetComponent<SpriteRenderer>().sprite = _sprite;
    }
    public void Attack(InputAction.CallbackContext context)
    {
        if (context.started && _isEnabled)
        {
            if (Time.time > _timeCooldown + _attackCooldown || Time.time <= _attackCooldown) Attack();
        }
    }
    private void Attack()
    {
        RaycastHit2D hit = Physics2D.Raycast(_attackPoint.position, this.transform.right, _attackDistance);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("enemy"))hit.collider.gameObject.GetComponent<Enemy>().TakeDamage(_attackDamage);
        }
        _timeCooldown = Time.time;
    }
}
