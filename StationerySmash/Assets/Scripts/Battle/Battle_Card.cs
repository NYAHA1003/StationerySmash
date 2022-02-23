using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Battle_Card : BattleCommand
{
    private int max_Card = 3;
    private int cur_Card = 0;
    private UnitDataSO unitDataSO;
    private GameObject cardMove_Prefeb;
    private Transform card_PoolManager;
    private Transform card_Canvas;
    private RectTransform card_Position;
    private RectTransform card_LeftPosition;
    private RectTransform card_RightPosition;
    private RectTransform card_SpawnPosition;

    public Battle_Card(BattleManager battleManager, UnitDataSO unitDataSO, GameObject card_Prefeb, Transform card_PoolManager, Transform card_Canvas, RectTransform card_Position, RectTransform card_SpawnPosition, RectTransform card_LeftPosition, RectTransform card_RightPosition) 
        : base(battleManager) 
    {
        this.unitDataSO = unitDataSO;
        this.cardMove_Prefeb = card_Prefeb;
        this.card_PoolManager = card_PoolManager;
        this.card_Canvas = card_Canvas;
        this.card_Position = card_Position;
        this.card_SpawnPosition = card_SpawnPosition;
        this.card_RightPosition = card_RightPosition;
        this.card_LeftPosition = card_LeftPosition;
    }

    /// <summary>
    /// 최대 장수까지 카드를 뽑는다
    /// </summary>
    public void Add_AllCard()
    {
        for(; cur_Card < max_Card;)
        {
            Add_OneCard();
        }
    }

    /// <summary>
    /// 카드 한장을 뽑는다
    /// </summary>
    public void Add_OneCard()
    {
        int random = Random.Range(0, unitDataSO.unitDatas.Count);
        cur_Card++;

        CardMove cardmove = Pool_Card();
        cardmove.Set_UnitData(unitDataSO.unitDatas[random]);
        battleManager.cardDatasTemp.Add(cardmove);
        Sort_Card();
    }

    /// <summary>
    /// 카드를 풀링함
    /// </summary>
    private CardMove Pool_Card()
    {
        GameObject cardmove_obj = null;
        if (card_PoolManager.childCount > 0)
        {
            cardmove_obj = card_PoolManager.GetChild(0).gameObject;
            cardmove_obj.transform.position = card_SpawnPosition.position;
            cardmove_obj.SetActive(true);
        }
        cardmove_obj ??= battleManager.Create_Object(cardMove_Prefeb, card_SpawnPosition.position, Quaternion.identity);
        cardmove_obj.transform.SetParent(card_Canvas);
        return cardmove_obj.GetComponent<CardMove>();
    }

    /// <summary>
    /// 카드 위치를 정렬함
    /// </summary>
    public void Sort_Card()
    {
        List<PRS> originCardPRS = new List<PRS>();
        originCardPRS = Return_RoundPRS(battleManager.cardDatasTemp.Count, 1f);

        for(int i = 0; i < battleManager.cardDatasTemp.Count; i++)
        {
            CardMove targetCard = battleManager.cardDatasTemp[i];
            targetCard.originPRS = originCardPRS[i];
            targetCard.Set_CardPosition(targetCard.originPRS, 0.5f);
        }
    }

    private List<PRS> Return_RoundPRS(int objCount, float height)
    {
        float[] objLerps = new float[objCount];
        List<PRS> results = new List<PRS>(objCount);

        switch(objCount)
        {
            case 1:
                objLerps = new float[] { 0.5f };
                break;
            case 2:
                objLerps = new float[] { 0.27f, 0.77f };
                break;
            default:
                float interbal = 1f / (objCount - 1 > 0 ? objCount - 1 : 1);
                for (int i = 0; i < objCount; i++)
                {
                    objLerps[i] = interbal * i;
                }
                break;
        }


        for(int i = 0; i < objCount; i++)
        {
            Vector3 pos = Vector3.Lerp(card_LeftPosition.anchoredPosition, card_RightPosition.anchoredPosition, objLerps[i]);
            
            float curve = Mathf.Sqrt(Mathf.Pow(height, 2) - Mathf.Pow(objLerps[i] - 0.5f, 2));
            pos.y += curve * 800 - 600;
            Quaternion rot = Quaternion.Slerp(card_LeftPosition.rotation, card_RightPosition.rotation, objLerps[i]);
            if (objCount <= 2)
            {
                rot = Quaternion.identity;
            }

            results.Add(new PRS(pos, rot, Vector3.one));
        }

        return results;
    }

    /// <summary>
    /// 최근 뽑은 카드를 지운다
    /// </summary>
    public void Subtract_Card()
    {
        if (cur_Card == 0) 
            return;

        cur_Card--;
        battleManager.cardDatasTemp[cur_Card].transform.SetParent(card_PoolManager);
        battleManager.cardDatasTemp[cur_Card].gameObject.SetActive(false);
        battleManager.cardDatasTemp.RemoveAt(cur_Card);
    }

    /// <summary>
    /// 모든 카드를 지운다
    /// </summary>
    public  void Clear_Cards()
    {
        for(;cur_Card > 0;)
        {
            Subtract_Card();
        }
    }

    public void Check_MouseOver(CardMove card)
    {
        Debug.Log("CardMouseOver");
    }
    public void Check_MouseExit(CardMove card)
    {
        Debug.Log("CardMouseExit");
    }
    public void Check_MouseDown(CardMove card)
    {
        Debug.Log("CardMouseDown");
    }
    public void Check_MouseUp(CardMove card)
    {
        Debug.Log("CardMouseUp");
    }

    public void Set_SizeCard(CardMove card , bool isSizeUp)
    {
        if(isSizeUp)
        {
            Vector3 sizeUpPos = new Vector3(card.originPRS.pos.x, -200f, -10);
            card.Set_CardPosition(new PRS(sizeUpPos, Quaternion.identity, Vector3.one * 1.5f), 0.3f);

            return;
        }
        card.Set_CardPosition(card.originPRS, 0.3f);

    }
}
