using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill;

public class Unit : MonoBehaviour 
{
    protected UnitState unitState;

    [SerializeField]
    protected SpriteRenderer spr;


    protected bool isSettingEnd;

    public int myDamagedId = 0;
    public int damageCount = 0;
    public int myUnitId;
    public int hp { get; protected set; } = 100;
    public int weight { get; protected set; }
    public int maxhp { get; protected set; }
    public bool isInvincibility { get; private set; }

    public bool isMyTeam;

    public BattleManager battleManager { get; protected set; }


    private void Update()
    {
        if (!isSettingEnd) return;

        unitState = unitState.Process();
    }

    public virtual void Set_UnitData(DataBase unitData, bool isMyTeam, BattleManager battleManager, int id)
    {
        transform.name = unitData.cord + unitData.unitName + (isMyTeam ? "¾Æ±º":"Àû");
        this.isMyTeam = isMyTeam;
        spr.color = isMyTeam ? Color.red : Color.blue;
        spr.sprite = unitData.sprite;
        this.battleManager = battleManager;
        maxhp = unitData.hp;
        hp = unitData.hp;
        weight = unitData.weight;
        this.myUnitId = id;

        isSettingEnd = true;
    }

    public virtual void Delete_Unit()
    {
        battleManager.Pool_DeleteUnit(this);

        if (isMyTeam)
        {
            battleManager.unit_MyDatasTemp.Remove(this);
            return;
        }
        battleManager.unit_EnemyDatasTemp.Remove(this);
    }

    public virtual void Run_Damaged(AtkData atkData)
    {

    }
    public virtual void Subtract_HP(int damage)
    {
        hp -= damage;
    }
    public virtual void Pull_Unit()
    {
    }

    public virtual void Throw_Unit()
    {

    }

    public void Set_IsInvincibility(bool isboolean)
    {
        isInvincibility = isboolean;
    }
}
