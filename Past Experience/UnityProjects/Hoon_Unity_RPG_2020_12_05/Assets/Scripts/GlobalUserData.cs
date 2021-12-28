using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalUserData 
{
    public static InGame_Mgr g_GameMgr = null;
    public static Lobby_Mgr g_LobbyMgr = null;

    public static int m_SkillCount = 0;
    public static int m_GoldCount = 0;
    public static string g_NickName = "User";

    public static ulong UniqueCount = 0; // 임시 Item 고유키 발급기
    public static List<ItemValue> g_ItemList = new List<ItemValue>();

    public static void LoadGameInfo()
    {
        GlobalUserData.m_GoldCount = PlayerPrefs.GetInt("GoldCount", 0);
        GlobalUserData.m_SkillCount = PlayerPrefs.GetInt("SkillCount", 0);
        GlobalUserData.g_NickName = PlayerPrefs.GetString("UserNick", "Hero");

        g_GameMgr = null;
        GameObject a_GameMgrObj = GameObject.Find("InGame_Mgr");
        if (a_GameMgrObj != null)
        {
            g_GameMgr = a_GameMgrObj.GetComponent<InGame_Mgr>();
        }

        if (g_GameMgr != null)
            if (g_GameMgr.m_GoldTxt != null)
            {
                if (m_GoldCount <= 0)
                {
                    g_GameMgr.m_GoldTxt.text = "x 00";
                }
                else
                {
                    string a_GoldStr = string.Format("{0:N0}", m_GoldCount);
                    g_GameMgr.m_GoldTxt.text = "x " + a_GoldStr; //GlobalUserData.Instance.m_GoldCount.ToString();
                }
            }//if(m_GoldTxt != null)

        if (g_GameMgr != null)
            if (g_GameMgr.m_SkillTxt != null)
            {
                if (GlobalUserData.m_SkillCount <= 0)
                {
                    g_GameMgr.m_SkillTxt.text = "x 00";
                }
                else
                {
                    g_GameMgr.m_SkillTxt.text = "x " + m_SkillCount.ToString();
                }
            }//if(m_SkillTxt != null)

        g_LobbyMgr = null;
        GameObject a_LobbyMgrObj = GameObject.Find("Lobby_Mgr");
        if (a_LobbyMgrObj != null)
        {
            g_LobbyMgr = a_LobbyMgrObj.GetComponent<Lobby_Mgr>();
        }

        if (g_LobbyMgr != null)
            if (g_LobbyMgr.m_GoldTxt != null)
            {
                if (m_GoldCount <= 0)
                {
                    g_LobbyMgr.m_GoldTxt.text = "x 0";
                }
                else

                {
                    string a_GoldStr = string.Format("{0:N0}", m_GoldCount);
                    g_LobbyMgr.m_GoldTxt.text = "x " + a_GoldStr; //GlobalUserData.Instance.m_GoldCount.ToString();
                }
            }//if(g_LobbyMgr.m_GoldTxt != null)

        ReflashItemLoad();

    }//public static void LoadGameInfo()

    public static ulong GetUnique() // 임시 고유키 발급기
    {
        UniqueCount = (ulong)PlayerPrefs.GetInt("SvUnique", 0);
        UniqueCount++;
        ulong a_Index = UniqueCount;

        if(0 < g_ItemList.Count)
        {
            for (int a_bb = 0; a_bb < g_ItemList.Count; ++a_bb)
            {
                if (g_ItemList[a_bb] == null)
                    continue;

                if(a_Index < g_ItemList[a_bb].UniqueID)
                {
                    a_Index = g_ItemList[a_bb].UniqueID + 1;
                }
            }//for (int a_bb = 0; a_bb < g_ItemList.Count; ++a_bb)
        }//if(0 < g_ItemList.Count) 
        UniqueCount = a_Index;
        PlayerPrefs.SetInt("SvUnique", (int)UniqueCount);
        return a_Index;
    }

    public static void InvenAddItem(GameObject a_Obj)
    {
        ItemObjInfo a_RefItemInfo = a_Obj.GetComponent<ItemObjInfo>();
        if(a_RefItemInfo != null)
        {
            ItemValue a_Node = new ItemValue();
            a_Node.UniqueID = a_RefItemInfo.m_ItemValue.UniqueID;
            a_Node.m_Item_Type = a_RefItemInfo.m_ItemValue.m_Item_Type;
            a_Node.m_ItemName = a_RefItemInfo.m_ItemValue.m_ItemName;
            a_Node.m_ItemLevel = a_RefItemInfo.m_ItemValue.m_ItemLevel;
            a_Node.m_ItemStar = a_RefItemInfo.m_ItemValue.m_ItemStar;

            g_ItemList.Add(a_Node);

            g_GameMgr = null;
            GameObject a_GameMgrObj = GameObject.Find("InGame_Mgr");
            if(a_GameMgrObj != null)
            {
                g_GameMgr = a_GameMgrObj.GetComponent<InGame_Mgr>();
            }

            if(g_GameMgr != null)
            {
                g_GameMgr.AddNodeScrollView(a_Node);
            }
            //if (m_GameMgr != null)

             ReflashItemSave(); // 파일저장


        }
        //if(a_RefItemInfo != null)
    }

    //------------ Item Reflash
    public static void ReflashItemLoad()  //<---- g_ItemList  갱신
    {
        g_ItemList.Clear();

        ItemValue a_LdNode;
        string a_KeyBuff = "";
        int a_ItmCount = PlayerPrefs.GetInt("Item_Count", 0);
        for (int a_ii = 0; a_ii < a_ItmCount; a_ii++)
        {
            a_LdNode = new ItemValue();
            a_KeyBuff = string.Format("IT_{0}_stUniqueID", a_ii);
            string stUniqueID = PlayerPrefs.GetString(a_KeyBuff, "");
            if (stUniqueID != "")
                a_LdNode.UniqueID = ulong.Parse(stUniqueID);
            a_KeyBuff = string.Format("IT_{0}_Item_Type", a_ii);
            a_LdNode.m_Item_Type = (Item_Type)PlayerPrefs.GetInt(a_KeyBuff, 0);
            a_KeyBuff = string.Format("IT_{0}_ItemName", a_ii);
            a_LdNode.m_ItemName = PlayerPrefs.GetString(a_KeyBuff, "");
            a_KeyBuff = string.Format("IT_{0}_ItemLevel", a_ii);
            a_LdNode.m_ItemLevel = PlayerPrefs.GetInt(a_KeyBuff, 0);
            a_KeyBuff = string.Format("IT_{0}_ItemStar", a_ii);
            a_LdNode.m_ItemStar = PlayerPrefs.GetInt(a_KeyBuff, 0);

            g_ItemList.Add(a_LdNode);
        }
    }

    public static void ReflashItemSave()  //<-- 리스트 다시 저장
    {
        //---------기존에 저장되어 있었던 아이템 목록 제거
        ItemValue a_SvNode;
        string a_KeyBuff = "";
        int a_ItmCount = PlayerPrefs.GetInt("Item_Count", 0);
        for (int a_ii = 0; a_ii < a_ItmCount + 10; a_ii++)
        {
            a_KeyBuff = string.Format("IT_{0}_stUniqueID", a_ii);
            PlayerPrefs.DeleteKey(a_KeyBuff);
            a_KeyBuff = string.Format("IT_{0}_Item_Type", a_ii);
            PlayerPrefs.DeleteKey(a_KeyBuff);
            a_KeyBuff = string.Format("IT_{0}_ItemName", a_ii);
            PlayerPrefs.DeleteKey(a_KeyBuff);
            a_KeyBuff = string.Format("IT_{0}_ItemLevel", a_ii);
            PlayerPrefs.DeleteKey(a_KeyBuff);
            a_KeyBuff = string.Format("IT_{0}_ItemStar", a_ii);
            PlayerPrefs.DeleteKey(a_KeyBuff);
        }
        PlayerPrefs.DeleteKey("Item_Count");  //아이템 수 제거
        PlayerPrefs.Save(); //폰에서 마지막 저장상태를 확실히 저장하게 하기 위하여...
                            //---------기존에 저장되어 있었던 아이템 목록 제거

        //---------- 새로운 리스트 저장
        PlayerPrefs.SetInt("Item_Count", g_ItemList.Count);
        for (int a_ii = 0; a_ii < g_ItemList.Count; a_ii++)
        {
            a_SvNode = g_ItemList[a_ii];
            a_KeyBuff = string.Format("IT_{0}_stUniqueID", a_ii);
            PlayerPrefs.SetString(a_KeyBuff, a_SvNode.UniqueID.ToString());
            a_KeyBuff = string.Format("IT_{0}_Item_Type", a_ii);
            PlayerPrefs.SetInt(a_KeyBuff, (int)a_SvNode.m_Item_Type);
            a_KeyBuff = string.Format("IT_{0}_ItemName", a_ii);
            PlayerPrefs.SetString(a_KeyBuff, a_SvNode.m_ItemName);
            a_KeyBuff = string.Format("IT_{0}_ItemLevel", a_ii);
            PlayerPrefs.SetInt(a_KeyBuff, a_SvNode.m_ItemLevel);
            a_KeyBuff = string.Format("IT_{0}_ItemStar", a_ii);
            PlayerPrefs.SetInt(a_KeyBuff, a_SvNode.m_ItemStar);
        }
        PlayerPrefs.Save(); //폰에서 마지막 저장상태를 확실히 저장하게 하기 위하여...
        //---------- 새로운 리스트 저장
    }
    //------------ Item Reflash

    public static void ClearGameInfo()
    {
        PlayerPrefs.DeleteAll();
        LoadGameInfo();
    }


}
