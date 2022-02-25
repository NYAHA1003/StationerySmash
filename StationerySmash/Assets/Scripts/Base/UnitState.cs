using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UnitState
{
    public enum eState  // ���� �� �ִ� ���� ����
    {
        IDLE, MOVE, ATTACK, WAIT, DAMAGED, DIE
    };

    public enum eEvent  // �̺�Ʈ ����
    {
        ENTER, UPDATE, EXIT, NOTHING
    };

    protected eState curState;
    protected eEvent curEvent;

    protected UnitState nextState;  // ���� ����

    protected Transform myTrm;
    protected Transform mySprTrm;
    protected Unit myUnit;
    protected UnitData myUnitData;

    public UnitState(Transform myTrm, Transform mySprTrm, Unit myUnit)
    {
        this.myTrm = myTrm;
        this.mySprTrm = mySprTrm;
        this.myUnit = myUnit;
        myUnitData = myUnit.unitData;
    }



    public virtual void Enter() { curEvent = eEvent.UPDATE; }
    public virtual void Update() { curEvent = eEvent.UPDATE; }
    public virtual void Exit() { curEvent = eEvent.EXIT; }

    /// <summary>
    /// ���� ����
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
