using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerForm3 : MonoBehaviour
{
    [Header("===Переменные формы ближнего боя===")]
    [SerializeField] private float _attackCooldown;
    [SerializeField] private float _attackDamage;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackDistance;

    private bool _isEnabled = false;
    private float _timeCooldown = 0;

    public void Enable(bool enable)
    {
        _isEnabled = enable;
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
            //Обработка нанесения урона
            Debug.Log($"Укусил {hit.collider.gameObject.name}.");
        }
        _timeCooldown = Time.time;
    }
}
