using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System; 
public enum PanelType
{
    Sticker,
    Ttagji,
    Dalgona
}
public class ButtonManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] Panels;
    
    [SerializeField]
    private TextMeshProUGUI textUnitName;
    [SerializeField]
    private TextMeshProUGUI textUpgradeInfo;
    [SerializeField]
    private TextMeshProUGUI textCost;

    [SerializeField]
    private GameObject descriptionPanel;

    Action showInfo,descript; 
    private void Awake()
    {
        showInfo = OnSetUnitUpgradeInfo;
        descript = OnDescriptionActive;

        EventManager.StartListening("ActiveDescription", showInfo);
        EventManager.StartListening("ActiveDescription", descript);
    }
    private void CheckCurPanel()
    {
        for(int i =0; i< Panels.Length; i++)
        {
            if(Panels[i].activeSelf == true)
            {
                Panels[i].SetActive(false);
            }
        }
    }
    public void OnActiveDescription()
    {
        EventManager.TriggerEvent("ActiveDescription");
    }
    public void OnSetUnitUpgradeInfo()
    {
        StoreUnitInfo stricker = EventSystem.current.currentSelectedGameObject.GetComponent<StoreUnitInfo>();
        Debug.Log(stricker.name);
        textUnitName.text = stricker.name;
        textUpgradeInfo.text = stricker.upgradeInfo;
        textCost.text = string.Format("가격 {0} 원", stricker.upgradeInfo);
    }
    public void OnDescriptionActive() //창 닫을 때랑 유닛 클릭했을 때 활성화 두 가지 상황에서 쓸건데 활성화 할때는 이벤트 매니저 써서 OnShowUnit~~ 함수랑 연결시켜서 쓸거임 
    {
        descriptionPanel.SetActive(!descriptionPanel.activeSelf);
    }

    public void OnActiveStickerPanel()
    {
        CheckCurPanel();
        Panels[(int)PanelType.Sticker].SetActive(true);
    }
    public void OnActiveTtagjiPanel()
    {
        CheckCurPanel();
        Panels[(int)PanelType.Ttagji].SetActive(true);
    }
    public void OnActiveDalgonaPanel()
    {
        CheckCurPanel();
        Panels[(int)PanelType.Dalgona].SetActive(true);
    }
}
