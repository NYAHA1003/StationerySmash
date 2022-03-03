using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Utill;

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
        curState = eState.IDLE;
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
        curState = eState.WAIT;
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
        curState = eState.MOVE;
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
            if (Vector2.Distance(myTrm.position, list[i].transform.position) < targetRange)
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
                if(list[i].isInvincibility)
                {
                    continue;
                }

                targetUnit = list[i];
                targetRange = Vector2.Distance(myTrm.position, targetUnit.transform.position);
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
        curState = eState.ATTACK;
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
        if(max_delay >= cur_delay || targetUnit.isInvincibility)
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
        targetUnit.Run_Damaged(myUnit, 10, myUnitData.knockback, myUnit.isMyTeam ? myUnitData.dir : 180 - myUnitData.dir, 0);
        targetUnit = null;
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
    private KBData kbData;
    private int damaged;

    private Unit attacker;
    public Pencil_Damaged_State(Transform myTrm, Transform mySprTrm, Stationary_Unit myUnit, Unit attacker, int damaged, KBData kbData) : base(myTrm, mySprTrm, myUnit)
    {
        curState = eState.DAMAGED;
        this.kbData = kbData;
        this.damaged = damaged;
        this.attacker = attacker;
    }

    public override void Enter()
    {
        myUnit.Set_IsInvincibility(true);
        myUnit.Subtract_HP(damaged);
        KnockBack();
        base.Enter();
    }

    private void KnockBack()
    {
        float calculated_knockback = kbData.Caculated_Knockback(myUnitData.weight, myUnit.hp, myUnit.maxhp, myUnit.isMyTeam);
        float height = Utill_Parabola.Caculated_Height((kbData.baseKnockback + kbData.extraKnockback) * 0.15f, kbData.direction, 1, 0.1f);
        float time = Mathf.Abs((kbData.baseKnockback * 0.5f + kbData.extraKnockback)  / (Physics2D.gravity.y ));
        
        myTrm.DOKill();
        myTrm.DOJump(new Vector3(myTrm.position.x - calculated_knockback, 0, myTrm.position.z), height, 1, time);
        Debug.Log("최고점: " + height + " 시간: " + time  + " 넉백: " + (kbData.baseKnockback + kbData.extraKnockback));
    }

    public override void Update()
    {
        nextState = new Pencil_Wait_State(myTrm, mySprTrm, myUnit, 0.5f);
        if (myUnit.hp <= 0 )
        {
            nextState = new Pencil_Die_State(myTrm, mySprTrm, myUnit);
        }
        curEvent = eEvent.EXIT;
    }

    public override void Exit()
    {
        myUnit.Set_IsInvincibility(false);
        base.Exit();
    }
}

public class Pencil_Die_State : Pencil_State
{
    public Pencil_Die_State(Transform myTrm, Transform mySprTrm, Stationary_Unit myUnit) : base(myTrm, mySprTrm, myUnit)
    {
        curState = eState.DIE;
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
        curState = eState.PULL;
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
        curState = eState.THROW;
    }

    public override void Enter()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        //방향
        Vector2 direction = (Vector2)myTrm.position - mousePos;
        float dir = Mathf.Atan2(direction.y, direction.x);
        float dirx = Mathf.Atan2(direction.y, -direction.x);

        if(dir < 0)
        {
            nextState = new Pencil_Wait_State(myTrm, mySprTrm, myUnit, 0.5f);
            curEvent = eEvent.EXIT;
            return;
        }

        //초기 벡터
        float force = Mathf.Clamp(Vector2.Distance(myTrm.position, mousePos), 0, 1) * 4;

        //최고점
        float height = Utill_Parabola.Caculated_Height(force, dirx);
        //수평 도달 거리
        float width = Utill_Parabola.Caculated_Width(force, dirx);
        //수평 도달 시간
        float time = Utill_Parabola.Caculated_Time(force, dir, 3);

        myTrm.DOJump(new Vector3(myTrm.position.x - width, 0, myTrm.position.z), height, 1, time).OnComplete(() =>
        {
            nextState = new Pencil_Wait_State(myTrm, mySprTrm, myUnit, 0.5f);
            curEvent = eEvent.EXIT;
        }).SetEase(myUnit.curve);
        
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
                Check_WeightDamage(targetUnit);
            }
        }
    }

    private void Check_WeightDamage(Unit targetUnit)
    {

        //무게가 더 클 경우
        if (myUnit.weight > targetUnit.weight)
        {

            return;
        }

        //무게가 더 작을 경우
        if (myUnit.weight < targetUnit.weight)
        {

            return;
        }

        //무게가 같을 경우
        if (myUnit.weight == targetUnit.weight)
        {
            float dir = Vector2.Angle((Vector2)myTrm.position, (Vector2)targetUnit.transform.position);
            float extraKnockBack = (targetUnit.weight - myUnitData.weight * (float)targetUnit.hp / targetUnit.maxhp) * 0.025f;

            targetUnit.Run_Damaged(myUnit, 10, 10, 180 - dir, extraKnockBack);
            nextState = new Pencil_Damaged_State(myTrm, mySprTrm, myUnit, myUnit, 0, new KBData(10, extraKnockBack, dir));
            curEvent = eEvent.EXIT;

            return;
        }
    }
}

public class Pencil_Stun_State : Pencil_State
{
    private float stunTime;
    public Pencil_Stun_State(Transform myTrm, Transform mySprTrm, Stationary_Unit myUnit, float stunTime) : base(myTrm, mySprTrm, myUnit)
    {
        curState = eState.STUN;
        this.stunTime = stunTime;
    }

    public override void Enter()
    {
        //myUnit.battleManager.battle_Effect.Set_Effect(EffectType.Stun, myTrm.position, stunTime, true);
        base.Enter();
    }

    public override void Update()
    {
        if (stunTime > 0)
        {
            stunTime -= Time.deltaTime;
            return;
        }
        nextState = new Pencil_Move_State(myTrm, mySprTrm, myUnit);
        curEvent = eEvent.EXIT;
    }
}
