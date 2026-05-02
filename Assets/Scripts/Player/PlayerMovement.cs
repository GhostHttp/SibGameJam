using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speedRotate = 5;
    private PlayerStatistic _playerS;
    private Rigidbody2D _rb;
    private float _moveDirection;
    private float _rotateDirection;
    private bool _isRotation = true;

    private void Start()
    {
        _playerS = GetComponent<PlayerStatistic>();
        _rb = GetComponent<Rigidbody2D>();
    }
    public void Move(InputAction.CallbackContext context)
    {
        float type = context.ReadValue<float>();
        if (context.canceled || type == 0)
        {
            _moveDirection = 0;
            _rb.linearVelocity = new Vector2(0, 0);
        }
        else
        {
            _moveDirection = type;
        }
    } //Шаблонно
    public void Rotate(InputAction.CallbackContext context)
    {
        if (!_isRotation) _rotateDirection = 0;
        float type = context.ReadValue<float>();
        if (context.canceled || type == 0)
        {
            _rotateDirection = 0;
            _isRotation = true;
        }
        else
        {
            _rotateDirection = type;
            _isRotation = false;
        }
    } // Шаблонно

    private void FixedUpdate()
    {
        if (_moveDirection != 0)
        {
            Vector2 forward = transform.right * _playerS.Speed * _moveDirection;
            _rb.linearVelocity = forward;
        }
    } //Физика передвижения
    private void Update()
    {
        if (_rotateDirection != 0)
        {
            float rotationZ = _rotateDirection * _speedRotate * Time.deltaTime;
            transform.Rotate(0, 0, rotationZ);
        }
    } //Повороты
}
