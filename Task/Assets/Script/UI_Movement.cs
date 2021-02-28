using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Movement : MonoBehaviour
{
    public static UI_Movement Inst;

    [SerializeField] Transform[] HistoryPan_Pos;

    private void Awake()
    {
        Inst = this;
    }


}
