using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NestedScrollBar : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Scrollbar scrollbar;
    [SerializeField]
    private Transform contentTr;
    [SerializeField]
    private Scrollbar[] yScrollBars;
    [SerializeField]
    private Slider accentSlider;
    [SerializeField]
    private RectTransform accentTab;
    [SerializeField]
    private RectTransform[] btns;

    const int SIZE = 3;
    float[] pos = new float[SIZE];
    float distance, curPos, targetPos;
    bool isDrag;
    int targetIndex;


    [SerializeField]
    private bool isXSlide;
    void Start()
    {
        distance = 1f / (SIZE - 1);
        for (int i = 0; i < SIZE; i++) pos[i] = distance * i;
    }

    float SetPos()
    {
        for (int i = 0; i < SIZE; i++)
        {
            if (scrollbar.value < pos[i] + distance * 0.5f && scrollbar.value > pos[i] - distance * 0.5f)
            {
                targetIndex = i;
                return pos[i];
            }
        }
        return 0;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        curPos = SetPos();
    }
    public void OnDrag(PointerEventData eventData)
    {
        isDrag = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;
        targetPos = SetPos();
        if (isXSlide) SetOriginScroll();
        if (curPos == targetPos)
        {
            print(eventData.delta.x);
            if (isXSlide)
            {
                deltaSlide(eventData.delta.x);
                SetOriginScroll();
            }
            else
            {
                deltaSlide(eventData.delta.y);
            }
        }
        if (isXSlide)
        {
            for (int i = 0; i < SIZE; i++)
            {
                btns[i].sizeDelta = new Vector2((targetIndex == i) ? 320 : 160, btns[i].sizeDelta.y);
            }
        }
    }

    void SetOriginScroll()
    {
        Debug.Log("½ÇÇà");
        for (int i = 0; i < SIZE; i++)
        {
            if (contentTr.GetChild(i).GetComponent<ScrollScript>() && pos[i] != curPos && pos[i] == targetPos)
            {
                yScrollBars[i].value = 1;
            }
        }
    }

    void deltaSlide(float deltaValue)
    {
        if (deltaValue > 18 && curPos - distance >= 0)
        {
            --targetIndex;
            targetPos = curPos - distance;
        }
        else if (deltaValue < -18 && curPos + distance <= 1.01f)
        {
            ++targetIndex;
            targetPos = curPos + distance;
        }
    }

    public void OnMovePanel(int n)
    {
        targetIndex = n;
        targetPos = pos[n];
    }
    void Update()
    {
        if (isXSlide) accentSlider.value = Mathf.Lerp(accentSlider.value, scrollbar.value, 0.2f);
        if (!isDrag)
        {
            scrollbar.value = Mathf.Lerp(scrollbar.value, targetPos, 0.1f);
        }
    }
}
