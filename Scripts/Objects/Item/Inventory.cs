using System;
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour ////원강
{
    [SerializeField]private ItemSlot[] slots;
    [SerializeField]private GameObject inventoryWindow;
    [SerializeField]private Transform slotPanel;

    [SerializeField]private TextMeshProUGUI selectedItemDescription;
    [SerializeField]private GameObject viewButton;
    [SerializeField]private GameObject backButton;
    [SerializeField]private GameObject equipButton;
    [SerializeField]private GameObject unequipButton;

    [SerializeField]private GameObject ItremPreview;

    private Hints hints;
    private ItemDataSO selectedItem;
    private int selectedItemIndex = 0;
    private int curEquipIndex;

    private void Awake()
    {
        hints = new Hints();
    }

    private void Start()
    {
        CharacterManager.Instance.Player.addItem += AddItme;
        CharacterManager.Instance.Player.controller.OnInventoryEvent += Toggle;

        inventoryWindow.SetActive(false);
        slots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
        }
        ClearSelectedItemWindow();
    }

    public void Toggle()
    {
        if (IsOpen())
        {
            inventoryWindow.SetActive(false);
            Back();
        }
        else
        {
            inventoryWindow.SetActive(true);
        }

        ItremPreview.SetActive(false);
    }

    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    private void ClearSelectedItemWindow()
    {
        selectedItemDescription.text = string.Empty;

        viewButton.SetActive(false);
        backButton.SetActive(false);
        equipButton.SetActive(false);
        unequipButton.SetActive(false);
    }

    private void AddItme()
    {
        ItemDataSO itemData = CharacterManager.Instance.Player.itemData;

        ItemSlot emptySlot = GetEmptySlot();

        if (emptySlot != null)
        {
            emptySlot.data = itemData;

            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
            return;
        }

        CharacterManager.Instance.Player.itemData = null;
    }

    private ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].data == null)
            {
                return slots[i];
            }
        }

        return null;
    }

    private void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].data != null)
            {
                slots[i].Set();
            }
            else
            {
                slots[i].Clear();
            }
        }
    }

    public void SelectItem(int index) // 영빈
    {
        if (slots[index].data == null) return;

        selectedItem = slots[index].data;
        selectedItemIndex = index;

        selectedItemDescription.text = selectedItem.description;

        viewButton.SetActive(slots[index].data.type == ItemType.Hint && GameObject.Find("HintMassage(Clone)") == null);
        backButton.SetActive(GameObject.Find("HintMassage(Clone)") != null);
        equipButton.SetActive(!slots[index].equipped && slots[index].data.type == ItemType.Ues && GameObject.Find("HintMassage(Clone)") == null);
        unequipButton.SetActive(slots[index].equipped && slots[index].data.type == ItemType.Ues && GameObject.Find("HintMassage(Clone)") == null);
    }

    public void OnEquipButton()
    {
        if (slots[curEquipIndex].equipped)
        {
            UnEquip(curEquipIndex);
        }

        slots[selectedItemIndex].equipped = true;
        curEquipIndex = selectedItemIndex;
        CharacterManager.Instance.Player.equip.EquipNew(selectedItem);
        UpdateUI();

        SelectItem(selectedItemIndex);
    }

    public void OnUnEquipButton()
    {
        UnEquip(selectedItemIndex);
    }

    private void UnEquip(int index)
    {
        slots[index].equipped = false;
        CharacterManager.Instance.Player.equip.UnEquip();
        UpdateUI();

        if (selectedItemIndex == index)
        {
            SelectItem(selectedItemIndex);
        }
    }
    public void OnViewButton() // 영빈
    {
        View(selectedItemIndex);
    }
    public void View(int index) // 영빈
    {
        hints.SetHints(slots[index].data.num);
        if (GameObject.Find("HintMassage(Clone)") != null)
        {
            Destroy(GameObject.Find("HintMassage(Clone)"));
        }
        Instantiate(GameManager.Instance.hintMemo, GameObject.Find("Canvas").transform);
        SelectItem(selectedItemIndex);
    }
    public void OnBackButton() // 영빈
    {
        Back();
    }
    public void Back() // 영빈
    {
        Destroy(GameObject.Find("HintMassage(Clone)"));
        ClearSelectedItemWindow();
    }
}
