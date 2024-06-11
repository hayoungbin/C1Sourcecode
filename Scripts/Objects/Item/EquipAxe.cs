using UnityEngine;

public class EquipAxe : MonoBehaviour // 원강
{
    private void Start()
    {
        GameManager.Instance.canDestroy = true; //장비 장착시 자물쇠 파괴 가능 해짐
    }
}