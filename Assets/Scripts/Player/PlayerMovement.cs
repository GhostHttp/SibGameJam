using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerStatistic _playerS;
    private Rigidbody2D _rb;
    private float _moveYDirection;
    private float _moveXDirection;

    private void Start()
    {
        _playerS = GetComponent<PlayerStatistic>();
        _rb = GetComponent<Rigidbody2D>();
    }
    public void MoveY(InputAction.CallbackContext context)
    {
        float type = context.ReadValue<float>();
        if (context.canceled || type == 0)
        {
            _moveYDirection = 0;
            _rb.linearVelocity = new Vector2(0, 0);
        }
        else
        {
            _moveYDirection = type;
        }
    } //Шаблонно
    public void MoveX(InputAction.CallbackContext context)
    {
        float type = context.ReadValue<float>();
        if (context.canceled || type == 0)
        {
            _moveXDirection = 0;
            _rb.linearVelocity = new Vector2(0, 0);
        }
        else
        {
            _moveXDirection = type;
        }
    } // Шаблонно

    private void FixedUpdate()
    {
        if (_moveYDirection != 0)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocityX, _playerS.Speed*_moveYDirection);
        }
        if (_moveXDirection != 0)
        {
            _rb.linearVelocity = new Vector2(_playerS.Speed * _moveXDirection, _rb.linearVelocityY);
        }
    } //Физика передвижения
    private void Update()
    {
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 direction = (targetPosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
