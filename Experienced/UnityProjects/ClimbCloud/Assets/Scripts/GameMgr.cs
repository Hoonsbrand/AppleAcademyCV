using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour
{
    public Text m_AppleText = null;
    int AppleCount = 0;

    public Text m_BananaText = null; // 초기화
    int BananaCount = 0;

    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    public void GetItem(int a_Type, int Num = 1)
    {
        if(a_Type == 1)
        {
            AppleCount += Num;
            m_AppleText.text = "x " + AppleCount.ToString();
        }

        else
        {
            BananaCount += Num;
            m_BananaText.text = "x " + BananaCount.ToString();
        }
    }

    public void ReSetItem()
    {
        AppleCount = 0;
        BananaCount = 0;
        m_AppleText.text = "x " + AppleCount.ToString();
        m_BananaText.text = "x " + BananaCount.ToString();
    }
}
