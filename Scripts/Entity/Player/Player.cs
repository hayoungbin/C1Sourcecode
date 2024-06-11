using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerInputController controller;
    public PlayerCondition condition;
    public Movement movement;

    [HideInInspector] public ItemDataSO itemData;  // W K. 아이템 가방 추가에 필요한 변수 2개
    [HideInInspector] public Action addItem; // wk
    [HideInInspector] public Equipment equip;// wk

    public List<ItemDataSO> EquipItemLsit;
    public List<ItemDataSO> ItemLsit;

    public PlayerData playerData;
    public QuestLine questLine = new QuestLine();

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        controller = GetComponent<PlayerInputController>();
        condition = GetComponent<PlayerCondition>();
        movement = GetComponent<Movement>();
        equip = GetComponent<Equipment>(); // wk
    }
}


[System.Serializable]
public class PlayerData
{
    public Vector3 position;
    public QuestLine quest;
    public List<ItemDataSO> equipItems;
    public List<ItemDataSO> itemList;
}
