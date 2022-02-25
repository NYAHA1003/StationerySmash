using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    private UnitState unitState;

    [SerializeField]
    private SpriteRenderer spr;

    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private Image delayBar;

    public UnitData unitData;

    private bool isSettingEnd;
    public bool isMyTeam;

    public BattleManager battleManager;

    private Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        if (!isSettingEnd) return;

        unitState = unitState.Process();
    }

    public void Set_UnitData(UnitData unitData, bool isMyTeam, BattleManager battleManager)
    {
        this.unitData = unitData;
        this.isMyTeam = isMyTeam;
        spr.color = isMyTeam ? Color.red : Color.blue;
        spr.sprite = unitData.sprite;
        this.battleManager = battleManager;
        canvas.worldCamera = mainCam;
        delayBar.rectTransform.anchoredPosition = isMyTeam ? new Vector2(-960.15f, -540.15f) : new Vector2(-959.85f, -540.15f);


        unitState = new Pencil_State(transform, spr.transform, this);

        isSettingEnd = true;
    }

    public void Update_DelayBar(float delay)
    {
        delayBar.fillAmount = delay;
    }
}
