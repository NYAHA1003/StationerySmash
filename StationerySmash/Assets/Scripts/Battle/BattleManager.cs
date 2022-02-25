using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    #region 데이터들

    [SerializeField]
    private UnitDataSO unitDataSO;
    [SerializeField]
    private StageDataSO stageDataSO;

    public StageData currentStageData
    {
        get
        {
            return stageDataSO.stageDatas[0];
        }

        private set
        {

        }
    }

    #endregion

    #region 카드 시스템 Battle_Card

    public Battle_Card battle_Card { get; private set;}
    public List<CardMove> cardDatasTemp;
    [SerializeField]
    private GameObject cardMove_Prefeb;
    [SerializeField]
    private Transform card_PoolManager;
    [SerializeField]
    private Transform card_Canvas;
    [SerializeField]
    private RectTransform card_SpawnPosition;
    [SerializeField]
    private RectTransform card_LeftPosition;
    [SerializeField]
    private RectTransform card_RightPosition;

    #endregion

    #region 카메라 시스템 Battle_Camera

    public Battle_Camera battle_Camera { get; private set; }
   
    [SerializeField]
    public Camera main_Cam;

    #endregion

    private void Start()
    {
        battle_Card = new Battle_Card(this, unitDataSO, cardMove_Prefeb, card_PoolManager, card_Canvas, card_SpawnPosition, card_LeftPosition, card_RightPosition);
        battle_Camera = new Battle_Camera(this, main_Cam);
    }

    private void Update()
    {
        battle_Camera.Update_CameraPos();

        if(Input.GetKeyDown(KeyCode.X))
        {
            battle_Card.Add_OneCard();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            battle_Card.Add_AllCard();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            battle_Card.Clear_Cards();
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            battle_Card.Subtract_Card();
        }
    }

    #region 공용함수
    public GameObject Create_Object(GameObject gameobj, Vector3 position, Quaternion quaternion)
    {
        return Instantiate(gameobj, position, quaternion);
    }

    #endregion

}
