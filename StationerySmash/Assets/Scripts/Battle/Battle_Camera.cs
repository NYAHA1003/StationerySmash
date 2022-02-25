using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle_Camera : BattleCommand
{
    private Vector3 click_Pos;
    private Vector3 cur_Pos;
    private Vector3 mouse_Pos;

    private bool isCameraMove = false;

    private Camera camera;

    public Battle_Camera(BattleManager battleManager, Camera camera) : base(battleManager)
    {
        this.camera = camera;
    }

    public void Update_CameraPos()
    {
        mouse_Pos = Input.mousePosition * 0.005f;

        if (Input.GetMouseButtonDown(0))
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
        }
    }
}
