using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Stationary_Unit : Unit
{

    public UnitData unitData;

    [SerializeField]
    protected Canvas canvas;
    [SerializeField]
    protected Image delayBar;
    public float attack_Cur_Delay { get; private set; }

    private Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
    }
    public virtual void Set_Stationary_UnitData(UnitData unitData, bool isMyTeam, BattleManager battleManager)
    {
        this.unitData = unitData;
        unitState = new Pencil_State(transform, spr.transform, this);

        Set_UnitData(unitData, isMyTeam, battleManager);
    }


    public void Set_AttackDelay(float delay)
    {
        attack_Cur_Delay = delay;
    }
    public void Update_DelayBar(float delay)
    {
        delayBar.fillAmount = delay;
    }
    public void Delete_Unit()
    {
        battleManager.Pool_Unit(this);

        if(isMyTeam)
        {
            battleManager.unit_MyDatasTemp.Remove(this);
            return;
        }
        battleManager.unit_EnemyDatasTemp.Remove(this);
    }

    public override void Run_Damaged(Unit attacker, int damage, float knockback)
    {
        unitState = new Pencil_Damaged_State(transform, spr.transform, this, attacker, damage, knockback);
    }

    public override void Pull_Unit()
    {
        battleManager.battle_Camera.Set_CameraMove(false);
        unitState = new Pencil_Pull_State(transform, spr.transform, this);
    }

    public override void Throw_Unit()
    {
        unitState = new Pencil_Throw_State(transform, spr.transform, this);
    }
}
