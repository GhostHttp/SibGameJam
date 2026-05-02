using UnityEngine;
using UnityEngine.InputSystem;

public class FormManager : MonoBehaviour
{
    [Header("===Менеджер форм===")]
    [SerializeField] private PlayerForm1 _playerForm1;
    [SerializeField] private PlayerForm2 _playerForm2;
    [SerializeField] private PlayerForm3 _playerForm3;
    [SerializeField] private PlayerForm4 _playerForm4;
    [SerializeField] private float _cooldownTransformation;

    private int _numberForm = 0;
    private float _cooldownTime = 0;
    private void OnEnable()
    {
        AllDisable();
        _playerForm1.Enable(true);
        _numberForm = 0;
        _cooldownTime = Time.time;
    }

    public void Transformation1(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (_numberForm == 0) return;
            if (Time.time > _cooldownTime + _cooldownTransformation || Time.time < _cooldownTransformation)
            {
                AllDisable();
                _playerForm1.Enable(true);
                _numberForm = 0;
                _cooldownTime = Time.time;
            }
        }
    }
    public void Transformation2(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (_numberForm == 1) return;
            if (Time.time > _cooldownTime + _cooldownTransformation || Time.time < _cooldownTransformation)
            {
                AllDisable();
                _playerForm2.Enable(true);
                _numberForm = 1;
                _cooldownTime = Time.time;
            }
        }
    }
    public void Transformation3(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (_numberForm == 2) return;
            if (Time.time > _cooldownTime + _cooldownTransformation || Time.time < _cooldownTransformation)
            {
                AllDisable();
                _playerForm3.Enable(true);
                _numberForm = 2;
                _cooldownTime = Time.time;
            }
        }
    }
    public void Transformation4(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (_numberForm == 3) return;
            if (Time.time > _cooldownTime + _cooldownTransformation || Time.time < _cooldownTransformation)
            {
                AllDisable();
                _playerForm4.Enable(true);
                _numberForm = 3;
                _cooldownTime = Time.time;
            }
        }
    }
    private void AllDisable()
    {
        _playerForm1.Enable(false);
        _playerForm2.Enable(false);
        _playerForm3.Enable(false);
        _playerForm4.Enable(false);
    }
}
