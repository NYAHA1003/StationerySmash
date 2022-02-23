using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

public class CardMove : MonoBehaviour
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
    private TextMeshProUGUI card_Name;

    private UnitData unitData;

    private RectTransform rectTransform;

    private void Awake() => rectTransform = GetComponent<RectTransform>();

    public PRS originPRS;

    public void Set_UnitData(UnitData unitData)
    {
        this.unitData = unitData;
        card_Name.text = unitData.name;
        card_UnitCost.text = unitData.cost.ToString();
        card_UnitImage = null;
        Set_UnitGrade();
    }

    public void Set_CardPosition(PRS prs, float duration)
    {
        rectTransform.DOAnchorPos(prs.pos, duration);
        rectTransform.DORotateQuaternion(prs.rot, duration);
        rectTransform.DOScale(prs.scale, duration);
    }

    public void Set_UnitGrade()
    {
        switch(unitData.grade)
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

}
