//전처리기(PreProcessor)
//#define INPUTACTION_REF
//#define WRAPPER_CLASS

using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
#if INPUTACTION_REF
    [Header("InputActionReferences")] 
    [SerializeField] private InputActionReference _moveAction;

    private void OnEnable()
    {
        _moveAction.action.started += OnMove;
        _moveAction.action.performed += OnMove;
        _moveAction.action.canceled += OnMove;
        
        //액션을 활성화.
        _moveAction.action.Enable();
    }

    private void OnDisable()
    {
        _moveAction.action.started -= OnMove;
        _moveAction.action.performed -= OnMove;
        _moveAction.action.canceled -= OnMove;
        
        _moveAction.action.Disable();
    }
    
    private void OnMove(InputAction.CallbackContext ctx)
    {
        switch (ctx.action.phase)
        {
            case InputActionPhase.Started:
                Debug.Log($"OnMove Started : {ctx.started}");
                break;
            case InputActionPhase.Performed:
                Debug.Log($"OnMove Performed : {ctx.ReadValue<Vector2>()}");
                break;
            case InputActionPhase.Canceled:
                Debug.Log($"OnMove Canceled : {ctx.canceled}");
                break;
        }
    }
#endif

#if WRAPPER_CLASS
    [SerializeField] private InputEventSO _inputEventSO;
    private InputSystem_Actions _inputSystemActions;

    private void Awake()
    {
        _inputSystemActions = new InputSystem_Actions();
        _inputSystemActions.Enable();
    }

    private void OnEnable()
    {
        //binding
        _inputSystemActions.Player.Move.performed += OnMove;
        _inputSystemActions.Player.Move.canceled += OnMove;
        
        //회전액션
        _inputSystemActions.Player.Look.performed += OnLook;
        _inputSystemActions.Player.Look.canceled += OnLook;
    }

    private void OnDisable()
    {
        _inputSystemActions.Player.Move.performed -= OnMove;
        _inputSystemActions.Player.Move.canceled -= OnMove;
        _inputSystemActions.Player.Look.performed -= OnLook;
        _inputSystemActions.Player.Look.canceled -= OnLook;
        
        _inputSystemActions.Disable();
    }

    private void OnLook(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Performed)
        {
            _inputEventSO.RaiseLook(ctx.ReadValue<Vector2>());   
        }
        else if (ctx.phase == InputActionPhase.Canceled)
        {
            _inputEventSO.RaiseLook(Vector2.zero);
        }
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        _inputEventSO.RaiseMove(ctx.ReadValue<Vector2>());
    }
#endif
}
