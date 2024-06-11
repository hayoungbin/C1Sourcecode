using TMPro;
using UnityEngine;

public class Interaction : MonoBehaviour // wk 수정함.원강
{
    private IInteractable curinteractable;
    private Controller controller;

    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDistance;

    public LayerMask layerMask;
    public GameObject curInteractGameObject;
    public TextMeshProUGUI promptText;
    private Camera camera;


    private void Start()
    {
        camera = Camera.main;
        controller = CharacterManager.Instance.Player.controller;

        CharacterManager.Instance.Player.controller.OnInteracEvent += Interact;
    }

    private void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    curInteractGameObject = hit.collider.gameObject;
                    curinteractable = hit.collider.GetComponent<IInteractable>();
                    SetPromptText();
                }
            }
            else
            {
                curInteractGameObject = null;
                curinteractable = null;
                promptText.gameObject.SetActive(false);
            }
        }
    }

    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = curinteractable.GetInteractPrompt();
    }

    private void Interact()
    {
        if (curinteractable != null)
        {
            curinteractable.ObjectInteract();
            curInteractGameObject = null;
            curinteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
}
