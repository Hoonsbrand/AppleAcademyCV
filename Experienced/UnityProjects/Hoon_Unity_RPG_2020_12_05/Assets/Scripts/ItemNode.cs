using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemNode : MonoBehaviour
{
    [HideInInspector] public ulong m_UniqueID = 0;
    public Text m_TextInfo = null;

    [HideInInspector] public bool m_SelOnOff = false;
    public Image m_SelImage = null;
    private Button m_SelBtn = null;
    // Start is called before the first frame update
    void Start()
    {
        m_SelBtn = gameObject.GetComponent<Button>();
        if (m_SelBtn != null)
            m_SelBtn.onClick.AddListener(() =>
            {
                m_SelOnOff = !m_SelOnOff;

                if (m_SelImage == null)
                    return;

                m_SelImage.gameObject.SetActive(m_SelOnOff);
            }
                );
        
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    public void SetItemRsc(ItemValue a_Node, InGame_Mgr a_GameMgr)
    {
        if (a_Node == null || a_GameMgr == null)
            return;

        Transform a_FindObj = gameObject.transform.Find("RawImage");
        if(a_FindObj != null)
        {
            if(Item_Type.IT_armor <= a_Node.m_Item_Type
                && a_Node.m_Item_Type <= Item_Type.IT_helmets)
            {
                a_FindObj.GetComponent<RawImage>().texture
                    = a_GameMgr.m_ItemImg[(int)a_Node.m_Item_Type];
            }
        } // if(a_FindObj != null)

        if(m_TextInfo != null)
        {
            m_TextInfo.text = "Lv(" + a_Node.m_ItemLevel.ToString() + ")";
        }

        m_UniqueID = a_Node.UniqueID;
    }

}
