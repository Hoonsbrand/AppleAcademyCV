using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby_Mgr : MonoBehaviour
{
    public Button m_GameStartBtn;
    public Button m_ReSet_Save_Btn;

    public Text m_GoldTxt;

    // Start is called before the first frame update
    void Start()
    {
        GlobalUserData.LoadGameInfo();

        if (m_GameStartBtn != null)
            m_GameStartBtn.onClick.AddListener(
            () =>                //람다식
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("InGame");
            }
            );

        if (m_ReSet_Save_Btn != null)
            m_ReSet_Save_Btn.onClick.AddListener(() =>
            {
                GlobalUserData.ClearGameInfo();
            });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
