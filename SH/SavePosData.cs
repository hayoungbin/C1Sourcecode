using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePosData : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("위치 저장 중...");
        DataManager.Instance.SavePosData(other.transform.position);
    }
}
