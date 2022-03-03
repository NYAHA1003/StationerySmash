using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PencilCase_Unit : Unit
{

    private PencilCaseDataSO pencilCaseData;

    private void Start()
    {
        battleManager = FindObjectOfType<BattleManager>();
        Set_PencilCaseData(battleManager.pencilCaseDataSO, isMyTeam, battleManager);
        Set_Position();

        if (isMyTeam)
        {
            battleManager.battle_Unit.Add_UnitListMy(this);
            return;
        }
        battleManager.battle_Unit.Add_UnitListEnemy(this);
        
    }

    private void Set_Position()
    {
        if (isMyTeam)
        {
            transform.position = new Vector2(-battleManager.currentStageData.max_Range, 0);
            return;
        }
        transform.position = new Vector2(battleManager.currentStageData.max_Range, 0);
    }

    public void Set_PencilCaseData(PencilCaseDataSO pencilCaseData, bool isMyTeam, BattleManager battleManager)
    {
        this.pencilCaseData = pencilCaseData;
        transform.name = pencilCaseData.data.cord + (isMyTeam ? "¾Æ±º" : "Àû");
        this.isMyTeam = isMyTeam;
        spr.color = isMyTeam ? Color.red : Color.blue;
        spr.sprite = pencilCaseData.data.sprite;
        this.battleManager = battleManager;
        hp = pencilCaseData.data.hp;

        unitState = new PencilCase_Normal_State(transform, spr.transform, this);
        isSettingEnd = true;
    }

    public override void Run_Damaged(Unit attacker, int damage, float knockback, float dir, float extraKnockback, AttackType attackType)
    {
        unitState = new PencilCase_Normal_Damaged_State(transform, spr.transform, this, damage);
    }
}
