using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill;

public enum AttackType
{
    Normal,
    Stun,
}

public class Unit : MonoBehaviour 
{
    protected UnitState unitState;

    [SerializeField]
    protected SpriteRenderer spr;


    protected bool isSettingEnd;

    public int myDamageId = 0;
    public int damagedId = 0;
    public int damageCount = 0;
    private int id;
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
        id = id;

        isSettingEnd = true;
    }


    public virtual void Run_Damaged(Unit attacker, int damageId ,int damage, KBData kBData, AttackType attackType)
    {
    }
    public void Subtract_HP(int damage)
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

    public void Set_DamageId()
    {
        damageCount++;
        myDamageId = id * 10000 + damageCount;
    }

    public int Get_DamageId()
    {
        return myDamageId;
    }
}
