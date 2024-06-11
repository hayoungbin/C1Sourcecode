using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Object", menuName = "SO/NewObjectSO")]
public class ObjectsSO : ScriptableObject
{
    [Header("Info")]
    public  string _name;
    public string description;
}
