using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro; 
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
    public void OnShowUnitUpgradeInfo()
    {
        StoreUnitInfo stricker = EventSystem.current.currentSelectedGameObject.GetComponent<StoreUnitInfo>();
        Debug.Log(stricker.name);
        textUnitName.text = stricker.name;
        textUpgradeInfo.text = stricker.upgradeInfo;
        textCost.text = string.Format("°¡°Ý {0} ¿ø", stricker.upgradeInfo);
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
