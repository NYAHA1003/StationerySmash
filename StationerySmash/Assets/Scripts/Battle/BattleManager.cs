using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    #region 데이터들

    [SerializeField]
    private UnitDataSO unitDataSO;

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


    private void Start()
    {
        battle_Card = new Battle_Card(this, unitDataSO, cardMove_Prefeb, card_PoolManager, card_Canvas, card_SpawnPosition, card_LeftPosition, card_RightPosition);
    }

    private void Update()
    {
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
