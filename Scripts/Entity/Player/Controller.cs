using System;
using UnityEngine;

/// <summary>
/// control 이벤트 담당 함수
/// <author>김수현</author>
/// </summary>
public class Controller : MonoBehaviour // WK interact 이벤트 함수 생성
{
    public event Action<Vector2> OnMoveEvent;
    public event Action<Vector2> OnLookEvent;
    public event Action<float> OnRunEvent;
    public event Action OnInventoryEvent;
    public event Action OnInteracEvent;
    public event Action OnMenuEvent; // 원강 메뉴화면 나오게


    public void CallMoveEvent(Vector2 direction)
    {
        OnMoveEvent?.Invoke(direction);
    }

    public void CallLookEvent(Vector2 direction)
    {
        OnLookEvent?.Invoke(direction);
    }

    public void CallRunEvent(float value)
    {
        OnRunEvent?.Invoke(value);
    }

    public void CallInventoryEvent()
    {
        OnInventoryEvent?.Invoke();
    }

    public void CallInteracEvent()
    {
        OnInteracEvent?.Invoke();
    }

    public void CallMenuEvent() // 원강 메뉴화면 나오게
    {
        OnMenuEvent?.Invoke();
    }
}
