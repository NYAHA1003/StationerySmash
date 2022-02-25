using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private UnitState unitState;

    [SerializeField]
    private SpriteRenderer spr;

    public UnitData unitData;

    private void Start()
    {

    }

    private void Update()
    {
        //unitState = unitState.Process();
    }

    public void Set_UnitData(UnitData unitData)
    {
        this.unitData = unitData;
        spr.sprite = unitData.sprite;
    }
}
