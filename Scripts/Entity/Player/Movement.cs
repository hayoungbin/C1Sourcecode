using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 움직임 구현부
/// <author>김수현</author>
/// </summary>
public class Movement : MonoBehaviour
{
    [Header("# Movement")]
    public float speed;
    public float moveSpeed;
    public float runSpeed;
    public float useStamina;

    [Header("# Look")]
    public Transform cameraContainer;
    public Camera cam;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot; // 카메라 회전
    [HideInInspector]public float lookSensitivity; // 카메라 감도 // 원강, 지금 이 변수는 쓰이는 곳이 없음.
    private Vector2 mouseDelta; // input 값으로 받아오는 마우스 delta 값

    [HideInInspector]
    public bool isRun = false;
    [HideInInspector]
    public bool canLook = true;

    private Rigidbody movementRigidbody;
    private FootStepsSound footStepsSound;

    public GameObject flashlight = null;
    public GameObject menuObject;
    public GameObject optionObject;

    Vector2 movementDir = Vector2.zero;

    private void Awake()
    {
        speed = moveSpeed;
        cam = UnityEngine.Camera.main;
        movementRigidbody = GetComponent<Rigidbody>();
        footStepsSound = GetComponent<FootStepsSound>();
    }

    private void Start()
    {
        CharacterManager.Instance.Player.controller.OnMoveEvent += Move;
        CharacterManager.Instance.Player.controller.OnInventoryEvent += UseInventory;
        CharacterManager.Instance.Player.controller.OnRunEvent += Run;
        CharacterManager.Instance.Player.controller.OnLookEvent += Camera;

        CharacterManager.Instance.Player.controller.OnMenuEvent += OpneMenu; // 원강 메뉴 오픈
        CharacterManager.Instance.Player.controller.OnMenuEvent += ToggleCursor; // 원강 메뉴 오픈시 마우스 온오프

    }

    private void FixedUpdate()
    {
        if (isRun)
        {
            if (CharacterManager.Instance.Player.condition.UseStamina(useStamina))
            {
                footStepsSound.SetRunning(true);
                speed = runSpeed;                
            }
            else
            {
                footStepsSound.SetRunning(false);
                speed = moveSpeed;                
            }
        }
        else
        {
            footStepsSound.SetRunning(false);
            speed = moveSpeed;            
        }

        ApplyMovement(movementDir);
    }

    private void LateUpdate()
    {
        if(canLook)
        {
            CameraLook();
        }
    }

    private void Move(Vector2 direction)
    {
        movementDir = direction;
    }

    private void ApplyMovement(Vector2 direction)
    {
        Vector3 dir = transform.forward * direction.y + transform.right * direction.x;
        dir *= speed;
        dir.y = movementRigidbody.velocity.y;

        movementRigidbody.velocity = dir;
    }

    private void Run(float value)
    {
        if(isRun == false)
        {
            isRun = true;
        }
        else
        {
            isRun = false;
        }
    }

    private void UseInventory()
    {
        ToggleCursor();
    }

    public void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }

    private void Camera(Vector2 vector)
    {
        mouseDelta = vector;
    }

    private void CameraLook()
    {
        camCurXRot += mouseDelta.y * GameManager.Instance.GamelookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cam.transform.localEulerAngles = new Vector3(-camCurXRot, 0, 0);
        if(flashlight != null)
        {
            flashlight.transform.localEulerAngles = new Vector3(camCurXRot - 90, 180, 0);
        }

        transform.eulerAngles += new Vector3(0, mouseDelta.x * GameManager.Instance.GamelookSensitivity, 0);
    }

    private void OpneMenu()
    {
        optionObject.SetActive(false); // 메뉴 킬 때 활성화 돼 있는 옵션창 꺼두기

        menuObject.SetActive(!menuObject.activeSelf); // ESC 누를 때 온오프 
    }
}
