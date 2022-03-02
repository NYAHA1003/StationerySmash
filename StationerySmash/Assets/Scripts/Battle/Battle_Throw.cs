using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle_Throw : BattleCommand
{
    private Unit throw_Unit;
    private LineRenderer parabola;
    private Vector2 touch_Origin_Pos;
    private Vector2 direction;
    private float force = 1;
    public Battle_Throw(BattleManager battleManager, LineRenderer parabola) : base(battleManager)
    {
        this.parabola = parabola;
    }

    public void Pull_Unit(Vector2 pos)
    {
        float targetRange = float.MaxValue;
        touch_Origin_Pos = pos;
        for (int i = 0; i < battleManager.unit_MyDatasTemp.Count; i++)
        {
            if (battleManager.unit_MyDatasTemp[i].transform.position.sqrMagnitude < targetRange)
            {
                throw_Unit = battleManager.unit_MyDatasTemp[i];
                targetRange = throw_Unit.transform.position.sqrMagnitude;
            }
        }

        if (throw_Unit != null)
        {
            if (Vector2.Distance(pos, throw_Unit.transform.position) < 0.1f)
            {
                throw_Unit.Pull_Unit();
                return;
            }
            throw_Unit = null;
        }

    }

    public void Draw_Parabola(Vector2 pos)
    {
        if(throw_Unit != null)
        {
            direction = pos - (Vector2)throw_Unit.transform.position;
            force = Vector2.Distance(throw_Unit.transform.position, pos);
            for (int i = 0; i < parabola.positionCount; i++)
            {
                parabola.SetPosition(i, Set_ParabolaPos(i * 0.1f));
            }
        }
    }

    private Vector2 Set_ParabolaPos(float t)
    {
        Vector2 pos = (Vector2)throw_Unit.transform.position - (direction.normalized * force * t) + 0.5f * Physics2D.gravity * (t * t);
        return pos;
    }

    public void Throw_Unit()
    {
        if (throw_Unit != null)
        {
            throw_Unit.Throw_Unit();
            throw_Unit = null;
        }
    }
}
