using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private static DataManager _instance;

    public static DataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("DataManager").AddComponent<DataManager>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if(_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    // 데이터 .json 저장
    public void SavePlayerDataToJson()
    {
        Debug.Log("저장완료");
        string jsonData = JsonUtility.ToJson(CharacterManager.Instance.Player.playerData);
        string path = Path.Combine(Application.dataPath, "playerData.json");
        File.WriteAllText(path, jsonData);
    }

    // json 데이터 불러오기
    public void LoadPlayerDataFromJson()
    {
        string path = Path.Combine(Application.dataPath, "playerData.json");
        string jsonData = File.ReadAllText(path);
        CharacterManager.Instance.Player.playerData = JsonUtility.FromJson<PlayerData>(jsonData); // 역직렬화
    }

    public void SaveData(QuestState state)
    {
        CharacterManager.Instance.Player.playerData.quest.QuestData(state);
        CharacterManager.Instance.Player.playerData.equipItems = CharacterManager.Instance.Player.EquipItemLsit; //장착 아이템 저장
        CharacterManager.Instance.Player.playerData.itemList = CharacterManager.Instance.Player.ItemLsit; //인벤 아이템 저장

        SavePlayerDataToJson();
    }

    public void SavePosData(Vector3 pos)
    {
        CharacterManager.Instance.Player.playerData.position = pos;
        CharacterManager.Instance.Player.playerData.equipItems = CharacterManager.Instance.Player.EquipItemLsit; //장착 아이템 저장
        CharacterManager.Instance.Player.playerData.itemList = CharacterManager.Instance.Player.ItemLsit; //인벤 아이템 저장

        Debug.Log("위치 저장");
        SavePlayerDataToJson();
    }
}
