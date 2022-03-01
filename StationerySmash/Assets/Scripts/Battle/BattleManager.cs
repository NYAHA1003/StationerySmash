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
    public PencilCaseDataSO pencilCaseDataSO;
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
    public List<CardMove> card_DatasTemp;
    [SerializeField]
    private GameObject card_cardMove_Prefeb;
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
    public List<Unit> unit_MyDatasTemp;
    public List<Unit> unit_EnemyDatasTemp;
    [SerializeField]
    private GameObject unit_Prefeb;
    [SerializeField]
    private Transform unit_PoolManager;
    [SerializeField]
    private Transform unit_Parent;
    [SerializeField]
    private GameObject unit_AfterImage;

    public TextMeshProUGUI unit_teamText;

    #endregion

    #region ī�޶� �ý��� Battle_Camera

    public Battle_Camera battle_Camera { get; private set; }

    [Header("ī�޶�ý��� Battle_Card")]
    [Space(30)]
    [SerializeField]
    public Camera main_Cam;

    #endregion

    #region ����Ʈ �ý��� Battle_Effect

    public Battle_Effect battle_Effect { get; private set; }
    [Header("����Ʈ �ý���")]
    [Space(30)]
    [SerializeField]
    private Transform effect_PoolManager;


    #endregion

    #region ������ �ý��� Battle_Throw

    private Unit throw_Unit;

    #endregion

    private void Awake()
    {
        battle_Card = new Battle_Card(this, unitDataSO, card_cardMove_Prefeb, card_PoolManager, card_Canvas, card_SpawnPosition, card_LeftPosition, card_RightPosition);
        battle_Camera = new Battle_Camera(this, main_Cam);
        battle_Unit = new Battle_Unit(this, unit_Prefeb, unit_PoolManager, unit_Parent, unit_AfterImage);
        battle_Effect = new Battle_Effect(this, effect_PoolManager);
    }

    private void Update()
    {
        battle_Camera.Update_CameraPos();
        battle_Camera.Update_CameraScale();

        if (Input.GetKeyDown(KeyCode.X))
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

        if(Input.GetMouseButtonDown(0))
        {
            Pull_TouchUnit(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        else if(Input.GetMouseButtonUp(0))
        {
            Throw_Unit();
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
        unit_teamText.text = battle_Unit.isMyTeam ? "���� ��" : "��� ��";
    }

    public void Pool_Unit(Unit unit)
    {
        unit.gameObject.SetActive(false);
        unit.transform.SetParent(unit_PoolManager);
    }

    #endregion

    #region ������ �ý���

    public void Pull_TouchUnit(Vector2 pos)
    {
        float targetRange = float.MaxValue;
        for (int i = 0; i < unit_MyDatasTemp.Count; i++)
        {
            if (unit_MyDatasTemp[i].transform.position.sqrMagnitude < targetRange)
            {
                throw_Unit = unit_MyDatasTemp[i];
                targetRange = throw_Unit.transform.position.sqrMagnitude;
            }
        }

        if (throw_Unit != null)
        {
            if (Vector2.Distance(pos, throw_Unit.transform.position) < 0.1f)
            {
                throw_Unit.Pull_Unit();
                return;
            }
            throw_Unit = null;
        }

    }

    public void Throw_Unit()
    {
        if(throw_Unit != null)
        {
            throw_Unit.Throw_Unit();
            throw_Unit = null;
        }
    }

    #endregion
}
