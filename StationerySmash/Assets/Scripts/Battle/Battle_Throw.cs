using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle_Throw : BattleCommand
{
    private Unit throw_Unit;
    private LineRenderer parabola;
    private Transform arrow;
    private Vector2 touch_Origin_Pos;
    private Vector2 direction;
    private float force;
    public Battle_Throw(BattleManager battleManager, LineRenderer parabola, Transform arrow) : base(battleManager)
    {
        this.parabola = parabola;
        this.arrow = arrow;
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
            arrow.transform.position = throw_Unit.transform.position;
            direction = pos - (Vector2)throw_Unit.transform.position;
            arrow.transform.eulerAngles = new Vector3(0,0, Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg);
            force = Mathf.Clamp(Vector2.Distance(throw_Unit.transform.position, pos), 0, 3) * 3;
            float dir = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            dir *= Mathf.Deg2Rad;

            float height = (force * force) * (Mathf.Sin(dir) * Mathf.Sin(dir)) / Mathf.Abs((Physics2D.gravity.y * 2));
            float width = ((force * force) * (Mathf.Sin(dir) * 2)) / Mathf.Abs(Physics2D.gravity.y);

            //Debug.Log(" 각도: " + dir * Mathf.Rad2Deg + " 최고점: " + height + " 이동거리: " + width);


            List<Vector2> linePos = Set_ParabolaPos(parabola.positionCount, width, height);

            for (int i = 0; i < parabola.positionCount; i++)
            {
                parabola.SetPosition(i, linePos[i]);
            }
        }
    }

    private List<Vector2> Set_ParabolaPos(int count, float width, float height)
    {
        List<Vector2> results = new List<Vector2>(count);
        float[] objLerps = new float[count];
        float interbal = 1f / (count - 1 > 0 ? count - 1 : 1);
        for (int i = 0; i < count; i++)
        {
            objLerps[i] = interbal * i;
        }

        for(int i = 0; i < count; i ++)
        {
            Vector3 pos = Vector3.Lerp((Vector2)throw_Unit.transform.position, new Vector2(throw_Unit.transform.position.x - width, 0), objLerps[i]);

            if (objLerps[i] > 0.5f)
            {
                pos.y = Mathf.Lerp(height, 0, objLerps[i]) * 2;
                results.Add(pos);
                continue;
            }
            pos.y = Mathf.Lerp(0, height, objLerps[i]) * 2;

            results.Add(pos);
        }

        return results;
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
