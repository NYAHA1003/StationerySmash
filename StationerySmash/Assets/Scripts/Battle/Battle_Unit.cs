using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle_Unit : BattleCommand
{
    private GameObject unit_Prefeb;
    private Transform unit_PoolManager;
    private Transform unit_Parent;
    private GameObject unit_AfterImage;
    private SpriteRenderer unit_AfterImage_Spr;
    public bool isMyTeam;

    public Battle_Unit(BattleManager battleManager, GameObject unit_Prefeb, Transform unit_PoolManager, Transform unit_Parent, GameObject unit_AfterImage) : base(battleManager)
    {
        this.unit_Prefeb = unit_Prefeb;
        this.unit_PoolManager = unit_PoolManager;
        this.unit_Parent = unit_Parent;
        this.unit_AfterImage = unit_AfterImage;
        unit_AfterImage_Spr = unit_AfterImage.GetComponent<SpriteRenderer>();
    }

    public void Summon_Unit(UnitData unitData, Vector3 Pos)
    {
        Unit unit = Pool_Unit(Pos);
        unit.Set_UnitData(unitData);
    }

    public void Set_UnitAfterImage(UnitData unitData, Vector3 Pos, bool isDelete = false)
    {
        unit_AfterImage.transform.position = new Vector3(Pos.x,0);
        unit_AfterImage_Spr.sprite = unitData.sprite;
        
        if (isDelete)
        {
            unit_AfterImage.SetActive(false);
            return;
        }
        unit_AfterImage.SetActive(true);
    }

    private Unit Pool_Unit(Vector3 Pos)
    {
        GameObject unit_obj = null;
        if (unit_PoolManager.childCount > 0)
        {
            unit_obj = unit_PoolManager.GetChild(0).gameObject;
            unit_obj.transform.position = Pos;
            unit_obj.SetActive(true);
        }
        unit_obj ??= battleManager.Create_Object(unit_Prefeb, Pos, Quaternion.identity);
        unit_obj.transform.SetParent(unit_Parent);
        return unit_obj.GetComponent<Unit>();
    }
}
