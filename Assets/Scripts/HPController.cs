using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPController : MonoBehaviour
{
    public static HPController instance;
    static int WinFish = 0;
    static int LoseFish = 0;

    public Text TextWin;
    public Text TextLose;

    public HPController()
    {
        if (instance == null) instance = this;
        else return;
    }
    

    public void UpdateWin()
    {
        WinFish++;
        TextWin.text = WinFish.ToString();
    }

    public void UpdateLose()
    {
        LoseFish++;
        TextLose.text = LoseFish.ToString();
    }
}
