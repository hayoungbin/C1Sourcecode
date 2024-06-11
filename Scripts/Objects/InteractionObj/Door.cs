using System.Collections;
using UnityEngine;
public interface IInteractable
{
    public string GetInteractPrompt();
    public void ObjectInteract();
}

public class Door : MonoBehaviour, IInteractable // ¿ø°­
{
    private float openAngle = -100f;
    private float closeAngle = 0f;
    private float rotationSpeed = 0.7f;

    public AudioClip openClip;
    public AudioClip closeClip;

    private bool isOpening = false;
    private bool isOpne = false;

    private Transform door;

    private void Awake()
    {
        door = GetComponent<Transform>();
    }

    public string GetInteractPrompt()
    {
        string str = "E";
        return str;
    }

    public void ObjectInteract()
    {
        if (!isOpening)
        {
            if (isOpne)
            {
                StartCoroutine(DoorRotation(closeAngle));
                SoundManager.instance.SFXPlay("CloseDoor", closeClip);
            }
            else
            {
                StartCoroutine(DoorRotation(openAngle));
                SoundManager.instance.SFXPlay("OpenDoor", openClip);
            }
            isOpne = !isOpne;
        }
    }

    public IEnumerator DoorRotation(float targetAngle)
    {
        

        Quaternion doorRotation = door.localRotation;
        Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
        float time = 0f;

        isOpening = true;

        while (time < 1f)
        {
            time += Time.deltaTime * rotationSpeed;
            door.localRotation = Quaternion.Slerp(doorRotation, targetRotation, time);
            yield return null;
        }

        door.localRotation = targetRotation;
        isOpening=false;
    }
}
