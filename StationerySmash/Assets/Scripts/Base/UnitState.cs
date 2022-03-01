using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UnitState
{
    public enum eState  // 가질 수 있는 상태 나열
    {
        IDLE, MOVE, ATTACK, WAIT, DAMAGED, DIE
    };

    public enum eEvent  // 이벤트 나열
    {
        ENTER, UPDATE, EXIT, NOTHING
    };

    protected eState curState;
    protected eEvent curEvent;

    protected UnitState nextState;  // 다음 상태

    protected Transform myTrm;
    protected Transform mySprTrm;
    protected Unit myUnit;

    public UnitState(Transform myTrm, Transform mySprTrm, Unit myUnit)
    {
        this.myTrm = myTrm;
        this.mySprTrm = mySprTrm;
        this.myUnit = myUnit;
    }

    public virtual void Enter() { curEvent = eEvent.UPDATE; }
    public virtual void Update() { curEvent = eEvent.UPDATE; }
    public virtual void Exit() { curEvent = eEvent.EXIT; }

    /// <summary>
    /// 로직 실행
    /// </summary>
    /// <returns></returns>
    public UnitState Process()
    {
        if (curEvent == eEvent.ENTER)
        {
            Enter();
        }
        if (curEvent == eEvent.UPDATE)
        {
            Update();
        }
        if (curEvent == eEvent.EXIT)
        {
            Exit();
            return nextState;
        }

        return this;
    }

}
