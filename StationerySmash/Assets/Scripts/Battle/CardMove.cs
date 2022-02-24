using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;


[System.Serializable]
public class PRS
{
    public Vector3 pos;
    public Quaternion rot;
    public Vector3 scale;

    public PRS(Vector3 pos, Quaternion rot, Vector3 scale)
    {
        this.pos = pos;
        this.rot = rot;
        this.scale = scale;
    }
}

public class CardMove : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private Image card_Background;
    [SerializeField]
    private Image card_UnitImage;
    [SerializeField]
    private TextMeshProUGUI card_UnitCost;
    [SerializeField]
    private Image card_Grade;
    [SerializeField]
    private TextMeshProUGUI card_GradeText;
    [SerializeField]
    private TextMeshProUGUI card_Name;
    [SerializeField]
    private Image fusion_Effect;


    public UnitData unitData;

    public int grade = 1;
    public int id;
    private float scale;

    private RectTransform rectTransform;

    private BattleManager battleManager;

    private Canvas cardCanvas;

    private void Awake()
    {
        battleManager = FindObjectOfType<BattleManager>();
        rectTransform = GetComponent<RectTransform>();
    }

    public PRS originPRS;

    public void Set_UnitData(UnitData unitData, int id)
    {
        cardCanvas ??= transform.parent.GetComponent<Canvas>();
        
        this.id = id;
        fusion_Effect.color = new Color(1, 1, 1, 1);
        fusion_Effect.DOFade(0, 0.8f);
        grade = 0;
        this.unitData = unitData;
        card_Name.text = unitData.name;
        card_UnitCost.text = unitData.cost.ToString();
        card_UnitImage = null;
        Set_UnitGrade();
    }

    public void Set_CardPosition(PRS prs, float duration, bool isDotween = true)
    {
        if (isDotween)
        {
            rectTransform.DOAnchorPos(prs.pos, duration);
            rectTransform.DORotateQuaternion(prs.rot, duration);
            rectTransform.DOScale(prs.scale, duration);
            return;
        }
        rectTransform.anchoredPosition = prs.pos;
        rectTransform.rotation = prs.rot;
        rectTransform.localScale = prs.scale;
    }

    public void Set_Size(float size, float duration)
    {
        rectTransform.DOScale(size, duration);
    }

    /// <summary>
    /// 유닛 단계 이미지 설정
    /// </summary>
    public void Set_UnitGrade()
    {
        card_GradeText.text = grade.ToString();
        switch (grade)
        {
            default:
            case 0:
            case 1:
                card_Grade.color = new Color(0, 0, 0);
                break;
            case 2:
                card_Grade.color = new Color(1, 1, 0);
                break;
            case 3:
                card_Grade.color = new Color(1, 1, 1);
                break;
        }
    }

    /// <summary>
    /// 유닛 업그레이드 
    /// </summary>
    public void Upgrade_UnitGrade()
    {
        grade++;
        Set_UnitGrade();
    }

    public void Fusion_FadeInEffect()
    {
        fusion_Effect.DOFade(1, 0.3f);
    }
    public void Fusion_FadeOutEffect()
    {
        fusion_Effect.DOFade(0, 0.3f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Vector2 localPos;
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mouseScrollDelta, cardCanvas.worldCamera, out localPos);
        //Set_CardPosition(new PRS(cardCanvas.transform.TransformPoint(localPos), Quaternion.identity, Vector3.one), 0, false);

        transform.position = Input.mousePosition;
        Set_Size(1.3f, 0.3f);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(rectTransform.anchoredPosition.y > 0)
        {
            battleManager.battle_Card.Check_MouseClick(this);
            Set_Size(1f, 0.3f);
            return;
        }

        Set_CardPosition(originPRS, 0.3f);
        Set_Size(1f, 0.3f);
        battleManager.battle_Card.Check_MouseExit(this);

    }
}
