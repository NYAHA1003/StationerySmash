using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(fileName = "UnitDataSO", menuName = "Scriptable Object/UnitDataSO")]
public class UnitDataSO : ScriptableObject
{
    [Header("�ӽ� ���� ������")]
    public List<UnitData> unitDatas;
}

[System.Serializable]
public class UnitData : DataBase
{
    public int cost;
    public int weight;
    public int knockback;
    public float moveSpeed;
    public float attackSpeed;
    public float range;
}