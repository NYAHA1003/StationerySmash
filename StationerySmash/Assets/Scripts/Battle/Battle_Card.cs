using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle_Card : BattleCommand
{
    private int max_Card;

    public Battle_Card(BattleManager battleManager) : base(battleManager) { }

    /// <summary>
    /// �ִ� ������� ī�带 �̴´�
    /// </summary>
    public void Add_AllCard()
    {
        for(int i = 0; i < max_Card; i++)
        {
            Add_OneCard();
        }
    }

    /// <summary>
    /// ī�� ������ �̴´�
    /// </summary>
    public void Add_OneCard()
    {

    }
}
