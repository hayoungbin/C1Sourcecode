using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable // WK원강
{
    public ItemDataSO data;

    public AudioClip getItem;
    public string GetInteractPrompt()
    {
        string str = "E 키를 누르시오.";
        return str;
    }

    public void ObjectInteract()
    {
        SoundManager.instance.SFXPlay("GetItem", getItem);
        CharacterManager.Instance.Player.itemData = data;
        CharacterManager.Instance.Player.addItem?.Invoke();
        Destroy(gameObject);
    }
}
