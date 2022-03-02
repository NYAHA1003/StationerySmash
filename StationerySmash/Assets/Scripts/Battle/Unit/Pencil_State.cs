using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pencil_State : Stationary_UnitState
{
    public Pencil_State(Transform myTrm, Transform mySprTrm, Stationary_Unit myUnit) : base(myTrm, mySprTrm, myUnit)
    {
        this.myUnit = myUnit;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class Pencil_Idle_State : Pencil_State
{
    public Pencil_Idle_State(Transform myTrm, Transform mySprTrm, Stationary_Unit myUnit) : base(myTrm, mySprTrm, myUnit)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        nextState = new Pencil_Wait_State(myTrm, mySprTrm, myUnit, 0.5f);
        curEvent = eEvent.EXIT;
    }

    public override void Exit()
    {
        base.Exit();
    }
}
public class Pencil_Wait_State : Pencil_State
{
    private float waitTime;
    public Pencil_Wait_State(Transform myTrm, Transform mySprTrm, Stationary_Unit myUnit, float waitTime) : base(myTrm, mySprTrm, myUnit)
    {
        this.waitTime = waitTime;
    }

    public override void Update()
    {
        if (waitTime > 0)
        {
            waitTime -= Time.deltaTime;
            return;
        }
        nextState = new Pencil_Move_State(myTrm, mySprTrm, myUnit);
        curEvent = eEvent.EXIT;
    }
}

public class Pencil_Move_State : Pencil_State
{
    public Pencil_Move_State(Transform myTrm, Transform mySprTrm, Stationary_Unit myUnit) : base(myTrm, mySprTrm, myUnit)
    {
    }

    public override void Update()
    {
        //우리 팀
        if(myUnit.isMyTeam)
        {
            Move_MyTeam();
            Check_Range(myUnit.battleManager.unit_EnemyDatasTemp);
            return;
        }

        //상대 팀
        Move_EnemyTeam();
        Check_Range(myUnit.battleManager.unit_MyDatasTemp);
    }

    private void Move_MyTeam()
    {
        myTrm.Translate(Vector2.right * myUnitData.moveSpeed * Time.deltaTime);
    }

    private void Move_EnemyTeam()
    {
        myTrm.Translate(Vector2.left * myUnitData.moveSpeed * Time.deltaTime);
    }

    private void Check_Range(List<Unit> list)
    {
        float targetRange = float.MaxValue;
        Unit targetUnit = null;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].transform.position.sqrMagnitude < targetRange)
            {
                if (myUnit.isMyTeam && myTrm.position.x > list[i].transform.position.x)
                {
                    continue;
                }
                if (!myUnit.isMyTeam && myTrm.position.x < list[i].transform.position.x)
                {
                    continue;
                }
                if(list[i].transform.position.y > myTrm.transform.position.y)
                {
                    continue;
                }

                targetUnit = list[i];
                targetRange = targetUnit.transform.position.sqrMagnitude;
            }
        }
        
        if(targetUnit != null)
        {
            if (Vector2.Distance(myTrm.position, targetUnit.transform.position) < myUnitData.range)
            {
                nextState = new Pencil_Attack_State(myTrm, mySprTrm, myUnit, targetUnit);
                curEvent = eEvent.EXIT;
            }
        }

    }
}

public class Pencil_Attack_State : Pencil_State
{
    private Unit targetUnit;
    private float cur_delay = 0;
    private float max_delay = 100;
    public Pencil_Attack_State(Transform myTrm, Transform mySprTrm, Stationary_Unit myUnit, Unit targetUnit) : base(myTrm, mySprTrm, myUnit)
    {
        this.targetUnit = targetUnit;
    }

    public override void Enter()
    {
        cur_delay = myUnit.attack_Cur_Delay;
        base.Enter();
    }
    public override void Update()
    {
        //상대와의 거리 체크
        Check_Range();

        //쿨타임 감소
        if(max_delay >= cur_delay)
        {
            cur_delay += myUnitData.attackSpeed * Time.deltaTime;
            Set_Delay();
            return;
        }

        Attack();
    }

    private void Attack()
    {
        cur_delay = 0;
        Set_Delay();
        myUnit.battleManager.battle_Effect.Set_Effect(EffectType.Attack, targetUnit.transform.position);
        targetUnit.Run_Damaged(myUnit, 10, myUnitData.knockback);
        nextState = new Pencil_Wait_State(myTrm, mySprTrm, myUnit, 0.4f);
        curEvent = eEvent.EXIT;
    }

    private void Set_Delay()
    {
        myUnit.Update_DelayBar(cur_delay / max_delay);
        myUnit.Set_AttackDelay(cur_delay);
    }

    private void Check_Range()
    {
        if (targetUnit != null)
        {
            if (Vector2.Distance(myTrm.position, targetUnit.transform.position) > myUnitData.range)
            {
                Set_Move();
                return;
            }

            if (myUnit.isMyTeam && myTrm.position.x > targetUnit.transform.position.x)
            {
                Set_Move();
                return;
            }
            if (!myUnit.isMyTeam && myTrm.position.x < targetUnit.transform.position.x)
            {
                Set_Move();
                return;
            }
            if(targetUnit.transform.position.y > myTrm.position.y)
            {
                Set_Move();
                return;
            }
        }
    }

    private void Set_Move()
    {
        nextState = new Pencil_Move_State(myTrm, mySprTrm, myUnit);
        curEvent = eEvent.EXIT;
    }
}



public class Pencil_Damaged_State : Pencil_State
{
    private float knockback;
    private int damaged;
    private Unit attacker;
    public Pencil_Damaged_State(Transform myTrm, Transform mySprTrm, Stationary_Unit myUnit, Unit attacker, int damaged, float knockback) : base(myTrm, mySprTrm, myUnit)
    {
        this.damaged = damaged;
        this.knockback = knockback;
        this.attacker = attacker;
    }

    public override void Enter()
    {
        //데미지 입음
        myUnit.Subtract_HP(damaged);
        KnockBack();
        Debug.Log(myUnit.name + "의 체력 : " + myUnit.hp);
        base.Enter();
    }

    private void KnockBack()
    {
        float calculated_knockback = (knockback / (myUnitData.weight * (((float)myUnit.hp / myUnit.maxhp) + 0.1f))) * (myUnit.isMyTeam ? 1 : -1);
        Debug.Log(calculated_knockback);
        myTrm.DOJump(new Vector3(myTrm.position.x - calculated_knockback, 0, myTrm.position.z), 0.3f, 1, 0.2f);
    }

    public override void Update()
    {
        nextState = new Pencil_Wait_State(myTrm, mySprTrm, myUnit, 0.2f);
        if (myUnit.hp <= 0 )
        {
            nextState = new Pencil_Die_State(myTrm, mySprTrm, myUnit);
        }
        curEvent = eEvent.EXIT;
    }
}

public class Pencil_Die_State : Pencil_State
{
    public Pencil_Die_State(Transform myTrm, Transform mySprTrm, Stationary_Unit myUnit) : base(myTrm, mySprTrm, myUnit)
    {
    }

    public override void Enter()
    {
        //뒤짐
        Debug.Log("뒤짐");
        myUnit.Delete_Unit();
        base.Enter();
    }

}

public class Pencil_Pull_State : Pencil_State
{
    public Pencil_Pull_State(Transform myTrm, Transform mySprTrm, Stationary_Unit myUnit) : base(myTrm, mySprTrm, myUnit)
    {
    }

    public override void Enter()
    {
        nextState = new Pencil_Pull_State(myTrm, mySprTrm, myUnit);
    }
}

public class Pencil_Throw_State : Pencil_State
{
    public Pencil_Throw_State(Transform myTrm, Transform mySprTrm, Stationary_Unit myUnit) : base(myTrm, mySprTrm, myUnit)
    {
    }

    public override void Enter()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        //방향
        Vector2 direction = (Vector2)myTrm.position - mousePos;
        float dir = Mathf.Atan2(direction.y, direction.x);
        float dirx = Mathf.Atan2(direction.y, -direction.x);

        //초기 벡터
        float force = Mathf.Clamp(Vector2.Distance(myTrm.position, mousePos), 0, 2) * 4;



        //최고점
        float height = (force * force) * (Mathf.Sin(dirx) * Mathf.Sin(dirx)) / Mathf.Abs((Physics2D.gravity.y * 2));
        //수평 도달 거리
        float width = ((force * force) * (Mathf.Sin(dirx * 2))) / Mathf.Abs(Physics2D.gravity.y);
        //수평 도달 시간
        float time = force * Mathf.Sin(dir) / Mathf.Abs(Physics2D.gravity.y);
        time *= 2;



        myTrm.DOJump(new Vector3(myTrm.position.x - width, 0, myTrm.position.z), height, 1, time).OnComplete(() =>
        {
            nextState = new Pencil_Wait_State(myTrm, mySprTrm, myUnit, 0.5f);
            curEvent = eEvent.EXIT;
        });
        
        base.Enter();
    }

    public override void Update()
    {
        if(myUnit.isMyTeam)
        {
            Check_Collide(myUnit.battleManager.unit_EnemyDatasTemp);
            return;
        }
        Check_Collide(myUnit.battleManager.unit_MyDatasTemp);
    }
    private void Check_Collide(List<Unit> list)
    {
        Unit targetUnit = null;
        for (int i = 0; i < list.Count; i++)
        {
            targetUnit = list[i];
            if (Vector2.Distance(myTrm.position, targetUnit.transform.position) < 0.2f)
            {
                targetUnit.Run_Damaged(myUnit, 10, 1);
            }
        }
    }
}