using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private UnitState unitState;


    private void Start()
    {
        //���� �ʱ�ȭ
    }

    private void Update()
    {
        unitState = unitState.Process();
    }
}
