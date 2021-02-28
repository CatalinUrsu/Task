using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Datas
{
    public static Datas current = new Datas();

    public int Score = 0;
    
    #region History Datas
    //max 20 elementsz
    // false - no datas, true - datas exist
    public bool[] HistoryDatasAmount = { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
    // false - Head, true - Tail
    public bool[] BetsSign = { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
    public bool[] ResultSign = { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
    #endregion
}
