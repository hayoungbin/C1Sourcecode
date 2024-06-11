using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// input system에서 입력된 값 받아오기
/// <author>김수현</author>
/// </summary>
public class PlayerInputController : Controller // WK 추가 E키
{
    private Vector2 curMovementInput;
    private Vector2 mouseDelta;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //gamemanager로 이동
    }

    public void OnMove(InputValue value)
    {
        curMovementInput = value.Get<Vector2>();
        CallMoveEvent(curMovementInput);
    }

    public void OnLook(InputValue value)
    {
        mouseDelta = value.Get<Vector2>();
        CallLookEvent(mouseDelta);
    }

    public void OnRun(InputValue value)
    {
        float runValue = value.Get<float>();
        CallRunEvent(runValue);
    }

    public void OnInventory(InputValue value)
    {
        CallInventoryEvent();
    }

    public void OnInteract(InputValue vlaue)
    {
        CallInteracEvent();
    }

    public void OnMenu(InputValue vlaue) // 원강, ESC 누를 시 메뉴 화면 나오게.
    {
        CallMenuEvent();
    }
}
