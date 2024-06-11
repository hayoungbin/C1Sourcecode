using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour ////원강
{
    [HideInInspector] public int index;
    [HideInInspector] public bool equipped = false;

    [HideInInspector] public ItemDataSO data;

    [HideInInspector] public Inventory inventory;

    [HideInInspector] public Image icon;

    [SerializeField] private GameObject previewItemUI;
    [SerializeField] private GameObject previewAxe;
    [SerializeField] private GameObject previewFlash;

    public void Set()
    {
        icon.gameObject.SetActive(true);
        icon.sprite = data.icon;
    }

    public void Clear()
    {
        data = null;
        icon.gameObject.SetActive(false);
    }

    public void OnClickButton()
    {
        inventory.SelectItem(index);
        OnPreviewItemObject();
    }

    public void OnPreviewItemObject()
    {
        previewAxe.SetActive(false); // 기존에 활성화 돼있는 오브젝트 끔
        previewFlash.SetActive(false);

        previewItemUI.SetActive(true);

        switch (data.name)
        {
            case ItemName.Axe:
                previewAxe.SetActive(true);
                break;

            case ItemName.Flash:
                previewFlash.SetActive(true);
                break;
            default:
                previewAxe.SetActive(false);
                previewFlash.SetActive(false);
                previewItemUI.SetActive(false);
                break;
        }
    }
}