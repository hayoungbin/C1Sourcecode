using System.Collections;
using UnityEngine;

public class BigDoor : MonoBehaviour , IInteractable // ¿ø°­
{
    private float openAngle = 110f;
    private float closeAngle = 0f;
    private float rotationSpeed = 0.7f;

    [SerializeField]private Transform leftDoor;
    [SerializeField]private Transform rightDoor;

    public AudioClip openClip;
    public AudioClip closeClip;
    public string GetInteractPrompt()
    {
        string str = "E";
        return str;
    }

    public void ObjectInteract()
    {
        if (!GameManager.Instance.isOpening)
        {
            if (GameManager.Instance.isOpne)
            {
                StartCoroutine(DoorRotation(closeAngle));
                SoundManager.instance.SFXPlay("CloseDoor", closeClip);
            }
            else
            {
                StartCoroutine(DoorRotation(openAngle));
                SoundManager.instance.SFXPlay("OpenDoor", openClip);
        }
            GameManager.Instance.isOpne = !GameManager.Instance.isOpne;
        }
    }

    public IEnumerator DoorRotation(float TargetAngle)
    {
        Quaternion leftDoorRotation = leftDoor.localRotation;
        Quaternion rightDoorRotation = rightDoor.localRotation;

        Quaternion leftTargetRotation = Quaternion.Euler(0f, -TargetAngle, 0f);
        Quaternion rightTargetRotation = Quaternion.Euler(-180f, TargetAngle, 0f);

        float time = 0f;

        GameManager.Instance.isOpening = true;

        while (time < 1f)
        {
            time += Time.deltaTime * rotationSpeed;
            leftDoor.localRotation = Quaternion.Slerp(leftDoorRotation, leftTargetRotation, time);
            rightDoor.localRotation = Quaternion.Slerp(rightDoorRotation, rightTargetRotation, time);
            yield return null;
        }

        leftDoor.localRotation = leftTargetRotation;
        rightDoor.localRotation = rightTargetRotation;

        GameManager.Instance.isOpening = false;
    }
}
