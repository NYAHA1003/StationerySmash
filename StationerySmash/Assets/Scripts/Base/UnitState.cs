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

    public UnitState()
    { 

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
