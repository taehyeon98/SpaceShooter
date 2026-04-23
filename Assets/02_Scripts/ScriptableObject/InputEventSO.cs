using System;
using UnityEngine;

[CreateAssetMenu(fileName = "InputEventSO", menuName = "Scriptable Objects/InputEventSO")]
public class InputEventSO : ScriptableObject
{
    //Listener정의
    private event Action<Vector2> OnMove;
    private event Action<Vector2> OnLook;

    /*
     * Lamda Expression(람다식) : 익명함수,간단한 문법으로 함수를 표현하는 방법.
     * 람다식 문법 :
     * -파라미터가 있는 경우 : (파라미터) => {함수 로직}
     * -파라미터가 없는 경우 : () => {함수 로직}
     * Action<int> onDamage += 함수;
     * 함수;
     *
     * Action<int> onDamage = (damage) => {Debug.Log("피격데미지"+damage);}
     * onDamage?.Invoke(10);
     *
     */

    //Move 구독 해지 처리
    public void SubscribeMove(Action<Vector2> listener) => OnMove += listener;
    public void UnsubscribeMove(Action<Vector2> listener) => OnMove -= listener;

    //Raise처리
    public void RaiseMove(Vector2 value) => OnMove?.Invoke(value);

    //Look 구독 해지 처리
    public void SubscribeLook(Action<Vector2> listener) => OnLook += listener;

    public void UnsubscribeLook(Action<Vector2> listener) => OnLook -= listener;

    //Raise 처리
    public void RaiseLook(Vector2 value) => OnLook?.Invoke(value);
}