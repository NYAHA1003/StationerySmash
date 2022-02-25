using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private UnitState unitState;

    [SerializeField]
    private SpriteRenderer spr;

    public UnitData unitData;

    private bool isSettingEnd;
    public bool isMyTeam;

    private void Update()
    {
        if (!isSettingEnd) return;

        unitState = unitState.Process();
    }

    public void Set_UnitData(UnitData unitData)
    {
        this.unitData = unitData;
        spr.sprite = unitData.sprite;

        unitState = new Pencil_State(transform, spr.transform, this);

        isSettingEnd = true;
    }
}
