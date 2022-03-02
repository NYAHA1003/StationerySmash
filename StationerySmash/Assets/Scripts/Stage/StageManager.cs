using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StageManager : MonoBehaviour
{

    [SerializeField]
    private GameObject selectArrow;
    //[SerializeField]
    //private GameObject[] chapter_GameObj;
    [SerializeField]
    private GameObject IndexPanel_GameObj;
    //[SerializeField]
    //private string[]

    private bool isCheck = true;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Chapter_Select()
    {
    //    if(isCheck)
    //    {
    //        IndexPanel_GameObj.SetActive(false);
    //        isCheck = false;
    //    }
        IndexPanel_GameObj.SetActive(isCheck);
        isCheck = !isCheck;
    }
}