using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private UnitState unitState;


    private void Start()
    {
        //¿Ø¥÷ √ ±‚»≠
    }

    private void Update()
    {
        unitState = unitState.Process();
    }
}
