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
public class UnitData
{
    public int cord;
    public int cost;
    public string name;
    public int hp;
    public float moveSpeed;
    public float attackSpeed;
    public float range;
    public Sprite sprite;
}