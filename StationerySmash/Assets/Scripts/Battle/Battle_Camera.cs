using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Battle_Camera : BattleCommand
{
    private Camera camera;
    
    private Vector3 click_Pos;
    private Vector3 cur_Pos;
    private Vector3 mouse_Pos;

    private bool isCameraMove = false;


    public Battle_Camera(BattleManager battleManager, Camera camera) : base(battleManager)
    {
        this.camera = camera;
    }

    public void Set_CameraMove(bool isboolean)
    {
        isCameraMove = isboolean;
    }

    public void Update_CameraPos()
    {
        //카드를 클릭한 상태라면
        if(battleManager.battle_Card.isCardDown)
        {
            isCameraMove = false;
            return;
        }

        mouse_Pos = Input.mousePosition * 0.005f;

        if (Input.GetMouseButtonDown(0) && Input.mousePosition.y > camera.pixelHeight * 0.3f)
        {
            click_Pos = mouse_Pos;
            cur_Pos = camera.transform.position;
            isCameraMove = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isCameraMove = false;
        }

        if (isCameraMove)
        {
            camera.transform.position = new Vector3(cur_Pos.x + (click_Pos.x + -mouse_Pos.x), 0, -10);
            if (battleManager.currentStageData.max_Range + 1f < camera.transform.position.x)
            {
                camera.transform.DOMoveX(battleManager.currentStageData.max_Range, 0.1f);
            }
            if (-battleManager.currentStageData.max_Range - 1f > camera.transform.position.x)
            {
                camera.transform.DOMoveX(-battleManager.currentStageData.max_Range, 0.1f);
            }
        }
    }
}
