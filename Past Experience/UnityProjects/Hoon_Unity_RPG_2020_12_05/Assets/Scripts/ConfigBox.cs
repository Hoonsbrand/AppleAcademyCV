using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigBox : MonoBehaviour
{
    InGame_Mgr m_GameMgr = null;

    public Button m_OK_Btn = null;
    public Button m_Close_Btn = null;

    public InputField IDInputField = null;

    public Toggle m_Sound_Toggle = null;
    public Slider m_TrSlider = null;

    // Start is called before the first frame update
    void Start()
    {
        m_GameMgr = FindObjectOfType<InGame_Mgr>();

        if (m_OK_Btn != null)
            m_OK_Btn.onClick.AddListener(OKBtnFunction);

        if (m_Close_Btn != null)
            m_Close_Btn.onClick.AddListener(CloseBtnFunction);

        // --- 각종 컨트롤들의 초기값 로딩 및 셋팅 부분
        Text a_Placeholder = null;
        if(IDInputField != null)
        {
            Transform a_PlhTr = IDInputField.transform.Find("Placeholder");
            a_Placeholder = a_PlhTr.GetComponent<Text>();
        }
        if(a_Placeholder != null)
        {
            a_Placeholder.text = PlayerPrefs.GetString("UserNick", "Hero");
        }
        // --- 각종 컨트롤들의 초기값 로딩 및 셋팅 부분

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OKBtnFunction()
    {

        if(IDInputField != null && IDInputField.text.Trim() != "")  // trim => 앞뒤 공백 delete (만약 닉네임이 다 공백이면 => 사용 불가)
        {
            string NickStr = IDInputField.text.Trim();

            if (m_GameMgr != null && m_GameMgr.m_refHero != null
                                 && m_GameMgr.m_refHero.m_NickName != null)
            {
                m_GameMgr.m_refHero.m_NickName.text = NickStr;
            }

            PlayerPrefs.SetString("UserNick", NickStr);
        }

        Time.timeScale = 1.0f;
        Destroy(this.gameObject);
    }

    void CloseBtnFunction()
    {
        Time.timeScale = 1.0f;
        Destroy(this.gameObject);
    }
}
