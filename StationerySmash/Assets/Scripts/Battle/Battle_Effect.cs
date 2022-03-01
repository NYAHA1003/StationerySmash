using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    Attack,
}

public class Battle_Effect : BattleCommand
{
    
    private Transform effect_PoolManager;

    public Battle_Effect(BattleManager battleManager, Transform effect_PoolManager) : base(battleManager)
    {
        this.effect_PoolManager = effect_PoolManager;
    }

    public void Set_Effect(EffectType effectType, Vector2 position)
    {
        Transform effect_Parent = effect_PoolManager.GetChild((int)effectType);
        EffectObject effect_Object = null;
        for (int i = 0; i < effect_Parent.childCount; i++)
        {
            effect_Object = effect_Parent.GetChild(i).GetComponent<EffectObject>();
            if (!effect_Object.gameObject.activeSelf)
            {
                effect_Object.Set_Effect(position);
                return;
            }
        }

        effect_Object = effect_Parent.GetChild(0).GetComponent<EffectObject>();
        effect_Object.Set_Effect(position);
    }
}
