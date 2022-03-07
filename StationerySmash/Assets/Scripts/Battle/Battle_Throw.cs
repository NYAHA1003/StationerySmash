using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
public class Battle_Throw : BattleCommand
{
    private Unit throw_Unit;
    private LineRenderer parabola;
    private Transform arrow;
    private Vector2 touch_Origin_Pos;
    private Vector2 direction;
    private float force;
    private float pullTime;

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
                throw_Unit = throw_Unit.Pull_Unit();
                if(throw_Unit == null)
                {
                    battleManager.battle_Camera.Set_CameraIsMove(false);
                }
                pullTime = 2f;
                return;
            }
            throw_Unit = null;
        }

    }

    public void Draw_Parabola(Vector2 pos)
    {
        if (throw_Unit != null)
        {
            pullTime -= Time.deltaTime;
            if(pullTime < 0)
            {
                throw_Unit = null;
                UnDraw_Parabola();
                return;
            }    

            throw_Unit = throw_Unit.Pulling_Unit();

            if (throw_Unit == null)
            {
                UnDraw_Parabola();
                return;
            }

            battleManager.battle_Camera.Set_CameraIsMove(false);

            //방향
            direction = (Vector2)throw_Unit.transform.position - pos;
            float dir = Mathf.Atan2(direction.y, direction.x);
            float dirx = Mathf.Atan2(direction.y, -direction.x);

            if(dir < 0)
            {
                UnDraw_Parabola();
                return;
            }

            //화살표
            arrow.transform.position = throw_Unit.transform.position;
            arrow.transform.eulerAngles = new Vector3(0, 0, dir * Mathf.Rad2Deg);
            
            //초기 벡터
            force = Mathf.Clamp(Vector2.Distance(throw_Unit.transform.position, pos), 0, 1) * 4;

            //최고점
            float height = Utill.Utill.Caculated_Height(force, dirx);
            //수평 도달 거리
            float width = Utill.Utill.Caculated_Width(force, dirx);
            //수평 도달 시간
            float time = Utill.Utill.Caculated_Time(force, dir, 2);
            
            List<Vector2> linePos = Set_ParabolaPos(parabola.positionCount, width, force, dir, time);

            for (int i = 0; i < parabola.positionCount; i++)
            {
                parabola.SetPosition(i, linePos[i]);
            }
            return;
        }

        UnDraw_Parabola();
    }

    public void UnDraw_Parabola()
    {
        for (int i = 0; i < parabola.positionCount; i++)
        {
            parabola.SetPosition(i, Vector3.zero);
        }
    }

    private List<Vector2> Set_ParabolaPos(int count, float width, float force, float dir_rad, float time)
    {
        List<Vector2> results = new List<Vector2>(count);
        float[] objLerps = new float[count];
        float[] timeLerps = new float[count];
        float interbal = 1f / (count - 1 > 0 ? count - 1 : 1);
        float timeInterbal = time / (count - 1 > 0 ? count - 1 : 1);
        for (int i = 0; i < count; i++)
        {
            objLerps[i] = interbal * i;
            timeLerps[i] = timeInterbal * i;
        }

        for(int i = 0; i < count; i ++)
        {
            Vector3 pos = Vector3.Lerp((Vector2)throw_Unit.transform.position, new Vector2(throw_Unit.transform.position.x - width, 0), objLerps[i]);
            pos.y = Utill.Utill.Caculated_TimeToPos(force, dir_rad, timeLerps[i]);

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
            UnDraw_Parabola();
        }
    }
}
