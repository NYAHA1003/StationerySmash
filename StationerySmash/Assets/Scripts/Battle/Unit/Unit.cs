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

    public BattleManager battleManager;

    private void Update()
    {
        if (!isSettingEnd) return;

        unitState = unitState.Process();
    }

    public void Set_UnitData(UnitData unitData, bool isMyTeam, BattleManager battleManager)
    {
        this.unitData = unitData;
        this.isMyTeam = isMyTeam;
        spr.color = isMyTeam ? Color.red : Color.blue;
        spr.sprite = unitData.sprite;
        this.battleManager = battleManager;

        unitState = new Pencil_State(transform, spr.transform, this);

        isSettingEnd = true;
    }
}
