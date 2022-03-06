using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stationary_Unit_Normal_Eff_State : UnitState
{
    protected new Stationary_Unit myUnit;
    protected UnitData myUnitData;
    public Stationary_Unit_Normal_Eff_State(Transform myTrm, Transform mySprTrm, Unit myUnit) : base(myTrm, mySprTrm, myUnit)
    {
    }

    public override void Exit()
    {
        curEvent = eEvent.EXIT;
        base.Exit();
    }
}
public class Stationary_Unit_Sturn_Eff_State : Stationary_Unit_Normal_Eff_State
{
    private float stunTime;
    public Stationary_Unit_Sturn_Eff_State(Transform myTrm, Transform mySprTrm, Unit myUnit, float stunTime) : base(myTrm, mySprTrm, myUnit)
    {
        curState = eState.STUN;
        this.stunTime = stunTime;
    }
    public override void Enter()
    {
        stunTime = stunTime + (stunTime * (((float)myUnit.maxhp / (myUnit.hp + 0.1f)) - 1));

        base.Enter();
    }

    public override void Update()
    {
        if (stunTime > 0)
        {
            stunTime -= Time.deltaTime;
            return;
        }
        nextState = new Stationary_Unit_Normal_Eff_State(myTrm, mySprTrm, myUnit);
        curEvent = eEvent.EXIT;
    }

}