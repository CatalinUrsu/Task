using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Inst;

    [Header("History Elements")]
    [SerializeField] int MoveTime;
    [SerializeField] GameObject Obj_HistoryPan;
    [SerializeField] LeanTweenType InType;
    [SerializeField] LeanTweenType OutType;
    [SerializeField] Transform[] HistoryPan_Pos;

    [Header("UI Elements")]
    [SerializeField] TextMeshProUGUI Txt_Score;
    [SerializeField] TextMeshProUGUI Txt_LoseWin;
    [SerializeField] Image Img_TailHead;
    [SerializeField] GameObject Obj_Content;
    [SerializeField] GameObject Obj_Info;
    [SerializeField] Transform Pan_Btns;

    [Header("Aditional Elements")]
    [SerializeField] Color Col_Win;
    [SerializeField] Color Col_Lose;
    [SerializeField] Sprite Sprite_Tail;
    [SerializeField] Sprite Sprite_Head;
    [SerializeField] Sprite Sprite_BtnDefault;
    [SerializeField] Sprite Sprite_BtnWin;
    [SerializeField] Sprite Sprite_BtnLose;
    [SerializeField] GameObject Prefab_HistoryPan;

    int PanChecked_OnLoading = 0;
    int HisotryLength = 0;
    int HistoryPan_NeedCreate = 0;
    bool CanClick = true;

    //true - Tail, false - Head
    bool ChosedSign = true;
    bool ResultSign = true;
    float Randomize_Timer = 0;
    GameObject Obj_ChoosedSign;

    private void Awake()
    {
        Inst = this;
        SaveSystem.LoadGame();
        Set_ScoreTxt();
        StartCoroutine(EnumeratorCheckHistory());
    }

    void Set_ScoreTxt() => Txt_Score.text = Datas.current.Score.ToString();

    //Save info after each game------------------------------------------------------------------------------------------------------------------------------------------
    void SaveInfo()
    {
        //Set Score****************************************************************************************************
        Datas.current.Score += ChosedSign == ResultSign ? 100 : -100;
        if (Datas.current.Score < 0)
            Datas.current.Score = 0;

        Set_ScoreTxt();

        //Check if need to add History Pan or change the exist pannels*************************************************
        HisotryLength = 0;
        for (int i = 0; i < 20; i++)
            if (Datas.current.HistoryPanInfo[i] == true)
                HisotryLength++;

        if (HisotryLength < 20)
        {
            Datas.current.HistoryPanInfo[HisotryLength] = true;
            Datas.current.BetsSign[HisotryLength] = ChosedSign;
            Datas.current.ResultSign[HisotryLength] = ResultSign;

            HisotryLength++;
            AddHistoryPans();
        }
        else
        {
            //Set each pan info to next pan info, and last pan to current progress*************************************
            for (int i = 0; i < 19; i++)
            {
                Datas.current.BetsSign[i] = Datas.current.BetsSign[i + 1];
                Datas.current.ResultSign[i] = Datas.current.ResultSign[i + 1];
            }

            Datas.current.BetsSign[19] = ChosedSign;
            Datas.current.ResultSign[19] = ResultSign;
        }

        SaveSystem.SaveGame();
    }

    //Add new Pans in history if is not 20-------------------------------------------------------------------------------------------------------------------------------
    void AddHistoryPans()
    {
        if (Obj_Content.transform.childCount != HisotryLength)
        {
            HistoryPan_NeedCreate = HisotryLength - Obj_Content.transform.childCount;

            for (int i = 0; i < HistoryPan_NeedCreate; i++)
                Instantiate(Prefab_HistoryPan, Obj_Content.transform);
        }
    }

    #region Btn Controll
    //After Choose Sign, Desable other btn and start game----------------------------------------------------------------------------------------------------------------
    public void ChooseBtn(bool ChooseTail)
    {
        if (CanClick)
        {
            CanClick = false;
            Obj_Info.SetActive(false);

            for (int i = 0; i < Pan_Btns.childCount; i++)
                Pan_Btns.GetChild(i).gameObject.SetActive(false);

            ChosedSign = ChooseTail;
            Obj_ChoosedSign = Pan_Btns.transform.GetChild(ChooseTail ? 0 : 1).gameObject;
            Obj_ChoosedSign.SetActive(true);

            StartCoroutine(StartRandomize());
        }
    }

    public void Open_HistoryPan(bool Open)
    {
        if(CanClick)
        {
            CanClick = false;

            if (Open)
            {
                Obj_HistoryPan.SetActive(true);
                LeanTween.move(Obj_HistoryPan, HistoryPan_Pos[0], MoveTime).setEase(OutType).setOnComplete(() => { CanClick = true; });
            }
            else
            {
                LeanTween.move(Obj_HistoryPan, HistoryPan_Pos[1], MoveTime).setEase(InType).setOnComplete(() => { 
                    CanClick = true;
                    Obj_HistoryPan.SetActive(false);
                });
            }
        }
    }
    public void ClearHistory()
    {
        for (int i = 0; i < 20; i++)
        {
            Datas.current.HistoryPanInfo[i] = false;
            Destroy(Obj_Content.transform.GetChild(i).gameObject);
        }

        SaveSystem.SaveGame();
    }    
    #endregion

    #region GetValues
    public float GetHistoryCheck() => ((float)PanChecked_OnLoading) / 20;
    public int GetHistoryLength() => HisotryLength;
    #endregion

    #region Coroutines
    //Check if there are some saved datas in SaveSystem during 1 sec-----------------------------------------------------------------------------------------------------
    IEnumerator EnumeratorCheckHistory()
    {
        while (PanChecked_OnLoading < 20)
        {
            if (Datas.current.HistoryPanInfo[PanChecked_OnLoading] == true)
                HisotryLength++;

            PanChecked_OnLoading++;
            yield return new WaitForSeconds(0.05f);
        }

        AddHistoryPans();
    }

    //Start game, select random sign during 1 sec------------------------------------------------------------------------------------------------------------------------
    IEnumerator StartRandomize()
    {
        Randomize_Timer = 0;

        while (Randomize_Timer < 1)
        {
            ResultSign = !ResultSign;
            Img_TailHead.sprite = ResultSign ? Sprite_Tail : Sprite_Head;

            Randomize_Timer += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        ResultSign = Mathf.Round(Random.Range(0f, 1f)) == 0 ? true : false;
        Img_TailHead.sprite = ResultSign ? Sprite_Tail : Sprite_Head;

        StartCoroutine(WinLoose());
    }

    //Check if win or loose and change btn image-------------------------------------------------------------------------------------------------------------------------
    IEnumerator WinLoose()
    {
        Obj_ChoosedSign.GetComponent<Image>().sprite = ChosedSign == ResultSign ? Sprite_BtnWin : Sprite_BtnLose;
        Txt_LoseWin.gameObject.SetActive(true);
        Debug.Log("A");
        Txt_LoseWin.text = ChosedSign == ResultSign ? "Win" :"Lose";
        Txt_LoseWin.color = ChosedSign == ResultSign ? Col_Win : Col_Lose;

        yield return new WaitForSeconds(2);
        Obj_ChoosedSign.GetComponent<Image>().sprite = Sprite_BtnDefault;
        for (int i = 0; i < Pan_Btns.childCount; i++)
            Pan_Btns.GetChild(i).gameObject.SetActive(true);

        Txt_LoseWin.gameObject.SetActive(false);
        Obj_Info.SetActive(true);
        CanClick = true;
        SaveInfo();
    }
    #endregion
}