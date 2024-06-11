using UnityEngine;

public enum ItemName
{
    Axe,
    Flash,
    Hint
}

public enum ItemType
{
    Ues,
    Hint
}

[CreateAssetMenu(fileName = "ItemSO", menuName = "SO/NewItemDataSO")]
public class ItemDataSO : ScriptableObject ///원강
{
    [Header("Info")]
    public string description;
    public Sprite icon;
    public GameObject equipPrefab;
    public GameObject previewPrefab;
    public ItemType type;
    public ItemName name;

    [Header("HintNum")]
    public int num;
}
