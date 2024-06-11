using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Equipment : MonoBehaviour ///원강
{
    public Equip curEquip;
    public Transform equipParent;

    private PlayerInputController controller;

    void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
    }

    public void OnFlashlight(InputValue value)
    {
        Debug.Log("손전등");
        curEquip.UseFlashlight();
    }

    public void EquipNew(ItemDataSO data)
    {
        UnEquip();
        CharacterManager.Instance.Player.EquipItemLsit.Clear();

        curEquip = Instantiate(data.equipPrefab, equipParent).GetComponent<Equip>();
        CharacterManager.Instance.Player.EquipItemLsit.Add(data);

    }

    public void UnEquip()
    {
        if (curEquip != null)
        {
            GameManager.Instance.canDestroy = false; // 도끼를 들고 있어야 자물쇠를 파괴 할 수 있게 하는 변수;
            Destroy(curEquip.gameObject);
            curEquip = null;
            CharacterManager.Instance.Player.EquipItemLsit.Clear();
        }
    }
}
