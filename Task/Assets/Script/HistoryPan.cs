using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HistoryPan : MonoBehaviour
{
    [SerializeField] Sprite Sprite_Win;
    [SerializeField] Sprite Sprite_Loose;
    [SerializeField] Sprite Sprite_SignHead;
    [SerializeField] Sprite Sprite_SignTail;

    [Header("Pan Elements")]
    [SerializeField] TextMeshProUGUI Txt_PanPlace;
    [SerializeField] Image Img_Bet;
    [SerializeField] Image Img_Result;

    int PanIdx;
    int HistoryDatas_Idx;
    bool PanBet_Sign;
    bool PanRestult_Sign;

    private void OnEnable() => SetPan();

    //Set History Pan----------------------------------------------------------------------------------------------------------------------------------------------------
    public void SetPan()
    {
        //Get Datas for Current HistoryPan*****************************************************************************
        PanIdx = transform.GetSiblingIndex();
        Txt_PanPlace.text = "#" + (PanIdx + 1).ToString();
        HistoryDatas_Idx = GameManager.Inst.GetHistoryLength()-1 - PanIdx;
        PanBet_Sign = Datas.current.BetsSign[HistoryDatas_Idx];
        PanRestult_Sign = Datas.current.ResultSign[HistoryDatas_Idx];

        //Set Pan Elements**********************************************************************************************
        GetComponent<Image>().sprite = PanBet_Sign == PanRestult_Sign ? Sprite_Win : Sprite_Loose;
        Img_Bet.sprite = PanBet_Sign ? Sprite_SignTail : Sprite_SignHead;
        Img_Result.sprite = PanRestult_Sign ? Sprite_SignTail : Sprite_SignHead;
    }
}
