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
            //ȭ��ǥ
            arrow.transform.position = throw_Unit.transform.position;
            arrow.transform.eulerAngles = new Vector3(0,0, Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg);
            
            //����
            direction = pos - (Vector2)throw_Unit.transform.position;
            //�ʱ� ����
            force = Mathf.Clamp(Vector2.Distance(throw_Unit.transform.position, pos), 0, 3) * 2;
            //������ ���Ȱ����� �ٲ۰�
            float dir = (Mathf.Atan2(-direction.y, -direction.x) - 90) * Mathf.Rad2Deg;
            dir *= Mathf.Deg2Rad;

            //�ְ���
            float height = (force * force) * (Mathf.Sin(dir) * Mathf.Sin(dir)) / Mathf.Abs((Physics2D.gravity.y * 2));
            //���� ���� �Ÿ�
            float width = ((force * force) * (Mathf.Sin(dir) * 2)) / Mathf.Abs(Physics2D.gravity.y);
            //���� ���� �ð�
            float time = force * Mathf.Sin(dir) / Mathf.Abs(Physics2D.gravity.y);
            time *= 2;

            //Debug.Log(" ����: " + dir * Mathf.Rad2Deg + " �ְ���: " + height + " �̵��Ÿ�: " + width + " �ð�: " + time);


            List<Vector2> linePos = Set_ParabolaPos(parabola.positionCount, width, force, dir, time);

            for (int i = 0; i < parabola.positionCount; i++)
            {
                parabola.SetPosition(i, linePos[i]);
            }
        }
    }

    private List<Vector2> Set_ParabolaPos(int count, float width, float force, float rad, float time)
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
            pos.y = (force * timeLerps[i] * Mathf.Sin(rad)) - (Mathf.Abs(Physics2D.gravity.y / 2)  * (timeLerps[i] * timeLerps[i]));
            //Debug.Log(i + " / " + count + " ���� �ð�: " + time + " �ð�: " + timeLerps[i] + " ����: " + (force * timeLerps[i] * Mathf.Sin(rad)) + " �� ����: " + (Mathf.Abs(Physics2D.gravity.y / 2) * (timeLerps[i] * timeLerps[i])) + " ���� ����: " + pos.y);

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
