using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class BattleManager : MonoBehaviour
{
    #region �����͵�

    [Header("���� �����͵�")]
    [Space(30)]
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

        private set {}
    }

    #endregion

    #region ī�� �ý��� Battle_Card

    public Battle_Card battle_Card { get; private set;}

    [Header("ī��ý��� Battle_Card")]
    [Space(30)]
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

    #region ���� �ý��� Battle_Unit

    public Battle_Unit battle_Unit { get; private set; }

    [Header("���ֽý��� Battle_Unit")]
    [Space(30)]
    public List<Unit> unitMyDatasTemp;
    public List<Unit> unitEnemyDatasTemp;
    [SerializeField]
    private GameObject unit_Prefeb;
    [SerializeField]
    private Transform unit_PoolManager;
    [SerializeField]
    private Transform unit_Parent;
    [SerializeField]
    private GameObject unit_AfterImage;

    public TextMeshProUGUI teamText;

    #endregion

    #region ī�޶� �ý��� Battle_Camera

    public Battle_Camera battle_Camera { get; private set; }

    [Header("ī�޶�ý��� Battle_Card")]
    [Space(30)]
    [SerializeField]
    public Camera main_Cam;

    #endregion

    private void Start()
    {
        battle_Card = new Battle_Card(this, unitDataSO, cardMove_Prefeb, card_PoolManager, card_Canvas, card_SpawnPosition, card_LeftPosition, card_RightPosition);
        battle_Camera = new Battle_Camera(this, main_Cam);
        battle_Unit = new Battle_Unit(this, unit_Prefeb, unit_PoolManager, unit_Parent, unit_AfterImage);
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

    #region �����Լ�
    public GameObject Create_Object(GameObject gameobj, Vector3 position, Quaternion quaternion)
    {
        return Instantiate(gameobj, position, quaternion);
    }

    #endregion

    #region ���� �ý��� �Լ� Battle_Unit

    public void Change_Team()
    {
        battle_Unit.isMyTeam = !battle_Unit.isMyTeam;
        teamText.text = battle_Unit.isMyTeam ? "���� ��" : "��� ��";
    }

    #endregion

}
