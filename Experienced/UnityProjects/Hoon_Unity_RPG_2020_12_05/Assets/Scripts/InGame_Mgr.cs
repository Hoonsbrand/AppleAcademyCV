using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

enum JoyStickType
{
    Fixed = 0,                   // m_JoySBackObj.activeSelf == true && m_JoyStickPanel.activeSelf == false
    Flexible = 1,                // m_JoySBackObj.activeSelf == true && m_JoyStickPanel.activeSelf == true
    FlexibleOnOff = 2            // m_JoySBackObj.activeSelf == false && m_JoyStickPanel.activeSelf == true
}

public class InGame_Mgr : MonoBehaviour
{
    public Button m_BackBtn;

    [HideInInspector] public HeroCtrl m_refHero = null;
    Vector3 a_PickVec = Vector3.zero;

    // 마우스 이동 클릭 마크 제어 변수
    public ClickMark m_ClickMark = null;
    Vector3 a_CacVLen = Vector3.zero;
    // 마우스 이동 클릭 마크 제어 변수

    public static GameObject m_BulletObj = null;
    float m_AttSpeed = 0.1f; // 주인공 공속
    float m_CacAtTick = 0.0f; // 기관총 발사 틱 만들기

    // ------------------------ UserInfo UI 관련 변수
    private bool m_UInfo_OnOff = false;  // 플래그 변수
    [Header("-------- UserInfo UI --------")]
    public Button m_UserInfo_Btn = null;
    public GameObject m_UserInfoPannel = null;
    public Text m_UserHPTxt;
    public Text m_SkillTxt;
    public Text m_MonKillTxt;
    public Text m_GoldTxt;

    int m_MonKillCount = 0; // 몬스터 킬수 변수
    // ------------------------ UserInfo UI 관련 변수

    JoyStickType m_JoyStickType = JoyStickType.Fixed;

    // ------------------------ Fixed JoyStick 처리부분
    [Header("-------- Fixed JoyStick --------")]
    public GameObject m_JoySBackObj = null;
    public Image m_JoyStickImg = null;
    float m_Radius = 0.0f;
    Vector3 m_OrignPos = Vector3.zero;
    Vector3 m_Axis = Vector3.zero;
    Vector3 m_JsCacVec = Vector3.zero;
    float m_JsCacDist = 0.0f;
    // ------------------------ Fixed JoyStick 처리부분

    // ------------------------ Flexible JoyStick 처리부분
    [Header("---------- Flexible JoyStick ----------")]
    public GameObject m_JoyStickPickPanel = null;
    private Vector2 posJoyBack;
    private Vector2 dirStick;
    // ------------------------ Flexible JoyStick 처리부분

    // ------------------------ 머리위에 데미지 띄우기용 변수 선언
    Vector3 a_StCacPos = Vector3.zero;
    [Header("--------- Damage Text ----------")]
    public Transform m_HUD_Canvas = null;
    public GameObject m_DamageObj = null;
    // ------------------------ 머리위에 데미지 띄우기용 변수 선언

    public static CameraCtrl RefCamCtrl = null;

    public Texture[] m_ItemImg = null;

    // ---------------- Inventory ScrollView OnOff
    [Header("---------- Inventory OnOff ----------")]
    public Button m_InVen_Btn = null;
    public Transform m_InVenScrollTr = null;
    public bool m_InVen_ScOnOff = false;
    private float m_ScSpeed = 2500.0f;
    private Vector3 m_ScOnPos = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 m_ScOffPos = new Vector3(470.0f, 0.0f, 0.0f);

    public Transform m_MkIvenContent = null;
    public GameObject m_MkItemMyNode = null;
    public Button m_ItemSell_Btn = null;
    // ---------------- Inventory ScrollView OnOff




    // -------------------- 환경설정 dlg 관련 변수
    [Header("---------- ConfigBox ----------")]
    public Button m_Cfgbtn = null;
    public GameObject Canvas_Dialog = null;
    private GameObject m_ConfigBoxObj = null;
    // -------------------- 환경설정 dlg 관련 변수



    // Start is called before the first frame update
    void Start()
    {
        GlobalUserData.LoadGameInfo();
        ReflashInGameItemScV();

        RefCamCtrl = FindObjectOfType<CameraCtrl>();

        if (m_BackBtn != null)
            m_BackBtn.onClick.AddListener(() => {
                UnityEngine.SceneManagement.SceneManager.LoadScene("LobbyScene");
            });

        m_refHero = FindObjectOfType<HeroCtrl>();

        m_BulletObj = Resources.Load("BulletObj") as GameObject;

        //GameObject a_HObj = GameObject.Find("HeroRoot");
        //if(a_HObj != null)
        //{
        //    a_HObj.GetComponent<HeroCtrl>();
        //}

        // ----- 유저 정보창 켜고 끄기
        m_UInfo_OnOff = m_UserInfoPannel.activeSelf; // 체크상태 확인해서 밑의 코드 작동 (게임 시작 시 켜져있나 꺼져있나)

        if(m_UserInfo_Btn != null && m_UserInfoPannel != null)
        {
            m_UserInfo_Btn.onClick.AddListener(() =>
            {
                m_UInfo_OnOff = !m_UInfo_OnOff;        // bool 형의 변수가 반전되는 것
                m_UserInfoPannel.SetActive(m_UInfo_OnOff);
            });
        }
        // ----- 유저 정보창 켜고 끄기

        // ----------- Fixed JoyStick 처리 부분
        if (m_JoySBackObj != null && m_JoyStickImg != null && m_JoySBackObj.activeSelf == true && m_JoyStickPickPanel.activeSelf == false)  // 조이스틱UI 연결여부
        {
            m_JoyStickType = JoyStickType.Fixed; // enum 

            Vector3[] v = new Vector3[4];
            m_JoySBackObj.GetComponent<RectTransform>().GetWorldCorners(v); // GetWorldCorners => 이미지들은 모두 사각형으로 되어있는데 사각형의 각 꼭짓점을 가져오는것 v변수가 값을 받아온다.
            // [0] : 좌측하단, [1] : 좌측상단, [2] : 우측상단, [3] : 우측하단
            // v[0] 좌측하단이 0, 0 좌표인 스크린 좌표 (Screen.width, Screen.height (현재 핸드폰의 해상도를 얻어옴))를 기준으로
            m_Radius = v[2].y - v[0].y;    // v[0]을 쓴 이유? => x,y의 min 값 (v[0]이 제일 작음) (사각형을 그려 생각해보자)
            m_Radius = m_Radius / 3.0f;    // 3등분을 안했을때 조이스틱의 반경이 너무 커서 조절해 준것 뿐이다. 조이스틱의 반경 제한

            m_OrignPos = m_JoyStickImg.transform.position;  // 움직이다가 손가락을 떼는 순간 원래위치로 돌아가게 하기 위해 저장한 좌표

            // 스크립트로만 대기하고자 할 때
            EventTrigger trigger = m_JoySBackObj.GetComponent<EventTrigger>();
            // Inspector 에서 GameObject.Find("Button");
            // 에 꼭 AddComponent --> EventTrigger 가 되어 있어야 한다.
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.Drag;
            entry.callback.AddListener((data) => { OnDragJoyStick((PointerEventData)data); });
            trigger.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.EndDrag;
            entry.callback.AddListener((data) => { OnEndDragJoyStick((PointerEventData)data); });
            trigger.triggers.Add(entry);

            // 구글링
                

        }
        // ----------- Fixed JoyStick 처리 부분

        // ----------- Flexible JoyStick 처리 부분
        if(m_JoyStickPickPanel != null && m_JoySBackObj != null
            && m_JoyStickImg != null
            && m_JoyStickPickPanel.activeSelf == true) // 예외처리
        {
            if(m_JoySBackObj.activeSelf == true)
            {
                m_JoyStickType = JoyStickType.Flexible;
            }
            else
            {
                m_JoyStickType = JoyStickType.FlexibleOnOff; // 조건에 따라 모드 선택
            }

            EventTrigger a_JBTrigger = m_JoySBackObj.GetComponent<EventTrigger>();
            if(a_JBTrigger != null)
            {
                Destroy(a_JBTrigger); // 하얀색 마스크에서 움직임을 제어함 => 조이스틱UI는 이미지로만 동작하게 만듬(이벤트제거)
            } // 조이스틱 백에 설치되어 있는 이벤트 트리거는 제거한다.

            Vector3[] v = new Vector3[4];
            m_JoySBackObj.GetComponent<RectTransform>().GetWorldCorners(v); // GetWorldCorners => 이미지들은 모두 사각형으로 되어있는데 사각형의 각 꼭짓점을 가져오는것 v변수가 값을 받아온다.
            // [0] : 좌측하단, [1] : 좌측상단, [2] : 우측상단, [3] : 우측하단
            // v[0] 좌측하단이 0, 0 좌표인 스크린 좌표 (Screen.width, Screen.height (현재 핸드폰의 해상도를 얻어옴))를 기준으로
            m_Radius = v[2].y - v[0].y;    // v[0]을 쓴 이유? => x,y의 min 값 (v[0]이 제일 작음) (사각형을 그려 생각해보자)
            m_Radius = m_Radius / 3.0f;    // 3등분을 안했을때 조이스틱의 반경이 너무 커서 조절해 준것 뿐이다. 조이스틱의 반경 제한

            m_OrignPos = m_JoyStickImg.transform.position;
            m_JoySBackObj.GetComponent<Image>().raycastTarget = false;
            m_JoyStickImg.raycastTarget = false; // ?????????????????

            EventTrigger trigger = m_JoyStickPickPanel.GetComponent<EventTrigger>();
            // Inspector에서 m_JoyStickPickPanel 에 꼭 AddComponent --> EventTrigger가 되어있어야한다

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((data) => { OnPointerDown_Flx((PointerEventData)data); });
            trigger.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerUp;
            entry.callback.AddListener((data) => { OnPointerUp_Flx((PointerEventData)data); });
            trigger.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.Drag;
            entry.callback.AddListener((data) => { OnDragJoyStick_Flx((PointerEventData)data); });
            trigger.triggers.Add(entry);


        }
        // ----------- Flexible JoyStick 처리 부분

        // -------------------- ScrollView OnOff
        if(m_InVen_Btn != null)
        {
            m_InVen_Btn.onClick.AddListener(
                () =>
                {
                    m_InVen_ScOnOff = !m_InVen_ScOnOff;
                    if (m_ItemSell_Btn != null)
                        m_ItemSell_Btn.gameObject.SetActive(m_InVen_ScOnOff);
                }
                );
        }
        // -------------------- ScrollView OnOff

        // --------------- 아이템 판매 처리
        if (m_ItemSell_Btn != null)
            m_ItemSell_Btn.onClick.AddListener(ItemSellMethod);
        // --------------- 아이템 판매 처리

        // ----------------------- 환경설정 dlg 관련 구현 부분
        if (m_Cfgbtn != null)
            m_Cfgbtn.onClick.AddListener(() =>
            {
                if (m_ConfigBoxObj == null)
                    m_ConfigBoxObj = Resources.Load("Prefabs/ConfigBox") as GameObject;

                GameObject a_CfgBoxObj = (GameObject)Instantiate(m_ConfigBoxObj);
                a_CfgBoxObj.transform.SetParent(Canvas_Dialog.transform, false);
                // false로 해야 로컬 프리팹에 설정된 좌표를 유지한 채 차일드로 붙게된다.
                Time.timeScale = 0.0f;
            });
        // ----------------------- 환경설정 dlg 관련 구현 부분


    } // void Start()

    // Update is called once per frame
    void Update()
    {
        m_CacAtTick = m_CacAtTick - Time.deltaTime;
        if (m_CacAtTick <= 0.0f)
            m_CacAtTick = 0.0f;

        // ---------- 총알 발사 코드
        if(0.0f < Time.timeScale)
            if(Input.GetMouseButton(1))
            {
                if(m_CacAtTick <= 0.0f)
                {
                    m_refHero.MsShooting(Camera.main.ScreenToWorldPoint(Input.mousePosition));

                    m_CacAtTick = m_AttSpeed;
                }
            } // if(m_JsCacDist == 0.0f && Input.GetMouseButton(1))
        // ---------- 총알 발사 코드

        // ------------------- 마우스 이동 코드
        if (Input.GetMouseButtonDown(0))
        {
            if (MouseHover.instance.isUIHover == false)
                //if (IsPointerOverUIObject() == false)        
            {
                a_PickVec = Camera.main.ScreenToWorldPoint(Input.mousePosition); // ray, raycast 와의 차이점 정리해보기 ->
                m_refHero.MsPicking(a_PickVec);

                if (m_ClickMark != null)
                {
                    m_ClickMark.transform.position = new Vector3(a_PickVec.x, 0.3f, a_PickVec.z);
                    m_ClickMark.gameObject.SetActive(true);
                    m_ClickMark.ResetEff();
                }
            }

        }

        // ------------------- 마우스 이동 코드

        CLMarkUpdate(); // -- 클릭마크 끄기

        SvActionUpdate(); // <-- ScrollView OnOff 연출

    }

    void CLMarkUpdate()
    {
        // -- 클릭마크 끄기
        if (m_ClickMark != null &&
            m_ClickMark.gameObject.activeSelf == true)
        {
            if (m_refHero != null) // 아직 죽지 않았을 때
            {
                a_CacVLen = m_refHero.transform.position - m_ClickMark.transform.position;

                a_CacVLen.y = 0.0f;
                if(a_CacVLen.magnitude < 1.0f)
                {
                    m_ClickMark.gameObject.SetActive(false);
                }

                if(m_refHero.IsKJMove() == true)
                {
                    m_ClickMark.gameObject.SetActive(false);
                }
            } // if(m_TankComp != null) 아직 죽지 않았을 때
            else
            {
                m_ClickMark.gameObject.SetActive(false);
            }
        } /*if(m_RTS_Arrow != null)*/
            // -- 클릭마크 끄기
    }

    //    PointerEventData a_EDCurPos; // using UnityEngine.EventSystems;
    //    public bool IsPointerOverUIObject() //UGUI의 UI들이 먼저 피킹되는지 확인하는 함수
    //    {
    //        a_EDCurPos = new PointerEventData(EventSystem.current);

    //#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID)

    //            //using System.Collections.Generic;
    //            List<RaycastResult> results = new List<RaycastResult>();
    //            for (int i = 0; i < Input.touchCount; ++i)
    //            {
    //                a_EDCurPos.position = Input.GetTouch(i).position;  //new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    //                results.Clear();
    //                EventSystem.current.RaycastAll(a_EDCurPos, results);
    //                if (0 < results.Count)
    //                    return true;
    //            }

    //            return false;
    //#else
    //        a_EDCurPos.position = Input.mousePosition;
    //        //using System.Collections.Generic;
    //        List<RaycastResult> results = new List<RaycastResult>();
    //        EventSystem.current.RaycastAll(a_EDCurPos, results);
    //        return results.Count > 0;
    //#endif
    //    }

    // ------------------------ Fixed JoyStick 처리부분

    void OnDragJoyStick(PointerEventData _data) // Delegate
    {
        if (m_JoyStickImg == null)
            return;

        m_JsCacVec = Input.mousePosition - m_OrignPos; // 조이스틱을 움직인 벡터길이
        m_JsCacVec.z = 0.0f;                           // z축 안씀
        m_JsCacDist = m_JsCacVec.magnitude;
        m_Axis = m_JsCacVec.normalized; // 방향

        // 조이스틱 백드라운드를 벗어나지 못하게 막는 부분
        if(m_Radius < m_JsCacDist) // 높이값을 못넘어가게 함
        {
            m_JoyStickImg.transform.position = m_OrignPos + m_Axis * m_Radius;
        }
        else
        {
            m_JoyStickImg.transform.position = m_OrignPos + m_Axis * m_JsCacDist; // 원래위치 + 방향 * 거리
        }

        if (1.0f < m_JsCacDist)
            m_JsCacDist = 1.0f; // 최대 땡겼을때 1.0

        // 캐릭터 이동 처리
        if (m_refHero != null)
            m_refHero.SetJoyStickMv(m_JsCacDist, m_Axis);

    } //  void OnDragJoyStick(PointerEventData _data)

    void OnEndDragJoyStick(PointerEventData _data) // Delegate
    {
        if (m_JoyStickImg == null)
            return;

        m_Axis = Vector3.zero;
        m_JoyStickImg.transform.position = m_OrignPos;

        m_JsCacDist = 0.0f;

       // 캐릭터 정지 처리
         if (m_refHero != null)
            m_refHero.SetJoyStickMv(0.0f, m_Axis); // 안땡겼을 때 0.0
    }
    // ------------------------ Fixed JoyStick 처리부분

    // ----------- Flexible JoyStick 처리 부분
    void OnPointerDown_Flx(PointerEventData eventData) // Delegate
    {
        if (eventData.button != PointerEventData.InputButton.Left) // 마우스 왼쪽 버튼만
            return;

        if (m_JoySBackObj == null)
            return;

        if (m_JoyStickImg == null)
            return;

        m_JoySBackObj.transform.position = eventData.position;  // eventData.position이 클릭한 위치고, 그 위치에서 조이스틱을 시작하겠다
        m_JoyStickImg.transform.position = eventData.position;

       m_JoySBackObj.SetActive(true);
    }

    void OnPointerUp_Flx(PointerEventData eventData) // Delegate
    {
        if (eventData.button != PointerEventData.InputButton.Left) // 마우스 왼쪽버튼만
            return;

        if (m_JoySBackObj == null)
            return;

        if (m_JoyStickImg == null)
            return;

        m_JoySBackObj.transform.position = m_OrignPos;
        m_JoyStickImg.transform.position = m_OrignPos;

        if(m_JoyStickType == JoyStickType.FlexibleOnOff)
        {
            m_JoySBackObj.SetActive(false); // <-- 꺼진 상태로 시작하는 방식일 때는 활성화 필요
        }

        m_Axis = Vector3.zero;
        m_JsCacDist = 0.0f;
        //m_JoyStickImg.gameObject.SetActive(false);
        //캐릭터 정지 처리
        if(m_refHero != null)
        {
            m_refHero.SetJoyStickMv(0.0f, Vector3.zero);
        }
    }

    void OnDragJoyStick_Flx(PointerEventData eventData) // Delegate
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        // eventData.position 현재 마우스의 월드 좌표

        if (m_JoyStickImg == null)
            return;

        posJoyBack = (Vector2)m_JoySBackObj.transform.position;
        // 조이스틱 백 그라운드 현재 위치 기준
        m_JsCacDist = Vector2.Distance(posJoyBack, eventData.position); // 거리
        dirStick = eventData.position - posJoyBack; // 방향

        if(m_Radius < m_JsCacDist)
        {
            m_JsCacDist = m_Radius;
            m_JoyStickImg.transform.position = (Vector3)(posJoyBack + (dirStick.normalized * m_Radius));
        }
        else
        {
            m_JoyStickImg.transform.position = (Vector3)eventData.position;
        }

        if (1.0f < m_JsCacDist)
            m_JsCacDist = 1.0f;

        m_Axis = (Vector3)dirStick.normalized;

        if (m_refHero != null)
        {
            m_refHero.SetJoyStickMv(m_JsCacDist, m_Axis);
        }
    }
    // ----------- Flexible JoyStick 처리 부분

    public void ReflashUserInfoHP()
    {
       
        m_UserHPTxt.text = "HP : " + m_refHero.m_CurHP.ToString() + " / " +
                                     m_refHero.m_MaxHP.ToString();
    }

    public void AddMonKill(int a_Val = 1)
    {
        m_MonKillCount = m_MonKillCount + a_Val;
        m_MonKillTxt.text = "x " + m_MonKillCount.ToString();
    }

    public void AddGold(int a_Val = 100)
    {
        GlobalUserData.m_GoldCount = GlobalUserData.m_GoldCount + a_Val;
        string a_GoldStr = string.Format("{0:N0}", GlobalUserData.m_GoldCount);
        m_GoldTxt.text = "x " + a_GoldStr;

        PlayerPrefs.SetInt("GoldCount", GlobalUserData.m_GoldCount); // 값 저장
    }

    public void AddSkill(int a_Val = 1)
    {
        GlobalUserData.m_SkillCount = GlobalUserData.m_SkillCount + a_Val;
        m_SkillTxt.text = "x " + GlobalUserData.m_SkillCount.ToString();

        PlayerPrefs.SetInt("SkillCount", GlobalUserData.m_SkillCount); // 값 저장
    }

    public void DamageTxt(int a_Value, Transform txtTr)
    {
        GameObject a_DamClone = (GameObject)Instantiate(m_DamageObj);
        if(a_DamClone != null && m_HUD_Canvas != null)
        {
            a_StCacPos = new Vector3(txtTr.position.x, 0.8f, txtTr.position.z + 4.0f);

            a_DamClone.transform.SetParent(m_HUD_Canvas);
            DamageText a_DamageTx = a_DamClone.GetComponent<DamageText>();
            a_DamageTx.m_SavePos = a_StCacPos;
            a_DamageTx.m_DamageVal = (int)a_Value;
            a_DamClone.transform.position = a_StCacPos;
        }
    }

    void SvActionUpdate()
    {
        // ------------ Menu Scroll 연출
        if(m_InVen_ScOnOff == false)
        {
            if(m_InVenScrollTr != null)
                if(m_InVenScrollTr.localPosition.x < m_ScOffPos.x)
                {
                    m_InVenScrollTr.localPosition =
                        Vector3.MoveTowards(m_InVenScrollTr.localPosition,
                                      m_ScOffPos, m_ScSpeed * Time.deltaTime);
                }
        }
        else
        {
            if (m_InVenScrollTr != null)
                if (m_ScOnPos.x < m_InVenScrollTr.localPosition.x)
                {
                    m_InVenScrollTr.localPosition =
                        Vector3.MoveTowards(m_InVenScrollTr.localPosition,
                                      m_ScOnPos, m_ScSpeed * Time.deltaTime);
                }
        }
        // ------------ Menu Scroll 연출

    }

    public void AddNodeScrollView(ItemValue a_Node)
    {
        GameObject m_ItemObj = (GameObject)Instantiate(m_MkItemMyNode);

        m_ItemObj.transform.SetParent (m_MkIvenContent, false);
        // false 일 경우 : 로컬 기준의 정보를 유지한 채 차일드화된다.
        ItemNode a_MyItemInfo = m_ItemObj.GetComponent<ItemNode>();

        if (a_MyItemInfo != null)
            a_MyItemInfo.SetItemRsc(a_Node, this);

        m_MkIvenContent.GetComponent<RectTransform>().pivot = new Vector2(0.0f, 1.0f);
    }

    void ItemSellMethod()
    {
        //스크롤뷰의 노드를 모두 돌면서 선택되어 있는 것들만 판매하고, 
        //해당 유니크ID를 g_ItemList에서 찾아서 제거해 준다.
        ItemNode[] a_MyNodeList =
            m_MkIvenContent.GetComponentsInChildren<ItemNode>(true);
        for (int i = 0; i< a_MyNodeList.Length; i++)
        {
            if (a_MyNodeList[i].m_SelOnOff == false)
                continue;

            for (int a_bb = 0; a_bb < GlobalUserData.g_ItemList.Count; a_bb++)
            {
                if(a_MyNodeList[i].m_UniqueID ==
                                    GlobalUserData.g_ItemList[a_bb].UniqueID)
                {
                    GlobalUserData.g_ItemList.RemoveAt(a_bb);
                    break;
                }
            } // for(int a_bb = 0; a_bb < GlobalUserData.Instance.g_ItemList.Count; a_bb++)

            Destroy(a_MyNodeList[i].gameObject);

            AddGold(100); // 골드 증가

        } //for(int i = 0; i < a_MyNodeList.Length; i++)

        GlobalUserData.ReflashItemSave(); // 리스트 다시 저장
    }// void ItemSellMethod()

    public void ReflashInGameItemScV() // <---- InGame의 ScrollView 갱신
    {
        ItemNode[] a_MyNodeList =
            m_MkIvenContent.GetComponentsInChildren<ItemNode>(true);

        for (int i = 0; i < a_MyNodeList.Length; i++)
        {
            Destroy(a_MyNodeList[i].gameObject);
        } 

        for(int a_i = 0; a_i < GlobalUserData.g_ItemList.Count; a_i++)
        {
            AddNodeScrollView(GlobalUserData.g_ItemList[a_i]);
        }
    }

}
